using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineService.App.Models;
using OnlineService.Library;

namespace OnlineService.App.Controllers
{
    public class OrderController : Controller
    {
        private readonly Library.Interfaces.IServiceRepository repo;
        static private Customer client = null;
        static private Location locale = null;
        static private List<Item> items = null;
        static private Dictionary<int, uint> counts = null;

        public OrderController(Library.Interfaces.IServiceRepository _repo)
        {
            repo = _repo ?? throw new ArgumentNullException(nameof(_repo));
        }

        public IActionResult SetUp(int customerId)
        {
            client = null;
            locale = null;
            items = null;
            counts = null;
            try
            {
                client = repo.GetCustomerById(customerId);
                items = new List<Item>();
                counts = new Dictionary<int, uint>();
            }
            catch (Exception)
            {
                RedirectToAction("Index", "Customer");
            }
            
            return RedirectToAction(nameof(LocationPicking));
        }

        public IActionResult LocationPicking([FromQuery]string search = "")
        {
            return View(repo.GetLocations(search));
        }

        public IActionResult ChoseLocation(int id)
        {
            try
            {
                locale = repo.GetLocationById(id);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(LocationPicking));
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Index([FromQuery]string search = "")
        {
            NewOrderViewModel val = new NewOrderViewModel
            {
                CustomerName = client.FullName,
                LocationName = locale.Name,
                AvailableItems = repo.GetItems(search).OrderBy(x => x.Name).ToList(),
                Purchased = items,
                Counts = counts
            };
            return View(val);
        }

        public IActionResult Back()
        {
            client = null;
            locale = null;
            return RedirectToAction("Index", "Customer");
        }

        public IActionResult Orders(int id)
        {
            Order order = repo.GetOrderById(id);

            return View(order);
        }

        public IActionResult SelectItemCount(int id)
        {
            try
            {
                AddingViewModel model = new AddingViewModel
                {
                    ItemId = id,
                    ItemName = repo.GetItemById(id).Name,
                    ItemCount = 0
                };
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SelectItemCount(AddingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // there was a server-side validation error - return the user a new
                // form with what he put in.
                return View(model);
            }

            if (items.FirstOrDefault(x => x.Id == model.ItemId) is Item item)
            {
                uint count = counts[item.Id] + model.ItemCount;
                if (count > 10)
                {
                    model.ItemCount = 10 - counts[item.Id];
                    return View(model);
                }
            }

            try
            {
                locale.AddToOrder(client, model.ItemId, model.ItemCount, repo);
                if (items.FirstOrDefault(x => x.Id == model.ItemId) != null)
                {
                    counts[model.ItemId] += model.ItemCount;
                }
                else
                {
                    items.Add(repo.GetItemById(model.ItemId));
                    counts.TryAdd(model.ItemId, model.ItemCount);
                }
            }
            catch (ArgumentException ex)
            {
                //ModelState.AddModelError("Title", "title error");

                // empty string for "key" means, it's overall model error
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Submit()
        {
            try
            {
                locale.PlaceOrder(repo);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction(nameof(Index));
            }
            items = null;
            counts = null;
            client = null;
            locale = null;
            return RedirectToAction("Index", "Customer");
        }
    }
}
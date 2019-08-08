using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineService.Library;

namespace OnlineService.App.Controllers
{
    public class LocationController : Controller
    {
        private readonly Library.Interfaces.IServiceRepository repo;

        public LocationController(Library.Interfaces.IServiceRepository _repo)
        {
            repo = _repo ?? throw new ArgumentNullException(nameof(_repo));
        }

        public IActionResult Index([FromQuery]string search = "")
        {
            return View(repo.GetLocations(search));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Location locale)
        {
            if (!ModelState.IsValid)
            {
                // there was a server-side validation error - return the user a new
                // form with what he put in.
                return View(locale);
            }

            locale.Id = 0;
            locale.Orders = new List<Order>();

            try
            {
                repo.AddLocation(locale);
                repo.Save();
            }
            catch (ArgumentException ex)
            {
                //ModelState.AddModelError("Title", "title error");

                // empty string for "key" means, it's overall model error
                ModelState.AddModelError("", ex.Message);
                return View(locale);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult OrderList(int id)
        {
            try
            {
                List<Order> orders = repo.GetLocationById(id).Orders;
                for (int i = 0; i < orders.Count; i++)
                {
                    orders[i] = repo.GetOrderById(orders[i].Id);
                    orders[i].Recipient = repo.GetCustomerById(orders[i].CustomerId);
                    orders[i].OrderLocation = repo.GetLocationById(orders[i].LocationId);
                }
                return View(orders);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Orders(int id)
        {
            try
            {
                Order order = repo.GetOrderById(id);

                return View(order);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
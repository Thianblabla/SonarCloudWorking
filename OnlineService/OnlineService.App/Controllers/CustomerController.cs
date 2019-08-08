using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineService.Library;

namespace OnlineService.App.Controllers
{
    public class CustomerController : Controller
    {
        private readonly Library.Interfaces.IServiceRepository repo;

        public CustomerController(Library.Interfaces.IServiceRepository _repo)
        {
            repo = _repo ?? throw new ArgumentNullException(nameof(_repo));
        }
        public IActionResult Index([FromQuery] string search = "")
        {
            // Get all customers containing a first or last name from the search
            IEnumerable<Customer> custom = repo.GetCustomersByFirstName(search)
                .Union(repo.GetCustomersByLastName(search), new Library.Comparers.CustomerComparer());
            // Due to how I am mapping the values, Union returns the entirety of the second list
            // To resolve this issue, I have made a class deriving from IEqualityComparer of type Customer

            // Display them all in the view
            return View(custom);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Customer custom)
        {
            if (!ModelState.IsValid)
            {
                // there was a server-side validation error - return the user a new
                // form with what he put in.
                return View(custom);
            }

            custom.Id = 0;
            custom.Security = SecurityLevel.NONE;
            custom.PastOrders = new List<Order>();

            try
            {
                repo.AddCustomer(custom);
                repo.Save();
            }
            catch (ArgumentException ex)
            {
                //ModelState.AddModelError("Title", "title error");

                // empty string for "key" means, it's overall model error
                ModelState.AddModelError("", ex.Message);
                return View(custom);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult OrderList(int id)
        {
            try
            {
                List<Order> orders = repo.GetCustomerById(id).PastOrders;
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
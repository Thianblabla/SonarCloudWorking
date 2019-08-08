using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineService.Library;

namespace OnlineService.App.Controllers
{
    public class ItemController : Controller
    {
        private readonly Library.Interfaces.IServiceRepository repo;

        public ItemController(Library.Interfaces.IServiceRepository _repo)
        {
            repo = _repo ?? throw new ArgumentNullException(nameof(_repo));
        }

        public IActionResult Index([FromQuery]string search = "")
        {
            List<Item> items = repo.GetItems(search).OrderBy(x => x.Name).ToList();
            return View(items);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Item item)
        {
            if (!ModelState.IsValid)
            {
                // there was a server-side validation error - return the user a new
                // form with what he put in.
                return View(item);
            }

            item.Id = 0;
            item.SecurityLevel = SecurityLevel.NONE;

            try
            {
                repo.AddItem(item);
                repo.Save();
            }
            catch (ArgumentException ex)
            {
                //ModelState.AddModelError("Title", "title error");

                // empty string for "key" means, it's overall model error
                ModelState.AddModelError("", ex.Message);
                return View(item);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
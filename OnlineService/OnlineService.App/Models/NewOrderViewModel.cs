using OnlineService.Library;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineService.App.Models
{
    public class NewOrderViewModel
    {
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }

        [Display(Name = "Location Name")]
        public string LocationName { get; set; }

        public List<Item> AvailableItems { get; set; }

        public List<Item> Purchased { get; set; }

        public Dictionary<int, uint> Counts { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineService.App.Models
{
    public class AddingViewModel
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        [Display(Name = "Item Count")]
        [Range(1, 10)]
        public uint ItemCount { get; set; }
    }
}

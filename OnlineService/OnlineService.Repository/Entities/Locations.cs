using System;
using System.Collections.Generic;

namespace OnlineService.Repository.Entities
{
    public partial class Locations
    {
        public Locations()
        {
            Orders = new HashSet<Orders>();
        }

        public int LocationId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Orders> Orders { get; set; }
    }
}

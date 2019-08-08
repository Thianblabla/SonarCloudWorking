using System;
using System.Collections.Generic;

namespace OnlineService.Repository.Entities
{
    public partial class Items
    {
        public Items()
        {
            OrderLine = new HashSet<OrderLine>();
        }

        public int ItemId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public long SecurityLevel { get; set; }

        public virtual ICollection<OrderLine> OrderLine { get; set; }
    }
}

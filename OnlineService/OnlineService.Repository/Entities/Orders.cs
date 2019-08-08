using System;
using System.Collections.Generic;

namespace OnlineService.Repository.Entities
{
    public partial class Orders
    {
        public Orders()
        {
            OrderLine = new HashSet<OrderLine>();
        }

        public int OrderId { get; set; }
        public int LocationId { get; set; }
        public int CustomerId { get; set; }
        public DateTime? PurchaseDate { get; set; }

        public virtual Customers Customer { get; set; }
        public virtual Locations Location { get; set; }
        public virtual ICollection<OrderLine> OrderLine { get; set; }
    }
}

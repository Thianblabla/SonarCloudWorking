using System;
using System.Collections.Generic;

namespace OnlineService.Repository.Entities
{
    public partial class OrderLine
    {
        public int OrderLineId { get; set; }
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public int ItemCount { get; set; }
        public decimal? Price { get; set; }

        public virtual Items Item { get; set; }
        public virtual Orders Order { get; set; }
    }
}

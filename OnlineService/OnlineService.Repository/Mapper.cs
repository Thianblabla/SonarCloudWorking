using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace OnlineService.Repository
{
    class Mapper
    {
        public static Library.Item Map(Entities.Items item) => new Library.Item
        {
            Id = item.ItemId,
            Name = item.Name,
            Price = item.Price,
            SecurityLevel = (Library.SecurityLevel)item.SecurityLevel
        };

        public static Entities.Items Map(Library.Item item) => new Entities.Items
        {
            ItemId = item.Id,
            Name = item.Name,
            Price = item.Price,
            SecurityLevel = (long)item.SecurityLevel
        };

        public static Library.Customer Map(Entities.Customers custom)
        {
            var customer = new Library.Customer
            {
                Id = custom.CustomerId,
                FirstName = custom.FirstName,
                LastName = custom.LastName,
                Security = (Library.SecurityLevel)custom.SecurityLevel
            };
            customer.PastOrders = Map(custom.Orders, customer).ToList();
            return customer;
        }

        public static Entities.Customers Map(Library.Customer custom)
        {
            var customer = new Entities.Customers
            {
                CustomerId = custom.Id,
                FirstName = custom.FirstName,
                LastName = custom.LastName,
                SecurityLevel = (long) custom.Security
            };
            customer.Orders = Map(custom.PastOrders, customer).ToList();
            return customer;
        }

        public static Library.Location Map(Entities.Locations locale) => new Library.Location
        {
            Id = locale.LocationId,
            Name = locale.Name,
            Orders = Map(locale.Orders).ToList()
        };

        public static Entities.Locations Map(Library.Location locale) => new Entities.Locations
        {
            LocationId = locale.Id,
            Name = locale.Name,
            Orders = Map(locale.Orders).ToList()
        };

        public static Library.Order Map(Entities.Orders order) => new Library.Order
        {
            Id = order.OrderId,
            CustomerId = order.CustomerId,
            LocationId = order.LocationId,
            Ordered = Map(order.OrderLine).ToList(),
            ItemCount = MapDic(order.OrderLine),
            PlacedTime = (DateTime)order.PurchaseDate
        };

        public static Library.Order Map(Entities.Orders order, Library.Customer custom) => new Library.Order
        {
            Id = order.OrderId,
            CustomerId = order.CustomerId,
            Recipient = custom,
            LocationId = order.LocationId,
            Ordered = Map(order.OrderLine).ToList(),
            ItemCount = MapDic(order.OrderLine),
            PlacedTime = (DateTime)order.PurchaseDate
        };

        public static Entities.Orders Map(Library.Order order, Entities.Customers custom)
        {
            var ord = new Entities.Orders
            {
                OrderId = order.Id,
                CustomerId = order.CustomerId,
                Customer = custom,
                LocationId = order.LocationId,
                PurchaseDate = order.PlacedTime,
                OrderLine = MapL(order)
            };
            return ord;
        }

        public static Entities.Orders Map(Library.Order order)
        {
           
            var ord = new Entities.Orders
            {
                CustomerId = order.CustomerId,
                OrderLine = MapL(order),
                LocationId = order.LocationId,
                PurchaseDate = order.PlacedTime,
          
            };
            return ord;
        }

        public static Library.Item Map(Entities.OrderLine order) => Map(order.Item);

        public static IEnumerable<Library.Order> Map(IEnumerable<Entities.Orders> orders) =>
            orders.Select(Map);

        public static ICollection<Entities.OrderLine> MapL(Library.Order order)
        {
            var returning = new List<Entities.OrderLine>();
            foreach (var item in order.Ordered)
            {
                var brandNew = new Entities.OrderLine
                {
                    ItemId = item.Id,
                    ItemCount = (int)order.ItemCount[item.Id],
                    Price = item.Price * order.ItemCount[item.Id]
                };
                returning.Add(brandNew);
            }
            return returning;
        }
        
        public static IEnumerable<Library.Item> Map(IEnumerable<Entities.OrderLine> orders) =>
           orders.Select(Map);

        public static IEnumerable<Entities.Orders> Map(IEnumerable<Library.Order> orders) =>
           orders.Select(Map);

        public static IEnumerable<Entities.Orders> Map(IEnumerable<Library.Order> orders, Entities.Customers custom)
        {
            List<Entities.Orders> order = new List<Entities.Orders>();
            foreach (var item in orders)
            {
                order.Add(Map(item, custom));
            }
            return order;
        }

        public static IEnumerable<Library.Order> Map(IEnumerable<Entities.Orders> orders, Library.Customer custom) => 
            orders.Select(Map);

        public static Dictionary<int, uint> MapDic(IEnumerable<Entities.OrderLine> orders)
        {
            var returning = new Dictionary<int, uint>();
            foreach (var item in orders)
            {
                returning.Add(item.ItemId, (uint)item.ItemCount);
            }
            return returning;
        }
    }
}

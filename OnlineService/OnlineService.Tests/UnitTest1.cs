using System;
using Xunit;
using OnlineService.Library;
using System.Collections.Generic;

namespace OnlineService.Testing
{
    public class UnitTest1
    {
        /// <summary>
        /// Create a default Location.  Test passes if location object is successfully made
        /// </summary>
        [Fact]
        public void DefaultLocation()
        {
            Location locale = new Location();
        }
        /// <summary>
        /// Create a default Inventory.  Test passes if inventory object is successfully made
        /// </summary>
        [Fact]
        public void DefaultInventory()
        {
            Inventory invent = new Inventory();
        }
        /// <summary>
        /// Create a default Customer.  Test passes if customer name is John
        /// </summary>
        [Fact]
        public void DefaultCustomer()
        {
            Customer loyal = new Customer();
            Assert.Equal("John", loyal.FirstName);
        }
        /// <summary>
        /// Create a somewhat default order.  Test passes if order object is successfully made
        /// </summary>
        [Fact]
        public void CreateOrder()
        {
            Location locale = new Location();
            Customer loyal = new Customer();

            Order up = new Order(locale, loyal);
        }
        /// <summary>
        /// Create a default item.  Test passes if id is -1
        /// </summary>
        [Fact]
        public void DefaultItem()
        {
            var SELLME = new Item();
            Assert.Equal(-1, SELLME.GetId());
        }

        // Parameter Constructor testing

        /// <summary>
        /// Create an item using an overloaded constructor.  Test passes if id is 1
        /// </summary>
        [Fact]
        public void CreateItem()
        {
            var SELLME = new Item(1, SecurityLevel.CHEMICAL | SecurityLevel.EXPLOSIVE);
            Assert.Equal(1, SELLME.GetId());
        }
        /// <summary>
        /// Create an item using an overloaded constructor.  Test passes if item name is gasoline
        /// </summary>
        [Fact]
        public void CreateItem2()
        {
            var SELLME = new Item(1, SecurityLevel.CHEMICAL | SecurityLevel.EXPLOSIVE, "gasoline");
            Assert.Equal("gasoline", SELLME.GetName());
        }
        /// <summary>
        /// Create a custom Customer object.  Test passes if customer name is Fred
        /// </summary>
        [Fact]
        public void CustomCustomer()
        {
            Customer loyal = new Customer("Fred", "Flintstone");
            Assert.Equal("Fred", loyal.FirstName);
        }
        /// <summary>
        /// Create a custom Customer object.  Test passes if customer security level is max
        /// </summary>
        [Fact]
        public void CustomCustomer2()
        {
            Customer loyal = new Customer("Fred", "Flintstone", new List<Order>(), SecurityLevel.ALL);
            Assert.Equal(SecurityLevel.ALL, loyal.Security);
        }
        /// <summary>
        /// Create a custom order object.  Test passes if order object is successfully made
        /// </summary>
        [Fact]
        public void CustomOrder()
        {
            Customer loyal = new Customer("Fred", "Flintstone");
            Location locale = new Location();
            Item banang = new Item();
            List<Item> list = new List<Item>();
            Dictionary<int, uint> dict = new Dictionary<int, uint>();
            list.Add(banang);
            dict.TryAdd(banang.GetId(), 7);

            Order up = new Order(locale, loyal, list, dict);
        }

        /// <summary>
        /// Create a custom order object.  Test passes if order object is successfully made
        /// </summary>
        [Fact]
        public void CustomOrder2()
        {
            Customer loyal = new Customer("Fred", "Flintstone");
            Location locale = new Location();
            Item banang = new Item();
            List<Item> list = new List<Item>();
            Dictionary<int, uint> dict = new Dictionary<int, uint>();
            list.Add(banang);
            dict.TryAdd(banang.GetId(), 7);

            Order up = new Order(locale, loyal, list, dict, DateTime.UnixEpoch);
        }

        /// <summary>
        /// Add an item and a count to an inventory.  Test passes if function successfully calls and returns.
        /// </summary>
        [Fact]
        public void AddToInventory()
        {
            Inventory invent = new Inventory();
            invent.AddItems(1, 99);
        }

        /// <summary>
        /// Use the location to place an order.  If it doesn't stop, it passes
        /// </summary>
        [Fact]
        public void LocationOrderCall()
        {
            Customer loyal = new Customer("Fred", "Flintstone");
            Inventory invent = new Inventory();
            invent.AddItems(1, 99);
            var temp = new List<Order>();
            Location locale = new Location(invent, temp);
            locale.AddToOrder(loyal, 1, 8);
        }
    }
}

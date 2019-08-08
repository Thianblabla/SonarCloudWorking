using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Linq;

namespace OnlineService.Library
{
    public class Order
    {
        private Customer _recipient;
        private Location _orderLocation;
        private List<Item> orderedItems;
        private Dictionary<int, uint> _itemCount;
        public int Id { get { return orderId; } set { orderId = value; } }
        public int CustomerId { get; set; }
        public int LocationId { get; set; }
        public Customer Recipient { get { return _recipient; } set { _recipient = value; } }
        public Location OrderLocation { get { return _orderLocation; } set { _orderLocation = value; } }
        [Display(Name = "Items")]
        public List<Item> Ordered { get { return orderedItems; } set { orderedItems = value; } }
        [Display(Name = "Count")]
        public Dictionary<int, uint> ItemCount { get { return _itemCount; } set { _itemCount = value; } }
        [Display(Name = "Purchase Date")]
        public DateTime PlacedTime { get; set; }
        private int orderId;

        public Order()
        {

        }
        public Order(Location _location, Customer recipient) : 
            this(_location, recipient, new List<Item>(), new Dictionary<int, uint>(), DateTime.MinValue)
        {
            
        }

        public Order(Location _location, Customer recipient, List<Item> item, Dictionary<int, uint> purchased, int _id, DateTime time) :
            this(_location, recipient, item, purchased, _id)
        {
            PlacedTime = time;
        }

        public Order(Location _location, Customer recipient, List<Item> item, Dictionary<int, uint> purchased, DateTime time) : 
            this(_location, recipient, item, purchased)
        {
            
        }

        public Order(Location _location, Customer recipient, List<Item> item, Dictionary<int, uint> purchased) :
            this(_location, recipient, item, purchased, BusinessInformation.OrderCount++)
        {
            
        }

        public Order(Location _location, Customer recipient, List<Item> item, Dictionary<int, uint> purchased, int _id)
        {
            _orderLocation = _location;
            _recipient = recipient;
            orderedItems = item;
            _itemCount = purchased;
            orderId = _id;
        }
        /// <summary>
        /// Add a specific item to the order
        /// If it already exists in it, it adds on to the item count instead
        /// </summary>
        /// <param name="item"></param>
        /// <param name="_count"></param>
        public void AddItemToOrder(Item item, uint _count)
        {
            if (orderedItems.FirstOrDefault(x => x.Id == item.Id) != null)
            {
                _itemCount[item.GetId()] += _count;
            }
            else
	        {
                orderedItems.Add(item);
                _itemCount.TryAdd(item.GetId(), _count);
            }
        }
        /// <summary>
        /// Remove an item from the order by a specific count
        /// If the count exceeds or is equal to the item count, the item is removed from the order
        /// If the item doesn't exist, throw a MissingMemberException
        /// </summary>
        /// <param name="item"></param>
        /// <param name="_count"></param>
        public void RemoveItemFromOrder(Item item, uint _count)
        {
            if (orderedItems.Contains(item))
            {
                if (_itemCount[item.GetId()] <= _count)
                {
                    _itemCount[item.GetId()] = 0;
                    _itemCount.Remove(item.GetId());
                    orderedItems.Remove(item);
                }
                else
                {
                    _itemCount[item.GetId()] -= _count;
                }
            }
            else
            {
                throw new MissingMemberException("The item is not part of the order!");
            }
        }
        /// <summary>
        /// Creates a dateTime object for the order, marking when the order was submitted
        /// </summary>
        public void PlaceOrder()
        {
            PlacedTime = DateTime.Now;
        }
        /// <summary>
        /// Returns the loctation the order takes place at
        /// </summary>
        /// <returns>Location object</returns>
        public Location GetLocation()
        {
            return _orderLocation;
        }
        /// <summary>
        /// Returns the customer who purchased the order
        /// </summary>
        /// <returns>Customer object</returns>
        public Customer GetCustomer()
        {
            return _recipient;
        }
        /// <summary>
        /// Returns a list of items that are being ordered
        /// </summary>
        /// <returns></returns>
        public List<Item> GetItems()
        {
            return orderedItems;
        }
        /// <summary>
        /// Returns the date and time an order was placed
        /// </summary>
        /// <returns>DateTime object from when the order was placed</returns>
        public DateTime GetOrderPlacedTime()
        {
            return PlacedTime;
        }
        /// <summary>
        /// Returns the dictionary containing the counts for each unique item
        /// </summary>
        /// <returns>Dictionary object made with item ids</returns>
        public Dictionary<int, uint> GetItemCounts()
        {
            return _itemCount;
        }
        /// <summary>
        /// Returns the order id for this specific object
        /// </summary>
        /// <returns>ulong object with order id</returns>
        public int GetOrderId()
        {
            return orderId;
        }

        public decimal TotalPriceOfItem(int id = 0)
        {
            if (id > 0 && Ordered.FirstOrDefault(x => x.Id == id) != null)
            {
                return Ordered.FirstOrDefault(x => x.Id == id).Price * ItemCount[id];
            }
            else
            {
                decimal returnValue = 0;
                foreach (var item in Ordered)
                {
                    returnValue += item.Price * ItemCount[item.Id];
                }
                return returnValue;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineService.Library
{
    public class Location
    {
        readonly Inventory inventory;
        List<Order> pastOrders;
        public int Id { get; set; }
        public List<Order> Orders { get { return pastOrders; } set { pastOrders = value; } }
        public string Name { get; set; }
        Order currentOrder = null;

        public Location() : this(new Inventory(), new List<Order>())
        {

        }

        public Location(Inventory _inventory, List<Order> orders)
        {
            inventory = _inventory;
            pastOrders = orders;
        }

        public void AddToOrder(Customer recipient, int id, uint count, Interfaces.IServiceRepository repo)
        {
            if (currentOrder != null)
            {
                try
                {
                    //inventory.OrderItems(id, count, recipient.Security);
                    currentOrder.AddItemToOrder(repo.GetItemById(id), count);
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (AccessViolationException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (KeyNotFoundException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                // check customer history for this location
                foreach (var item in recipient.PastOrders)
                {
                    if (pastOrders.Contains(item))
                    {
                        var totalHours = DateTime.Now.Subtract(item.GetOrderPlacedTime()).TotalHours;
                        if (totalHours < 2.0)
                        {
                            var additional = item.GetOrderPlacedTime().AddHours(2);
                            throw new ArgumentOutOfRangeException($"You cannot order from this location until {additional}");
                        }
                    }
                }
                CreateOrder(recipient);
                AddToOrder(recipient, id, count, repo);
            }
        }

        public void AddToOrder(Customer recipient, int id, uint count)
        {
            if (currentOrder != null)
            {
                try
                {
                    //inventory.OrderItems(id, count, recipient.Security);
                    currentOrder.AddItemToOrder(BusinessInformation.intItemList[id], count);
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (AccessViolationException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (KeyNotFoundException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                // check customer history for this location
                foreach (var item in recipient.PastOrders)
                {
                    if (pastOrders.Contains(item))
                    {
                        var totalHours = DateTime.Now.Subtract(item.GetOrderPlacedTime()).TotalHours;
                        if (totalHours < 2.0)
                        {
                            var additional = item.GetOrderPlacedTime().AddHours(2);
                            throw new ArgumentOutOfRangeException($"You cannot order from this location until {additional}");
                        }
                    }
                }
                CreateOrder(recipient);
                AddToOrder(recipient, id, count);
            }
        }

        private void CreateOrder(Customer recipient)
        {
            currentOrder = new Order(this, recipient);
            currentOrder.CustomerId = recipient.Id;
            currentOrder.LocationId = Id;
            currentOrder.Id = 0;
        }

        public void RemoveFromOrder(int id, uint count)
        {
            if (currentOrder != null)
            {
                try
                {
                    currentOrder.RemoveItemFromOrder(BusinessInformation.intItemList[id], count);
                    inventory.AddItems(id, count);
                    if (currentOrder.GetItems().Count == 0)
                    {
                        currentOrder = null;
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                    }
                }
                catch (MissingMemberException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                throw new MissingMemberException("There is no order to remove items from!");
            }
        }

        public void PlaceOrder(Library.Interfaces.IServiceRepository repo)
        {
            if (currentOrder != null)
            {
                currentOrder.PlaceOrder();
                currentOrder.Recipient.PastOrders.Add(currentOrder);
                pastOrders.Insert(0, currentOrder);
                repo.AddOrder(currentOrder);
                repo.UpdateLocation(this);
                repo.Save();

                currentOrder = null;
            }
            else
            {
                throw new MissingMemberException("An order cannot be placed if it doesn't exist!");
            }
        }

        public void RestartOrder()
        {
            currentOrder = null;
        }

        public Inventory GetInventory()
        {
            return inventory;
        }

        public List<Order> GetPastOrders()
        {
            return pastOrders;
        }
    }
}

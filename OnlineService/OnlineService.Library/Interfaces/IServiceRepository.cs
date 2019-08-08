using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineService.Library.Interfaces
{
    public interface IServiceRepository : IDisposable
    {
        /// <summary>
        /// Get all items with deferred execution.
        /// </summary>
        /// <returns>The collection of items</returns>
        IEnumerable<Item> GetItems(string search = null);

        /// <summary>
        /// Get all locations with deferred execution.
        /// </summary>
        /// <returns>The collection of locations</returns>
        IEnumerable<Location> GetLocations(string search = null);

        /// <summary>
        /// Get all customers with deferred execution.
        /// </summary>
        /// <returns>The collection of customers</returns>
        IEnumerable<Customer> GetCustomersByFirstName(string search = null);

        /// <summary>
        /// Get all locations with deferred execution.
        /// </summary>
        /// <returns>The collection of locations</returns>
        IEnumerable<Customer> GetCustomersByLastName(string search = null);

        /// <summary>
        /// Get an item by ID.
        /// </summary>
        /// <returns>The item</returns>
        Item GetItemById(int id);

        Order GetOrderById(int id);

        Location GetLocationById(int id);

        /// <summary>
        /// Add an item
        /// </summary>
        /// <param name="item">The item</param>
        void AddItem(Item item);

        /// <summary>
        /// Delete an item by ID. Any orders associated to it will remove the deleted item.
        /// </summary>
        /// <param name="itemId">The ID of the item</param>
        void DeleteItem(int itemId);

        /// <summary>
        /// Update an item as well as its reviews.
        /// </summary>
        /// <param name="item">The item with changed values</param>
        void UpdateItem(Item item);

        /// <summary>
        /// Add an order.
        /// </summary>
        /// <param name="order">The order</param>
        void AddOrder(Order order);

        /// <summary>
        /// Delete an order by ID.
        /// </summary>
        /// <param name="reviewId">The ID of the order</param>
        void DeleteOrder(int reviewId);

        /// <summary>
        /// Update an order.
        /// </summary>
        /// <param name="order">The order with changed values</param>
        void UpdateOrder(Order order);

        /// <summary>
        /// Add a location.
        /// </summary>
        /// <param name="location">The location</param>
        void AddLocation(Location location);

        /// <summary>
        /// Delete a location by ID.
        /// </summary>
        /// <param name="locationId">The ID of the location</param>
        void DeleteLocation(int locationId);

        /// <summary>
        /// Update a location.
        /// </summary>
        /// <param name="location">The location with changed values</param>
        void UpdateLocation(Location location);

        /// <summary>
        /// Get a customer by ID.
        /// </summary>
        /// <returns>The customer</returns>
        Customer GetCustomerById(int id);

        /// <summary>
        /// Add a customer.
        /// </summary>
        /// <param name="customer">The customer</param>
        void AddCustomer(Customer customer);

        /// <summary>
        /// Delete a customer by ID.
        /// </summary>
        /// <param name="customerId">The ID of the customer</param>
        void DeleteCustomer(int customerId);

        /// <summary>
        /// Update a customer.
        /// </summary>
        /// <param name="customer">The customer with changed values</param>
        void UpdateCustomer(Customer customer);

        /// <summary>
        /// Persist changes to the data source.
        /// </summary>
        void Save();
    }
}

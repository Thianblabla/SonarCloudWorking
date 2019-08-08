using System;
using System.Collections.Generic;
using System.Text;
using OnlineService.Library;
using OnlineService.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using NLog;
using System.Linq;

namespace OnlineService.Repository.Repository
{
    public class ServiceRepository : Library.Interfaces.IServiceRepository
    {
        private readonly OnlineServiceContext _dbContext;

        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public ServiceRepository(OnlineServiceContext dbContext) =>
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

        public void AddCustomer(Customer customer)
        {
            if (customer.Id != 0)
            {
                _logger.Warn($"Customer to be added has an ID ({customer.Id}) already: ignoring.");
            }

            _logger.Info($"Adding customer");

            Customers entity = Mapper.Map(customer);
            entity.CustomerId = 0;
            _dbContext.Add(entity);
        }

        public void AddItem(Item item)
        {
            if (item.Id != 0)
            {
                _logger.Warn($"Item to be added has an ID ({item.Id}) already: ignoring.");
            }

            _logger.Info($"Adding item");

            Items entity = Mapper.Map(item);
            entity.ItemId = 0;
            _dbContext.Add(entity);
        }

        public void AddLocation(Location location)
        {
            if (location.Id != 0)
            {
                _logger.Warn($"Restaurant to be added has an ID ({location.Id}) already: ignoring.");
            }

            _logger.Info($"Adding restaurant");

            Locations entity = Mapper.Map(location);
            entity.LocationId = 0;
            _dbContext.Add(entity);
        }

        public void AddOrder(Order order)
        {
            if (order.Id != 0)
            {
                _logger.Warn($"Restaurant to be added has an ID ({order.Id}) already: ignoring.");
            }

            _logger.Info($"Adding order");

            Orders entity = Mapper.Map(order);
            _dbContext.Orders.Add(entity);
        }

        public void DeleteCustomer(int customerId)
        {
            _logger.Info($"Deleting location with ID {customerId}");
            Customers entity = _dbContext.Customers.Find(customerId);
            _dbContext.Remove(entity);
        }

        public void DeleteItem(int itemId)
        {
            _logger.Info($"Deleting location with ID {itemId}");
            Items entity = _dbContext.Items.Find(itemId);
            _dbContext.Remove(entity);
        }

        public void DeleteLocation(int locationId)
        {
            _logger.Info($"Deleting location with ID {locationId}");
            Locations entity = _dbContext.Locations.Find(locationId);
            _dbContext.Remove(entity);
        }

        public void DeleteOrder(int orderId)
        {
            _logger.Info($"Deleting order with ID {orderId}");
            Orders entity = _dbContext.Orders.Find(orderId);
            _dbContext.Remove(entity);
        }

        public Customer GetCustomerById(int id) =>
            Mapper.Map(_dbContext.Customers.Include(x => x.Orders).FirstOrDefault(x => x.CustomerId == id));

        public Order GetOrderById(int id) =>
            Mapper.Map(_dbContext.Orders.Include(x => x.OrderLine).ThenInclude(y => y.Item).FirstOrDefault(x => x.OrderId == id));

        public Item GetItemById(int id) =>
            Mapper.Map(_dbContext.Items.Find(id));

        public Location GetLocationById(int id) =>
            Mapper.Map(_dbContext.Locations.Include(x => x.Orders)
                .ThenInclude(y => y.OrderLine).ThenInclude(z => z.Item)
                .FirstOrDefault(s=> s.LocationId == id));

        public IEnumerable<Customer> GetCustomersByFirstName(string search = null)
        {
            // disable unnecessary tracking for performance benefit
            IQueryable<Customers> custom = _dbContext.Customers.Include(x => x.Orders).AsNoTracking();
            if (search != null)
            {
                custom = custom.Where(r => r.FirstName.Contains(search));
            }
            return custom.Select(Mapper.Map);
        }

        public IEnumerable<Customer> GetCustomersByLastName(string search = null)
        {
            // disable unnecessary tracking for performance benefit
            IQueryable<Customers> custom = _dbContext.Customers.Include(x => x.Orders).AsNoTracking();
            if (search != null)
            {
                custom = custom.Where(r => r.LastName.Contains(search));
            }
            return custom.Select(Mapper.Map);
        }

        public IEnumerable<Item> GetItems(string search = null)
        {
            // disable unnecessary tracking for performance benefit
            IQueryable<Items> items = _dbContext.Items.AsNoTracking();
            if (search != null)
            {
                items = items.Where(r => r.Name.Contains(search));
            }
            return items.Select(Mapper.Map);
        }

        public IEnumerable<Location> GetLocations(string search = null)
        {
            IQueryable<Locations> items = _dbContext.Locations.Include(x => x.Orders).AsNoTracking();
            if (search != null)
            {
                items = items.Where(r => r.Name.Contains(search));
            }
            return items.Select(Mapper.Map);
        }

        public void Save()
        {
            _logger.Info("Saving changes to the database");
            _dbContext.SaveChanges();
        }

        public void UpdateCustomer(Customer customer)
        {
            _logger.Info($"Updating customer with Id{customer.Id}");

            // calling Update would mark every property as Modified.
            // this way will only mark the changed properties as Modified.
            Customers currentEntity = _dbContext.Customers.Find(customer.Id);
            Customers newEntity = Mapper.Map(customer);

            _dbContext.Entry(currentEntity).CurrentValues.SetValues(newEntity);
        }

        public void UpdateItem(Item item)
        {
            _logger.Info($"Updating item with Id{item.Id}");

            // calling Update would mark every property as Modified.
            // this way will only mark the changed properties as Modified.
            Items currentEntity = _dbContext.Items.Find(item.Id);
            Items newEntity = Mapper.Map(item);

            _dbContext.Entry(currentEntity).CurrentValues.SetValues(newEntity);
        }

        public void UpdateLocation(Location location)
        {
            _logger.Info($"Updating location with Id{location.Id}");

            // calling Update would mark every property as Modified.
            // this way will only mark the changed properties as Modified.
            Locations currentEntity = _dbContext.Locations.Find(location.Id);
            Locations newEntity = Mapper.Map(location);

            _dbContext.Entry(currentEntity).CurrentValues.SetValues(newEntity);
        }

        public void UpdateOrder(Order order)
        {
            _logger.Info($"Updating order with Id{order.Id}");

            // calling Update would mark every property as Modified.
            // this way will only mark the changed properties as Modified.
            Orders currentEntity = _dbContext.Orders.Find(order.Id);
            Orders newEntity = Mapper.Map(order);

            _dbContext.Entry(currentEntity).CurrentValues.SetValues(newEntity);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

    }

}

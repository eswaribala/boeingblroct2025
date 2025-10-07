using CustomerApp.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerApp.Services
{
    public class CustomerService : ICustomerService
    {
        IList<Customer> customers;
        private readonly IMemoryCache _memoryCache;
        public CustomerService(IMemoryCache memoryCache)
        {
            customers = new List<Customer>();
            _memoryCache = memoryCache;
        }

        public async Task AddCustomer(Customer customer)
        {
            customers.Add(customer);
            await Task.Delay(10000);
        }

        public async Task<bool> DeleteCustomer(long id)
        {
            bool isDeleted = false;
            var customer = customers.FirstOrDefault(c => c.CustomerId == id);
            if (customer != null)
            {
                customers.Remove(customer);
                await Task.Delay(1000);
                isDeleted = true;
            }
            return isDeleted;

        }

        public async Task<IEnumerable<Customer>> GetAllCustomers()
        {
            await Task.Delay(1000);
            return customers;
        }

        public async ValueTask<Customer> GetCustomerById(long id)
        {
            if (_memoryCache.TryGetValue(id, out Customer cachedCustomer))
            {
                return cachedCustomer;
            }

            await Task.Delay(1000);
            var customer= customers.FirstOrDefault(c => c.CustomerId == id);
            _memoryCache.Set(id, customer, TimeSpan.FromMinutes(5));
            return customer;
        }

        public async Task<bool> UpdateCustomer(Customer newCustomer)
        {
            bool isUpdated = false;
            var customer = customers.FirstOrDefault(c => c.CustomerId == newCustomer.CustomerId);
            if (customer != null)
            {
                customer.Name = newCustomer.Name;
                customer.Address = newCustomer.Address;
                customer.Email = newCustomer.Email;
                customer.PhoneNumber = newCustomer.PhoneNumber;
                customer.Password = newCustomer.Password;
                isUpdated = true;
            }
            await Task.Delay(1000);
            return isUpdated;
        }
    }
}

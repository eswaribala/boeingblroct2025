using CustomerApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerApp.Services
{
    public class CorporateService : ICustomerService
    {
        IList<Corporate> _corporates;
        public CorporateService()
        {
            _corporates = new List<Corporate>();
        }

        public async Task AddCustomer(Customer customer)
        {
            await Task.Delay(1000);
            _corporates.Add((Corporate)customer);
        }

        public async Task<bool> DeleteCustomer(long id)
        {
            var isDeleted = false;
            var corporate = _corporates.FirstOrDefault(c => c.CustomerId == id);
            if (corporate != null) { 
            
                _corporates.Remove(corporate);
                isDeleted = true;
            }
            await Task.Delay(1000);
            return isDeleted;

        }

        public async Task<IEnumerable<Customer>> GetAllCustomers()
        {
            await Task.Delay(1000);
            return _corporates;
        }

        public async Task<Customer> GetCustomerById(long id)
        {
            var corporate = _corporates.FirstOrDefault(c => c.CustomerId == id);
            await Task.Delay(1000);
            return corporate;
        }

        public async Task<bool> UpdateCustomer(Customer newCustomer)
        {
            var corporate = _corporates.FirstOrDefault(c => c.CustomerId == newCustomer.CustomerId);
            var isUpdated = false;
            var customer = newCustomer;
            if (corporate != null) {
                corporate.Name = customer.Name;
                corporate.Address = customer.Address;
                corporate.Email = customer.Email;
                corporate.PhoneNumber = customer.PhoneNumber;
                corporate.Password = customer.Password;
                
                isUpdated = true;
            }
            await Task.Delay(1000);
            return isUpdated;
        }
    }
}

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

        public void AddCustomer(Customer customer)
        {
           _corporates.Add((Corporate)customer);
        }

        public bool DeleteCustomer(long id)
        {
            var isDeleted = false;
            var corporate = _corporates.FirstOrDefault(c => c.CustomerId == id);
            if (corporate != null) { 
            
                _corporates.Remove(corporate);
                isDeleted = true;
            }
            return isDeleted;

        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            return _corporates;
        }

        public Customer GetCustomerById(long id)
        {
            var corporate = _corporates.FirstOrDefault(c => c.CustomerId == id);
            return corporate;
        }

        public bool UpdateCustomer(Customer newCustomer)
        {
            var corporate = _corporates.FirstOrDefault(c => c.CustomerId == id);
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

            return isUpdated;
        }
    }
}

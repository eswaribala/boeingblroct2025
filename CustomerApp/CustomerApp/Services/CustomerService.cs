using CustomerApp.Models;
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
        public CustomerService()
        {
            customers = new List<Customer>();
        }

        public void AddCustomer(Customer customer)
        {
            customers.Add(customer);
        }

        public bool DeleteCustomer(long id)
        {
            bool isDeleted = false;
            var customer = customers.FirstOrDefault(c => c.CustomerId == id);
            if (customer != null)
            {
                customers.Remove(customer);
                isDeleted = true;
            }
            return isDeleted;

        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            return customers;
        }

        public Customer GetCustomerById(long id)
        {
          return customers.FirstOrDefault(c => c.CustomerId == id);
        }

        public bool UpdateCustomer(Customer newCustomer)
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
            return isUpdated;
        }
    }
}

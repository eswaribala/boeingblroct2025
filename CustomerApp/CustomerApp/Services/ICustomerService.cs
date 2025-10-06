using CustomerApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerApp.Services
{
    public interface ICustomerService
    {
        void AddCustomer(Customer customer);
        Customer GetCustomerById(long id);
        IEnumerable<Customer> GetAllCustomers();
        void UpdateCustomer(Customer newCustomer);
        void DeleteCustomer(long id);
    }
}

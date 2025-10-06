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
        Task AddCustomer(Customer customer);
        Task<Customer> GetCustomerById(long id);
        Task<IEnumerable<Customer>> GetAllCustomers();
        Task<bool> UpdateCustomer(Customer newCustomer);
        Task<bool> DeleteCustomer(long id);
    }
}

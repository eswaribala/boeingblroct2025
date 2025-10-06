using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomerApp.Services;
using CustomerApp.Models;

namespace CustomerApp.Controllers
{
    public  class CustomerController
    {

        private readonly ICustomerService _customerService;
        //DI
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        public void AddCustomer(Customer customer)
        {
            _customerService.AddCustomer(customer);
        }
        public Models.Customer GetCustomerById(long id)
        {
            return _customerService.GetCustomerById(id);
        }
        public IEnumerable<Customer> GetAllCustomers()
        {
            return _customerService.GetAllCustomers();
        }
        public bool UpdateCustomer(Models.Customer newCustomer)
        {
            return _customerService.UpdateCustomer(newCustomer);
        }
        public bool DeleteCustomer(long id)
        {
            return _customerService.DeleteCustomer(id);
        }

    }
}

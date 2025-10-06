using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomerApp.Services;
using CustomerApp.Models;
using CustomerApp.DTOS;
using Microsoft.Extensions.Options;

namespace CustomerApp.Controllers
{
    public  class CustomerController
    {

        private readonly ICustomerService _customerService;
        private readonly KafkaServer _kafkaServer;
        //DI
        public CustomerController(ICustomerService customerService,IOptions<KafkaServer> kafkaServer)
        {
            _customerService = customerService;
            _kafkaServer = kafkaServer.Value;
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
            Console.WriteLine("Kafka Server Details:");
            Console.WriteLine($"BootstrapServers: {_kafkaServer.Host}");
            return _customerService.GetAllCustomers();
        }
        public bool UpdateCustomer(Customer newCustomer)
        {
            return _customerService.UpdateCustomer(newCustomer);
        }
        public bool DeleteCustomer(long id)
        {
            return _customerService.DeleteCustomer(id);
        }

    }
}

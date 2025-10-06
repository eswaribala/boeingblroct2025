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
        public async Task AddCustomer(Customer customer)
        {
            await _customerService.AddCustomer(customer);
        }
        public async Task<Customer> GetCustomerById(long id)
        {
            return await _customerService.GetCustomerById(id);
        }
        public async Task<IEnumerable<Customer>> GetAllCustomers()
        {
            Console.WriteLine("Kafka Server Details:");
            Console.WriteLine($"BootstrapServers: {_kafkaServer.Host}");
            return await _customerService.GetAllCustomers();
        }
        public async Task<bool> UpdateCustomer(Customer newCustomer)
        {
            return await _customerService.UpdateCustomer(newCustomer);
        }
        public async Task<bool> DeleteCustomer(long id) 
        {
            return await _customerService.DeleteCustomer(id);
        }

    }
}

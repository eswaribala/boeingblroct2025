using CustomerApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerApp.Services
{
    public class IndividualService : ICustomerService
    {
        IList<Individual> _individuals;
        public IndividualService()
        {
            _individuals = new List<Individual>();
        }
        public async Task AddCustomer(Customer customer)
        {
           _individuals.Add((Individual)customer);
              await Task.Delay(1000);
        }

        public async Task<bool> DeleteCustomer(long id)
        {
            bool isDeleted = false;
            var individual = _individuals.FirstOrDefault(i => i.CustomerId == id);
            if (individual != null)
            {
                _individuals.Remove(individual);
                isDeleted = true;
            }
            await Task.Delay(1000);
            return isDeleted;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomers()
        {
            await Task.Delay(1000);
            return _individuals;
        }

        public async Task<Customer> GetCustomerById(long id)
        {
            await Task.Delay(1000);
            return _individuals.FirstOrDefault(i => i.CustomerId == id);
        }

        public async Task<bool> UpdateCustomer(Customer newCustomer)
        {
            var individual = _individuals.FirstOrDefault(i => i.CustomerId == newCustomer.CustomerId);
            bool isUpdated = false;
            if (individual != null)
            {
                individual.Name = newCustomer.Name;
                individual.Address = newCustomer.Address;
                individual.Email = newCustomer.Email;
                individual.PhoneNumber = newCustomer.PhoneNumber;
                individual.Password = newCustomer.Password;
                individual.DateOfBirth = ((Individual)newCustomer).DateOfBirth;
                isUpdated = true;
            }
            await Task.Delay(1000);
            return isUpdated;

        }
    }
}

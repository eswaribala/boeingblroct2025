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
        public void AddCustomer(Customer customer)
        {
           _individuals.Add((Individual)customer);
        }

        public bool DeleteCustomer(long id)
        {
            bool isDeleted = false;
            var individual = _individuals.FirstOrDefault(i => i.CustomerId == id);
            if (individual != null)
            {
                _individuals.Remove(individual);
                isDeleted = true;
            }
            return isDeleted;
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            return _individuals;
        }

        public Customer GetCustomerById(long id)
        {
            return _individuals.FirstOrDefault(i => i.CustomerId == id);
        }

        public bool UpdateCustomer(Customer newCustomer)
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
            return isUpdated;

        }
    }
}

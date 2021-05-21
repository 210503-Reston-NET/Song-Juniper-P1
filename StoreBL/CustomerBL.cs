using System;
using System.Collections.Generic;
using StoreDL;
using StoreModels;

namespace StoreBL
{
    public class CustomerBL
    {
        private CustomerRepoDB _repo;
        public CustomerBL(CustomerRepoDB repo)
        {
            _repo = repo;
        }

        public Customer AddNewCustomer(Customer customer)
        {
            if(FindCustomerByName(customer.Name) is not null)
            {
                throw new Exception("This user already exists in the system.");
            }
            return _repo.AddNewCustomer(customer);
        }

        public List<Customer> GetAllCustomers()
        {
            return _repo.GetAllCustomers();
        }

        public Customer FindCustomerByName(string name)
        {
            return _repo.GetCustomerByName(name);
        }

        public Customer FindCustomerById(int id)
        {
            return _repo.GetCustomerById(id);
        }
    }
}

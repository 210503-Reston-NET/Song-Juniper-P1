using System;
using System.Collections.Generic;
using StoreDL;
using StoreModels;

namespace StoreBL
{
    public class CustomerBL : ICustomerBL
    {
        private ICustomerRepo _repo;
        public CustomerBL(ICustomerRepo repo)
        {
            _repo = repo;
        }
        /// <summary>
        /// calls the repo for adding a new customer. Checks whether the user is already in the system before adding
        /// </summary>
        /// <param name="customer"></param>
        /// <returns>customer object that has been created</returns>
        public Customer AddNewCustomer(Customer customer)
        {
            if(FindCustomerByName(customer.Name) is not null)
            {
                throw new Exception("This user already exists in the system.");
            }
            return _repo.AddNewCustomer(customer);
        }
        /// <summary>
        /// calls the repo method for getting all customers
        /// </summary>
        /// <returns>List of customer objects, null if empty</returns>
        public List<Customer> GetAllCustomers()
        {
            return _repo.GetAllCustomers();
        }
        /// <summary>
        /// calls repo method for finding a customer by name
        /// </summary>
        /// <param name="name">string name</param>
        /// <returns>customer object, if found, null otherwise</returns>
        public Customer FindCustomerByName(string name)
        {
            return _repo.GetCustomerByName(name);
        }
        /// <summary>
        /// calls repo method for finding a customer by id
        /// </summary>
        /// <param name="id">id of the customer in the db</param>
        /// <returns>if found, customer object, null if not</returns>
        public Customer FindCustomerById(int id)
        {
            return _repo.GetCustomerById(id);
        }

        /// <summary>
        /// Updates the customer info
        /// </summary>
        /// <param name="customer">customer object</param>
        /// <returns>updated customer</returns>
        public Customer UpdateCustomer(Customer customer)
        {
            return _repo.UpdateCustomer(customer);
        }

        /// <summary>
        /// Deletes customer from db
        /// </summary>
        /// <param name="customer">customer obj to be deleted</param>
        public void DeleteCustomer(Customer customer)
        {
            _repo.DeleteCustomer(customer);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StoreModels;

namespace StoreDL
{
    public class CustomerRepoDB : ICustomerRepo
    {
        private WssDBContext _context;
        public CustomerRepoDB(WssDBContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Gets all customers
        /// </summary>
        /// <returns>list of all customers</returns>
        public List<Customer> GetAllCustomers()
        {
            return _context.Customers
            .AsNoTracking()
            .Select(
                customer => customer
            ).ToList();
        }
        /// <summary>
        /// Finds customer by id
        /// </summary>
        /// <param name="id">int id, to be used to find</param>
        /// <returns>Found customer obj</returns>
        public Customer GetCustomerById(int id)
        {
            Customer found = _context.Customers
            .AsNoTracking()
            .FirstOrDefault(customer => customer.Id.Equals(id));
            return found;
        }
        /// <summary>
        /// Finds customer obj by name
        /// </summary>
        /// <param name="name">string name, to be used</param>
        /// <returns>Customer obj, if found</returns>
        public Customer GetCustomerByName(string name)
        {
            Customer found = _context.Customers
            .AsNoTracking()
            .FirstOrDefault(customer => customer.Name == name);
            return found;
        }
        /// <summary>
        /// Adds a new customer obj to db
        /// </summary>
        /// <param name="customer">customer obj to be added</param>
        /// <returns>created customer obj w/ id</returns>
        public Customer AddNewCustomer(Customer customer)
        {
            Customer newCust = _context.Customers.Add(customer).Entity;
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            return newCust;
        }
        /// <summary>
        /// Updates customer obj in db
        /// </summary>
        /// <param name="customer">customer obj to be updated</param>
        /// <returns>updated customer obj</returns>
        public Customer UpdateCustomer(Customer customer)
        {
            Customer updated = _context.Customers.Update(customer).Entity;
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            return updated;
        }

        /// <summary>
        /// Deletes the customer from DB
        /// </summary>
        /// <param name="customer">customer obj to be deleted</param>
        public void DeleteCustomer(Customer customer)
        {
            _context.Customers.Remove(customer);
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
        }
    }
}
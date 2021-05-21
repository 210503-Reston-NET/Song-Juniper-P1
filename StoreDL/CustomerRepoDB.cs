using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StoreModels;

namespace StoreDL
{
    public class CustomerRepoDB
    {
        private WssDBContext _context;
        public CustomerRepoDB(WssDBContext context)
        {
            _context = context;
        }

        public List<Customer> GetAllCustomers()
        {
            return _context.Customers
            .AsNoTracking()
            .Select(
                customer => customer
            ).ToList();
        }

        public Customer GetCustomerById(int id)
        {
            Customer found = _context.Customers
            .AsNoTracking()
            .FirstOrDefault(customer => customer.Id == id);
            return found;
        }

        public Customer GetCustomerByName(string name)
        {
            Customer found = _context.Customers
            .AsNoTracking()
            .FirstOrDefault(customer => customer.Name == name);
            return found;
        }

        public Customer AddNewCustomer(Customer customer)
        {
            Customer newCust = _context.Customers.Add(customer).Entity;
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            return newCust;
        }
    }
}
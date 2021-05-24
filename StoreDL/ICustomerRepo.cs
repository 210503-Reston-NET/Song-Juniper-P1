using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreModels;

namespace StoreDL
{
    public interface ICustomerRepo
    {
        public List<Customer> GetAllCustomers();
        public Customer GetCustomerById(int id);
        public Customer GetCustomerByName(string name);
        public Customer AddNewCustomer(Customer customer);
        public Customer UpdateCustomer(Customer customer);
        public void DeleteCustomer(Customer customer);
    }
}

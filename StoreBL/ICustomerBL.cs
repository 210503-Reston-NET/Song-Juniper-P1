using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreModels;

namespace StoreBL
{
    public interface ICustomerBL
    {
        public Customer AddNewCustomer(Customer customer);
        public List<Customer> GetAllCustomers();
        public Customer FindCustomerByName(string name);
        public Customer FindCustomerById(int id);
        public Customer UpdateCustomer(Customer customer);

        public void DeleteCustomer(Customer customer);
    }
}

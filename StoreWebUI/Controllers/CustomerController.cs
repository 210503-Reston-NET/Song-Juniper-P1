using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreBL;
using StoreModels;
using StoreWebUI.Models;

namespace StoreWebUI.Controllers
{
    public class CustomerController : Controller
    {
        private ICustomerBL _customerBL;
        // GET: CustomerController

        public CustomerController(ICustomerBL customerBL)
        {
            _customerBL = customerBL;
        }
        public ActionResult Index()
        {
            return View(
                _customerBL.GetAllCustomers()
                .Select(customer => new CustomerVM(customer))
                .ToList()
                );
        }

        // GET: CustomerController/Details/5
        // To be implemented later -- Profile view?
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        // GET: CustomerController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomerVM customerVM)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    _customerBL.AddNewCustomer(new Customer
                    {
                        Name = customerVM.Name,
                        Email = customerVM.Email
                    });
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CustomerController/Edit/5
        public ActionResult Edit(int id)
        {
            return View(new CustomerVM(_customerBL.FindCustomerById(id)));
        }

        // POST: CustomerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, CustomerVM customerVM)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    _customerBL.UpdateCustomer(new Customer
                    {
                        Id = customerVM.Id,
                        Name = customerVM.Name,
                        Email = customerVM.Email
                    });
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CustomerController/Delete/5
        public ActionResult Delete(int id)
        {
            return View(new CustomerVM(_customerBL.FindCustomerById(id)));
        }

        // POST: CustomerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, CustomerVM customerVM)
        {
            try
            {
                _customerBL.DeleteCustomer(new Customer
                {
                    Id = customerVM.Id
                });
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}

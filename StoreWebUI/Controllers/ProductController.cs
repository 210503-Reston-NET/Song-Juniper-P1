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
    public class ProductController : Controller
    {
        private IProductBL _productBL;

        public ProductController(IProductBL productBL)
        {
            _productBL = productBL;
        }
        // GET: ProductController
        public ActionResult Index()
        {
            return View(
                _productBL.GetAllProducts()
                .Select(product => new ProductVM(product))
                .ToList()
                );
        }

        public ActionResult Create()
        {
            return View(new ProductVM());
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductVM productVM)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    _productBL.AddNewProduct(new Product
                    {
                        Name = productVM.Name,
                        Description = productVM.Description,
                        Price = productVM.Price,
                        Category = productVM.Category
                    });
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            return View(new ProductVM(_productBL.FindProductById(id)));
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ProductVM productVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _productBL.UpdateProduct(new Product
                    {
                        Id = productVM.Id,
                        Name = productVM.Name,
                        Description = productVM.Description,
                        Price = productVM.Price,
                        Category = productVM.Category
                    });
                }
                 return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            return View(new ProductVM(_productBL.FindProductById(id)));
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, ProductVM productVM)
        {
            try
            {
                _productBL.DeleteProduct(new Product(productVM.Id));
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(new ProductVM(_productBL.FindProductById(id)));
            }
        }
    }
}

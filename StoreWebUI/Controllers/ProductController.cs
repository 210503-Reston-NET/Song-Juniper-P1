using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StoreBL;
using StoreModels;
using StoreWebUI.Models;

namespace StoreWebUI.Controllers
{
    public class ProductController : Controller
    {
        private IProductBL _productBL;
        private ILogger<ProductController> _logger;

        public ProductController(IProductBL productBL, ILogger<ProductController> logger)
        {
            _productBL = productBL;
            _logger = logger;
        }

        // GET: ProductController
        public ActionResult Index()
        {
            _logger.LogInformation("GET: ProductController/Index");
            return View(
                _productBL.GetAllProducts()
                .Select(product => new ProductVM(product))
                .ToList()
                );
        }

        /// <summary>
        /// GET: ProductController/Create
        /// </summary>
        /// <returns>View to create a new product</returns>
        public ActionResult Create()
        {
            _logger.LogInformation("GET: CREATE new product page");
            return View(new ProductVM());
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductVM productVM)
        {
            _logger.LogInformation("POST: CREATE new product", productVM);
            try
            {
                if (ModelState.IsValid)
                {
                    _productBL.AddNewProduct(new Product
                    {
                        Name = productVM.Name,
                        Description = productVM.Description,
                        Price = productVM.Price,
                        Category = productVM.Category
                    });
                    _logger.LogInformation("Product Creation successful", productVM);
                    return RedirectToAction(nameof(Index));
                }
                _logger.LogInformation("Product Creation: Model State is invalid");
                return View();    
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Product Creation Failed", productVM);
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
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
    public class LocationController : Controller
    {
        private ILocationBL _locationBL;
        public LocationController(ILocationBL locationBL)
        {
            _locationBL = locationBL;
        }
        // GET: LocationController
        public ActionResult Index()
        {
            return View(
                _locationBL.GetAllLocations()
                .Select(location => new LocationVM(location))
                .ToList()
                );
        }

        // GET: LocationController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LocationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LocationVM locationVM)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    _locationBL.AddNewLocation(new Location
                    {
                        Name = locationVM.Name,
                        Address = locationVM.Address,
                    });
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LocationController/Edit/5
        public ActionResult Edit(int id)
        {
            return View(new LocationVM(_locationBL.FindLocationByID(id)));
        }

        // POST: LocationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, LocationVM locationVM)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    _locationBL.UpdateLocation(new Location
                    {
                        Id = locationVM.Id,
                        Name = locationVM.Name,
                        Address = locationVM.Address
                    }); ;
                    return RedirectToAction(nameof(Index));
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: LocationController/Delete/5
        public ActionResult Delete(int id)
        {
            return View(new LocationVM(_locationBL.FindLocationByID(id)));
        }

        // POST: LocationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, LocationVM locationVM)
        {
            try
            {
                _locationBL.DeleteLocation(new Location
                {
                    Id = locationVM.Id,
                    Name = locationVM.Name,
                    Address = locationVM.Address
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

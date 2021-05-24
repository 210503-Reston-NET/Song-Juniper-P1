﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using StoreModels;

namespace StoreWebUI.Models
{
    public class ProductVM
    {
        public ProductVM(Product product)
        {
            Id = product.Id;
            Name = product.Name;
            Description = product.Description;
            Price = product.Price;
            Category = product.Category;
        }

        public ProductVM() { }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string Category { get; set; }

        public IEnumerable<SelectListItem> CategoryOptions = new List<SelectListItem>()
        {
            new SelectListItem
            {
                Value = "Tools",
                Text = "Tools"
            },
            new SelectListItem
            {
                Value = "Supplies",
                Text = "Supplies"
            }
        };
    }
}
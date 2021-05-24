﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using StoreModels;

namespace StoreWebUI.Models
{
    public class InventoryVM
    {
        public InventoryVM() { }
        public int Id { get; set; }
        public Product Product { get; set; }
        [DisplayName("Product")]
        public int ProductId { get; set; }
        [Required]
        public int LocationId { get; set; }
        [Required]
        public int Quantity { get; set; }
        public List<SelectListItem> ProductOptions { get; set; }
    }
}

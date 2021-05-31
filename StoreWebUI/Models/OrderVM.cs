using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StoreModels;

namespace StoreWebUI.Models
{
    public class OrderVM
    {
        public OrderVM() { }

        public OrderVM(Order order)
        {

        }

        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid UserId { get; set; }
        public int LocationId { get; set; }
        public List<LineItem> LineItems { get; set; }
    }
}

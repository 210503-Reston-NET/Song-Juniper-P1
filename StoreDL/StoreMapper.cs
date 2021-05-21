//using System.Collections.Generic;
//using Entity = StoreDL.Entities;
//using Model = StoreModels;

//namespace StoreDL
//{
//    public class StoreMapper : IMapper
//    {
//        public StoreMapper() {}
//        public Model.Customer ParseCustomer(Entity.Customer customer)
//        {
//            if(customer is null) return null;
//            List<Model.Order> orders = new List<Model.Order>();
//            if (customer.Orders is not null && customer.Orders.Count > 0)
//            {
//                foreach(Entity.Order order in customer.Orders)
//                {
//                    orders.Add(ParseOrder(order));
//                }
//            }
//            return new Model.Customer
//            {
//                Id = customer.Id,
//                Name = customer.CName,
//                Orders = orders
//            };
//        }

//        public Entity.Customer ParseCustomer(Model.Customer customer,bool create)
//        {
//            if(customer is null) return null;
//            if(create)
//            {
//                return new Entity.Customer
//                {
//                    CName = customer.Name,
//                };
//            }
//            else
//            {
//                return new Entity.Customer
//                {
//                    Id = customer.Id,
//                    CName = customer.Name,
//                };
//            }
//        }

//        public Model.Inventory ParseInventory(Entity.Inventory inventory)
//        {
//            if(inventory is null) return null;
//            return new Model.Inventory
//            {
//                Id = inventory.Id,
//                Product = ParseProduct(inventory.Product),
//                LocationId = inventory.StoreId,
//                Quantity = inventory.Quantity
//            };
//        }

//        public Entity.Inventory ParseInventory(Model.Inventory inventory, bool create)
//        {
//            if(inventory is null) return null;
//            if(create)
//            {
//                return new Entity.Inventory
//                {
//                    StoreId = inventory.LocationId,
//                    ProductId = inventory.Product.Id,
//                    Quantity = inventory.Quantity
//                };
//            }
//            else
//            {
//                return new Entity.Inventory
//                {
//                    Id = inventory.Id,
//                    StoreId = inventory.LocationId,
//                    ProductId = inventory.Product.Id,
//                    Quantity = inventory.Quantity
//                };
//            }
//        }

//        public Model.LineItem ParseLineItem(Entity.LineItem item)
//        {
//            if(item is null) return null;
//            return new Model.LineItem
//            {
//                Id = item.Id,
//                Product = ParseProduct(item.Product),
//                OrderId = item.OrderId,
//                Quantity = item.Quantity
//            };
//        }

//        public Entity.LineItem ParseLineItem(Model.LineItem item, bool create)
//        {
//            if(item is null) return null;
//            if(create)
//            {
//                return new Entity.LineItem
//                {
//                    ProductId = item.Product.Id,
//                    OrderId = item.OrderId,
//                    Quantity = item.Quantity
//                };
//            }
//            else
//            {
//                return new Entity.LineItem
//                {
//                    Id = item.Id,
//                    ProductId = item.Product.Id,
//                    OrderId = item.OrderId,
//                    Quantity = item.Quantity
//                };
//            }
//        }

//        public Model.Order ParseOrder(Entity.Order order)
//        {
//            if(order is null) return null;
//            List<Model.LineItem> lineitems = new List<Model.LineItem>();
//            if (order.LineItems is not null)
//            {
//                foreach(Entity.LineItem item in order.LineItems)
//                {
//                    lineitems.Add(ParseLineItem(item));
//                }
//            }
            
//            return new Model.Order
//            {
//                Id = order.Id,
//                DateCreated = order.DateCreated,
//                CustomerId = order.CustomerId,
//                LocationId = order.StoreId,
//                LineItems = lineitems,
//                Total = order.Total,
//                Closed = order.Placed
//            };
//        }

//        public Entity.Order ParseOrder(Model.Order order, bool create)
//        {
//            if(order is null) return null;
//            if(create)
//            {
//                return new Entity.Order
//                {
//                    DateCreated = order.DateCreated,
//                    CustomerId = order.CustomerId,
//                    StoreId = order.LocationId,
//                    Total = order.Total,
//                    Placed = order.Closed
//                };
//            }
//            else
//            {
//                return new Entity.Order
//                {
//                    Id = order.Id,
//                    DateCreated = order.DateCreated,
//                    CustomerId = order.CustomerId,
//                    StoreId = order.LocationId,
//                    Total = order.Total,
//                    Placed = order.Closed
//                };
//            }
//        }

//        public Model.Product ParseProduct(Entity.Product product)
//        {
//            if(product is null) return null;
//            return new Model.Product
//            {
//                Id = product.Id,
//                Name = product.PName,
//                Description = product.PDesc,
//                Price = product.Price,
//                Category = product.Category
//            };
//        }

//        public Entity.Product ParseProduct(Model.Product product, bool create)
//        {
//            if(product is null) return null;
//            if(create)
//            {
//                return new Entity.Product
//                {
//                    PName = product.Name,
//                    PDesc = product.Description,
//                    Price = product.Price,
//                    Category = product.Category
//                };
//            }
//            else
//            {
//                return new Entity.Product
//                {
//                    Id = product.Id,
//                    PName = product.Name,
//                    PDesc = product.Description,
//                    Price = product.Price,
//                    Category = product.Category
//                };
//            }
//        }

//        public Model.Location ParseStore(Entity.StoreFront store)
//        {
//            if(store is null) return null;
//            List<Model.Inventory> inventory = new List<Model.Inventory>();
//            List<Model.Order> orders = new List<Model.Order>();
//            if(store.Inventories is not null)
//            {
//                foreach(Entity.Inventory inven in store.Inventories)
//                {
//                    inventory.Add(ParseInventory(inven));
//                }
//            }
//            if(store.Orders is not null)
//            {
//                foreach(Entity.Order order in store.Orders)
//                {
//                    orders.Add(ParseOrder(order));
//                }
//            }
//            return new Model.Location
//            {
//                Id = store.Id,
//                Name = store.SName,
//                Address = store.SAddress,
//                Inventory = inventory,
//                Orders = orders
//            };
//        }

//        public Entity.StoreFront ParseStore(Model.Location store, bool create)
//        {
//            if(store is null) return null;
//            if(create)
//            {
//                return new Entity.StoreFront
//                {
//                    SName = store.Name,
//                    SAddress = store.Address
//                };
//            }
//            else
//            {
//                return new Entity.StoreFront
//                {
//                    Id = store.Id,
//                    SName = store.Name,
//                    SAddress = store.Address
//                };
//            }
//        }
//    }
//}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace EfEx1
{
    class Program
    {
        static void Main(string[] args)
        {

            var result = GetOrderDetailsByProductId(59);
            foreach (var res in result)
            {
                Console.WriteLine("result: " + res);
            }
            //var p = GetSingleOrderById(10510);
            //Console.WriteLine("result: " + p);
        }
        // 1. Get a single order by ID
        private static Orders GetSingleOrderById(int id)
        {
            using (var db = new NorthwindContex())
            {
                Orders o = null;
                var query = (from order in db.Orders
                             join orderDetails in db.OrderDetails on order.Id equals orderDetails.OrderId
                             join product in db.Products on orderDetails.ProductId equals product.Id
                             join category in db.Categories on product.CategoryId equals category.Id
                             where order.Id == id
                             select new { Orders = order, OrderDetails = orderDetails, Product = product, Category = category }
                            );
                foreach(var result in query)
                {
                    if (o == null) {
                        o = result.Orders;
                    }
                    if (o.OrderDetailsList == null) {
                        o.OrderDetailsList = new List<OrderDetails>();
                    }
                    result.OrderDetails.Product = result.Product;
                    if (result.OrderDetails.Product != null) {
                        result.OrderDetails.Product.Category = result.Category;
                    }
                    o.OrderDetailsList.Add(result.OrderDetails);
                }
                return o;
            }
        }

        // 2. Get orders by shipping name
        private static List<object>  GetOrderByShippingName(string shippingName)
        {
            using (var db = new NorthwindContex())
            {
                var ordersList = new List<object>();
                var query = (from order in db.Orders
                                          where order.ShipName != null
                                             && order.ShipName.ToLower().Contains(shippingName.ToLower())
                                  select new {order.Id, Date = order.OrderDate, order.ShipName, City = order.ShipCity}).ToList();
                foreach (var item in query)
                {
                    ordersList.Add(item);
                }
                return ordersList;
            }
        }

        // 3. Get all orders
        private static List<object> GetAllOrders()
        {
            using (var db = new NorthwindContex())
            {
                var ordersList = new List<object>();
                var query = (from order in db.Orders
                             select new { order.Id, Date = order.OrderDate, order.ShipName, City = order.ShipCity }).ToList();
                foreach (var item in query)
                {
                    ordersList.Add(item);
                }
                return ordersList;
            }
        }


        // 4.Get order details for specific orderid
        private static List<object> GetOrderDetailsByOrderId(int orderId)
        {
            using (var db = new NorthwindContex())
            {
                var orderDetailsList = new List<object>();
                var query = (from orderDetails in db.OrderDetails
                             join product in db.Products on orderDetails.ProductId equals product.Id
                             where orderDetails.OrderId == orderId
                             select new { productName = product.Name, unitPrice = orderDetails.UnitPrice, quantity = orderDetails.OrderQuantity}).ToList();
                foreach (var item in query)
                {
                    orderDetailsList.Add(item);
                }
                return orderDetailsList;
            }
        }

        // 5.Get order details for specific productid
        private static List<object> GetOrderDetailsByProductId(int productId)
        {
            using (var db = new NorthwindContex())
            {
                var orderDetailsList = new List<object>();
                var query = (from orderDetails in db.OrderDetails
                             join order in db.Orders on orderDetails.OrderId equals order.Id
                             where orderDetails.ProductId == productId
                             select new { order.OrderDate, orderDetails.UnitPrice, quantity = orderDetails.OrderQuantity }
                            );
                foreach(var item in query)
                {
                    orderDetailsList.Add(item);
                }
                return orderDetailsList;
            }
        }

        // 6.Get product by id
        private static Products GetProductById(int productId)
        {
            using (var db = new NorthwindContex())
            {
                Products products =
                    (from product in db.Products
                     where product.Id == productId
                     select product
                    ).First();
                return products;
                // category name needs to be added
            }
        }

        // 7. Get a list of products that contain substring
        private static List<Products> GetProductsBySubString(string searchValue)
        {
            using (var db = new NorthwindContex())
            {
                List<Products> products =
                    (from product in db.Products
                     where product.Name != null && product.Name.ToLower().Contains(searchValue)
                     select product
                    ).ToList();
                return products;
            }
        }

        // 8. Get products by categoryid
        private static List<Products> GetProductsByCategoryId(int categoryId)
        {
            using (var db = new NorthwindContex())
            {
                List <Products> products =
                (from product in db.Products
                 where product.CategoryId == categoryId
                 select product
                ).ToList();
                return products;
            }
        }

        // 9. Get category by id
        private static Category GetCategoryById(int id)
        {
            using (var db = new NorthwindContex())
            {
                Category c = (from category in db.Categories where category.Id == id select category).First();
                return c;
            }
        }

        // 10. Get all categories
        private static List<Category> GetAllCategories()
        {
            using (var db = new NorthwindContex())
            {
                List<Category> categories = (from category in db.Categories select category).ToList();
                return categories;
            }
        }
    }
}

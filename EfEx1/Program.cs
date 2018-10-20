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

            //var result = GetAllCategories();
            //foreach (var res in result)
            //{
            //    Console.WriteLine("result: " + res);
            //}
            var p = UpdateCategory(9, "name updt", "Desc changed");
            Console.WriteLine("result: " + p);
        }
        // 1.Get a single order by ID
        private static object GetSingleOrderById(int id)
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

        // 2.Get orders by shipping name
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

        // 3.Get all orders
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
        private static Object GetProductById(int productId)
        {
            using (var db = new NorthwindContex())
            {
                return (from product in db.Products
                             join category in db.Categories on product.CategoryId equals category.Id
                             where product.Id == productId
                             select new { name = product.Name, unitPrice = product.UnitPrice, categoryName = category.CategoryName }).First();
            }
        }

        // 7.Get a list of products that contain substring
        private static List<object> GetProductsBySubString(string searchValue)
        {
            using (var db = new NorthwindContex())
            {
                var responseList = new List<object>();
                var query = (from product in db.Products
                             join category in db.Categories on product.CategoryId equals category.Id
                             where product.Name != null && product.Name.ToLower().Contains(searchValue.ToLower())
                             select new { productName = product.Name, categoryName = category.CategoryName });
                foreach (var item in query)
                {
                    responseList.Add(item);
                }
                return responseList;
            }
        }

        // 8.Get products by categoryid
        private static List<object> GetProductsByCategoryId(int categoryId)
        {
            using (var db = new NorthwindContex())
            {
                var responseList = new List<object>();
                var query = (from product in db.Products
                             join category in db.Categories on product.CategoryId equals category.Id
                             where category.Id == categoryId
                             select new { name = product.Name, unitPrice = product.UnitPrice, categoryName = category.CategoryName });
                foreach(var item in query)
                {
                    responseList.Add(item);
                }
                return responseList;
            }
        }

        // 9.Get category by id
        private static Category GetCategoryById(int id)
        {
            using (var db = new NorthwindContex())
            {
                if ((from category in db.Categories where category.Id == id select category).Any()) {
                    Category c = (from category in db.Categories where category.Id == id select category).First();
                    return c;
                }
                return null;
            }
        }

        // 10.Get all categories
        private static List<object> GetAllCategories()
        {
            using (var db = new NorthwindContex())
            {
                List<object> categories = new List<object>();
                var query = (from category
                                           in db.Categories
                                           select new {Id = category.Id, Name = category.CategoryName, Description = category.CategoryDescription});
                foreach(var item in query)
                {
                    categories.Add(item);
                }
                return categories;
            }
        }

        // 11.Add category
        private static Category AddCategory(string name, string description)
        {
            using (var db = new NorthwindContex())
            {
                if (name.Length <= 15) {
                    int maxId = db.Categories.OrderByDescending(u => u.Id).FirstOrDefault().Id;
                    Category category = new Category
                    {
                        Id = maxId + 1,
                        CategoryName = name,
                        CategoryDescription = description
                    };
                    db.Categories.Add(category);
                    db.SaveChanges();
                    return category;
                }
                return null;
            }
        }

        // 12.Update category
        private static Boolean UpdateCategory(int categoryId, string name, string description)
        {
            using (var db = new NorthwindContex())
            {
                if (name != null && name.Length <= 15 && (from category in db.Categories where category.Id == categoryId select category).Any())
                {
                    var category = (from c in db.Categories where c.Id == categoryId select c).First();
                    category.CategoryName = name;
                    category.CategoryDescription = description;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        // 13.Delete category
        private static Boolean DeleteCategory(int categoryId)
        {
            using (var db = new NorthwindContex())
            {
                if ((from category in db.Categories where category.Id == categoryId select category).Any()) {
                    var category = (from c in db.Categories
                                    where c.Id == categoryId
                                    select c).First();
                    db.Categories.Remove(category);
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }


    }
}

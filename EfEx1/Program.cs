using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace EfEx1
{
    public class Program
    {
        static void Main(string[] args)
        {

        }

        // 1.Get a single order by ID
        public Orders GetOrder(int id)
        {
            using (var db = new NorthwindContex())
            {
                return db.Orders.Include(x => x.OrderDetailsList).
                    ThenInclude(x => x.Product).
                    ThenInclude(x => x.Category)
                    .FirstOrDefault(x => x.Id == id);
            }
        }

        // 2.Get orders by shipping name
        public List<object>  GetOrderByShippingName(string shippingName)
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
        public List<Orders> GetOrders()
        {
            using (var db = new NorthwindContex())
            {
                return (from order in db.Orders
                             select order).ToList();
            }
        }


        // 4.Get order details for specific orderid
        public List<OrderDetails> GetOrderDetailsByOrderId(int orderId)
        {
            using (var db = new NorthwindContex())
            {
                var orderDetails = db.OrderDetails
                     .Include(x => x.Order)
                     .Where(x => x.Order.Id == orderId)
                     .ToList();
                return orderDetails;
            }
        }

        // 5.Get order details for specific productid
        public List<OrderDetails> GetOrderDetailsByProductId(int productId)
        {
            using (var db = new NorthwindContex())
            {
                var orderDetails = db.OrderDetails
                    .Include(x => x.Order)
                    .Where(x => x.Product.Id == productId)
                    .ToList();
                return orderDetails;
            }
        }

        // 6.Get product by id
        public Products GetProduct(int productId)
        {
            using (var db = new NorthwindContex())
            {
                var product = db.Products
                    .Include(x => x.Category)
                    .FirstOrDefault(x => x.Id == productId);

                return product;
            }
        }

        // 7.Get a list of products that contain substring
        public List<Products> GetProductByName(string searchValue)
        {
            using (var db = new NorthwindContex())
            {
                return (from product in db.Products
                                    join category in db.Categories on product.CategoryId equals category.Id
                                    where product.Name != null && product.Name.ToLower().Contains(searchValue.ToLower())
                                    select product).ToList();

            }
        }

        // 8.Get products by categoryid
        public List<Products> GetProductByCategory(int categoryId)
        {
            using (var db = new NorthwindContex())
            {
                var products = db.Products
                    .Include(x => x.Category)
                    .Where(x => x.Category.Id == categoryId)
                    .ToList();

                return products;
            }
        }

        // 9.Get category by id
        public Category GetCategory(int id)
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
        public List<Category> GetAllCategories()
        {
            using (var db = new NorthwindContex())
            {
                List<Category> categories = (from category
                                           in db.Categories
                                             select category).ToList();
                return categories;
            }
        }

        // 11.Add category
        public Category CreateCategory(string name, string description)
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
        public Boolean UpdateCategory(int categoryId, string name, string description)
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
        public Boolean DeleteCategory(int categoryId)
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

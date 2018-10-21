using System;
using System.Linq;
using EfEx1;
using Xunit;

namespace Tests
{
        public class DataServiceTests
        {
            /* Categories */

            [Fact]
            public void Category_Object_HasIdNameAndDescription()
            {
                var category = new Category();
                Assert.Equal(0, category.Id);
                Assert.Null(category.CategoryName);
                Assert.Null(category.CategoryDescription);
            }

            [Fact]
            public void GetAllCategories_NoArgument_ReturnsAllCategories()
            {
                var service = new Program();
                var categories = service.GetAllCategories();
                Assert.Equal(8, categories.Count);
            Assert.Equal("Beverages", categories.First().CategoryName);
            }

            [Fact]
            public void GetCategory_ValidId_ReturnsCategoryObject()
            {
                var service = new Program();
                var category = service.GetCategory(1);
                Assert.Equal("Beverages", category.CategoryName);
            }

            [Fact]
            public void CreateCategory_ValidData_CreteCategoryAndRetunsNewObject()
            {
                var service = new Program();
                var category = service.CreateCategory("Test", "CreateCategory_ValidData_CreteCategoryAndRetunsNewObject");
                Assert.True(category.Id > 0);
                Assert.Equal("Test", category.CategoryName);
                Assert.Equal("CreateCategory_ValidData_CreteCategoryAndRetunsNewObject", category.CategoryDescription);

                // cleanup
                service.DeleteCategory(category.Id);
            }

            [Fact]
            public void DeleteCategory_ValidId_RemoveTheCategory()
            {
                var service = new Program();
                var category = service.CreateCategory("Test", "DeleteCategory_ValidId_RemoveTheCategory");
                var result = service.DeleteCategory(category.Id);
                Assert.True(result);
                category = service.GetCategory(category.Id);
                Assert.Null(category);
            }

            [Fact]
            public void DeleteCategory_InvalidId_ReturnsFalse()
            {
                var service = new Program();
                var result = service.DeleteCategory(-1);
                Assert.False(result);
            }

            [Fact]
            public void UpdateCategory_NewNameAndDescription_UpdateWithNewValues()
            {
                var service = new Program();
                var category = service.CreateCategory("TestingUpdate", "UpdateCategory_NewNameAndDescription_UpdateWithNewValues");

                var result = service.UpdateCategory(category.Id, "UpdatedName", "UpdatedDescription");
                Assert.True(result);

                category = service.GetCategory(category.Id);

            Assert.Equal("UpdatedName", category.CategoryName);
                Assert.Equal("UpdatedDescription", category.CategoryDescription);

                // cleanup
                service.DeleteCategory(category.Id);
            }

            [Fact]
            public void UpdateCategory_InvalidID_ReturnsFalse()
            {
                var service = new Program();
                var result = service.UpdateCategory(-1, "UpdatedName", "UpdatedDescription");
                Assert.False(result);
            }

            /* products */

            [Fact]
            public void Product_Object_HasIdNameUnitPriceQuantityPerUnitAndUnitsInStock()
            {
                var product = new Products();
                Assert.Equal(0, product.Id);
                Assert.Null(product.Name);
                Assert.Equal(0.0, product.UnitPrice);
                Assert.Null(product.QuantityPerUnit);
                Assert.Equal(0, product.UnitsInStock);
            }

            [Fact]
            public void GetProduct_ValidId_ReturnsProductWithCategory()
            {
                var service = new Program();
                var product = service.GetProduct(1);
                Assert.Equal("Chai", product.Name);
            Assert.Equal("Beverages", product.Category.CategoryName);
            }

            [Fact]
            public void GetProduct_NameSubString_ReturnsProductsThatMachesTheSubString()
            {
                var service = new Program();
                var products = service.GetProductByName("ant");
                Assert.Equal(3, products.Count);
                Assert.Equal("Chef Anton's Cajun Seasoning", products.First().Name);
                Assert.Equal("Guaraná Fantástica", products.Last().Name);
            }

            [Fact]
            public void GetProductsByCategory_ValidId_ReturnsProductWithCategory()
            {
                var service = new Program();
                var products = service.GetProductByCategory(1);
                Assert.Equal(12, products.Count);
                Assert.Equal("Chai", products.First().Name);
                Assert.Equal("Beverages", products.First().Category.CategoryName);
                Assert.Equal("Lakkalikööri", products.Last().Name);
            }

            /* orders */
            [Fact]
            public void Order_Object_HasIdDatesAndOrderDetails()
            {
                var order = new Orders();
                Assert.Equal(0, order.Id);
                Assert.Equal(new DateTime(), order.OrderDate);
                Assert.Equal(new DateTime(), order.RequiredDate);
                Assert.Null(order.OrderDetailsList);
                Assert.Null(order.ShipName);
                Assert.Null(order.ShipCity);
            }

            [Fact]
            public void GetOrder_ValidId_ReturnsCompleteOrder()
            {
                var service = new Program();
                var order = service.GetOrder(10248);
                Assert.Equal(3, order.OrderDetailsList.Count);
                Assert.Equal("Queso Cabrales", order.OrderDetailsList.First().Product.Name);
                Assert.Equal("Dairy Products", order.OrderDetailsList.First().Product.Category.CategoryName);
            }

            [Fact]
            public void GetOrders()
            {
                var service = new Program();
                var orders = service.GetOrders();
                Assert.Equal(830, orders.Count);
            }


            /* orderdetails */
            [Fact]
            public void OrderDetails_Object_HasOrderProductUnitPriceQuantityAndDiscount()
            {
                var orderDetails = new OrderDetails();
                Assert.Equal(0, orderDetails.OrderId);
               // Assert.Null(orderDetails.Order);
                Assert.Equal(0, orderDetails.ProductId);
                Assert.Null(orderDetails.Product);
                Assert.Equal(0.0, orderDetails.UnitPrice);
            Assert.Equal(0.0, orderDetails.OrderQuantity);
            Assert.Equal(0.0, orderDetails.OrderDiscount);
            }

            [Fact]
            public void GetOrderDetailByOrderId_ValidId_ReturnsProductNameUnitPriceAndQuantity()
            {
                var service = new Program();
                var orderDetails = service.GetOrderDetailsByOrderId(10248);
                Assert.Equal(3, orderDetails.Count);
                Assert.Equal("Queso Cabrales", orderDetails.First().Product.Name);
                Assert.Equal(14, orderDetails.First().UnitPrice);
            Assert.Equal(12, orderDetails.First().OrderQuantity);
            }

            [Fact]
            public void GetOrderDetailByProductId_ValidId_ReturnsOrderDateUnitPriceAndQuantity()
            {
                var service = new Program();
                var orderDetails = service.GetOrderDetailsByProductId(11);
                Assert.Equal(38, orderDetails.Count);
                Assert.Equal("1996-07-04", orderDetails.First().Order.OrderDate.ToString("yyyy-MM-dd"));
                Assert.Equal(14, orderDetails.First().UnitPrice);
                Assert.Equal(12, orderDetails.First().OrderQuantity);
            }
        }
}

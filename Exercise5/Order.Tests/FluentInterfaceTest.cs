using System;
using System.Linq;
using NUnit.Framework;

namespace Order.Tests
{
    [TestFixture]
    internal class FluentInterfaceTest
    {
        [SetUp]
        public void Setup()
        {
            _orderService = new OrderService();
        }

        [TearDown]
        public void Cleanup()
        {
            _orderService.Delete();
        }

        private OrderService _orderService;

        [Test]
        public void CreateTest()
        {
            var od1 = new OrderDetailsBuilder()
                .AddProduct("商品1", 1, 6)
                .GetOrderDetails();

            var od2 = new OrderDetailsBuilder()
                .AddProduct("商品2", 2, 5)
                .GetOrderDetails();

            var od3 = new OrderDetailsBuilder()
                .AddProduct("商品3", 3, 4).GetOrderDetails();

            var od4 = new OrderDetailsBuilder()
                .AddProduct("商品4", 4, 3).GetOrderDetails();

            _orderService.Create("1", od1, out var i1);
            _orderService.Create("2", od2, out var i2);
            _orderService.Create("3", od3, out var i3).Find(out var q1);

            Assert.AreEqual(1, i1);
            Assert.AreEqual(2, i2);
            Assert.AreEqual(3, i3);
            Assert.AreEqual(3, q1.Count);

            try
            {
                _orderService.Create("4", od1, out _);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.AreEqual("Already Existed", e.Message);
                _orderService.Reset();
            }
        }

        [Test]
        public void SortTest()
        {
            var od1 = new OrderDetailsBuilder()
                .AddProduct("商品1", 1, 1)
                .GetOrderDetails();

            var od2 = new OrderDetailsBuilder()
                .AddProduct("商品2", 3, 1)
                .GetOrderDetails();

            var od3 = new OrderDetailsBuilder()
                .AddProduct("商品3", 2, 1).GetOrderDetails();

            _orderService.Create("1", od1, out _);
            _orderService.Create("2", od2, out _);
            _orderService.Create("3", od3, out _);

            _orderService.Sort(OrderService.SortMethod.Ascending).First(out var o1);
            _orderService.Sort(OrderService.SortMethod.Descending).First(out var o2);

            Assert.AreEqual(1, o1.Price);
            Assert.AreEqual(3, o2.Price);

            _orderService
                .Sort(OrderService.SortMethod.Ascending, order => order.CreatedAt)
                .First(out var o3);
            _orderService
                .Sort(OrderService.SortMethod.Descending, order => order.CreatedAt)
                .First(out var o4);

            Assert.IsTrue(o4.CreatedAt.CompareTo(o3.CreatedAt) > 0);
        }

        [Test]
        public void QueryTest()
        {
            var od1 = new OrderDetailsBuilder()
                .AddProduct("商品1", 1, 6)
                .GetOrderDetails();

            var od2 = new OrderDetailsBuilder()
                .AddProduct("商品2", 2, 5)
                .AddProduct("商品3", 3, 4)
                .GetOrderDetails();

            var od3 = new OrderDetailsBuilder()
                .AddProduct("商品4", 4, 3)
                .AddProduct("商品5", 5, 2)
                .AddProduct("商品6", 6, 1)
                .GetOrderDetails();

            _orderService.Create("o1", od1, out _);
            _orderService.Create("o2", od2, out _);
            _orderService.Create("o3", od3, out _);

            // First should work
            _orderService.Where(o => o.Customer == "o1").First(out var q1);
            _orderService.Where(o => o.Customer == "o2").First(out var q2);
            _orderService.Where(o => o.Customer == "o3").First(out var q3);
            _orderService.Where(o => o.Price >= 22).Find(out var q4);
            Assert.AreEqual(od1, q1.Products);
            Assert.AreEqual(od2, q2.Products);
            Assert.AreEqual(od3, q3.Products);
            Assert.AreEqual(2, q4.Count);

            // Find or First should clear out previous status
            _orderService.Where(o => o.Customer == "o1").First(out _)
                .Where(o => o.Customer == "o2").First(out var q5);
            _orderService.Where(o => o.Price >= 22).Find(out _)
                .Where(o => o.Customer == "o1").First(out var q6);
            Assert.AreEqual(od2, q5.Products);
            Assert.AreEqual(od1, q6.Products);
        }

        [Test]
        public void DeleteTest()
        {
            var od1 = new OrderDetailsBuilder()
                .AddProduct("商品1", 1, 6)
                .GetOrderDetails();

            var od2 = new OrderDetailsBuilder()
                .AddProduct("商品2", 2, 5)
                .GetOrderDetails();

            _orderService.Create("o1", od1, out _);
            _orderService.Create("o2", od2, out _);

            // Delete method should commit at once
            _orderService
                .Where(o => o.Customer == "o2").Delete()
                .Find(out var q1);
            Assert.AreEqual(1, q1.Count);
            Assert.AreEqual(od1, q1.First().Products);

            // Delete is available without Where
            _orderService.Delete().Find(out var q2);
            Assert.AreEqual(0, q2.Count);
        }

        [Test]
        public void UpdateTest()
        {
            var od1 = new OrderDetailsBuilder()
                .AddProduct("商品1", 1, 1)
                .GetOrderDetails();

            var od2 = new OrderDetailsBuilder()
                .AddProduct("商品2", 3, 1)
                .GetOrderDetails();

            var od3 = new OrderDetailsBuilder()
                .AddProduct("商品3", 2, 1).GetOrderDetails();

            _orderService.Create("1", od1, out _);
            _orderService.Create("2", od2, out var id);

            _orderService.Where(o => o.OrderId == id).Update("new customer");
            _orderService.Where(o => o.Customer == "new customer").First(out var q1);
            Assert.AreEqual(od2, q1.Products);
            Assert.AreNotEqual(q1.CreatedAt, q1.UpdatedAt);

            _orderService.Where(o => o.Products == od1).Update(od3);
            _orderService.Where(o => o.Customer == "1").First(out var q2);
            Assert.AreEqual(od3, q2.Products);
            Assert.AreNotEqual(q2.CreatedAt, q2.UpdatedAt);

            try
            {
                _orderService.Where(o => o.Customer == "2").Update("3");
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.AreEqual("Nothing to update!", e.Message);
                _orderService.Reset();
            }

            try
            {
                _orderService.Update("3");
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.AreEqual("Try to update more than one record", e.Message);
                _orderService.Reset();
            }
        }
    }
}
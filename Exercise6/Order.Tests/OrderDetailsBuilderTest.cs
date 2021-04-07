using System;
using NUnit.Framework;

namespace Order.Tests
{
    [TestFixture]
    internal class OrderDetailsTest
    {
        [Test]
        public void Test()
        {
            var odb = new OrderDetailsBuilder()
                .AddProduct("a", 1, 2)
                .AddProduct(new Product {ProductName = "b", Price = 3}, 4);
            var od = odb.GetOrderDetails();
            Assert.AreEqual(2, od.Count);
            Assert.IsTrue(od[0].Price >= od[1].Price);

            try
            {
                var odb1 = new OrderDetailsBuilder()
                    .AddProduct("a", 1, 2)
                    .AddProduct("a", 3, 4);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(ArgumentException), e.GetType());
            }

            try
            {
                var odb1 = new OrderDetailsBuilder()
                    .AddProduct(new Product {ProductName = "a", Price = 1}, 2)
                    .AddProduct(new Product {ProductName = "a", Price = 3}, 4);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(ArgumentException), e.GetType());
            }
        }
    }
}
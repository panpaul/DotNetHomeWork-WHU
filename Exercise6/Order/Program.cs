using System;

namespace Order
{
    internal class Program
    {
        public static void Main(string[] args)
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

            var os = new OrderService();

            os.Create("1", od1, out _);
            os.Create("2", od2, out _);
            os.Create("3", od3, out _).Find(out _);

            os.Export("./test.xml");

            var os1 = new OrderService();
            os1.Import("./test.xml").Where(o => o.Customer == "1").First(out var o1);
            Console.WriteLine(o1);
        }
    }
}
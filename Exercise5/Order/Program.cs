using System;

namespace Order
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var od1 = new OrderDetailsBuilder()
                .AddProduct("商品1", 1, 6)
                .AddProduct("商品2", 2, 5)
                .AddProduct("商品3", 3, 4)
                .AddProduct("商品4", 4, 3)
                .AddProduct("商品5", 5, 2)
                .AddProduct("商品6", 6, 1)
                .GetOrderDetails();
            foreach (var o in od1) Console.WriteLine(o);
            Console.WriteLine();

            var od2 = new OrderDetailsBuilder()
                .AddProduct("g1", 2, 3)
                .AddProduct("g2", 5, 1)
                .GetOrderDetails();
            var od3 = new OrderDetailsBuilder()
                .AddProduct("gg1", 1, 5)
                .AddProduct("gg2", 4, 5)
                .GetOrderDetails();

            var os = new OrderService();
            os.Create("customer1", od1, out _)
                .Create("customer2", od2, out _)
                .Where(o => o.Customer == "customer2")
                .First(out var o1);
            Console.WriteLine(o1);
            Console.WriteLine();

            os.Where(o => o.OrderId == 1)
                .Update("c1")
                .Sort(OrderService.SortMethod.Descending)
                .Find(out var ol1);
            foreach (var o in ol1) Console.WriteLine(o);
            Console.WriteLine();

            os.Where(o => o.OrderId == 1)
                .Delete()
                .Create("c3", od3, out _)
                .Sort(OrderService.SortMethod.Descending, o => o.CreatedAt)
                .Find(out var ol2);
            foreach (var o in ol2) Console.WriteLine(o);
        }
    }
}
using System;
using System.Collections.Generic;

namespace OrderView
{
    public class OrderDetailsBuilder
    {
        private readonly List<OrderDetails> products;

        public OrderDetailsBuilder()
        {
            products = new List<OrderDetails>();
        }

        public OrderDetailsBuilder AddProduct(Product product, uint amount)
        {
            if (products.Exists(o => o.Product == product))
                throw new ArgumentException("product already in order details!");

            products.Add(new OrderDetails {Amount = amount, Product = product});
            return this;
        }

        public OrderDetailsBuilder AddProduct(string productName, double price, uint amount)
        {
            if (products.Exists(o => o.Product.ProductName == productName))
                throw new ArgumentException("product already in order details!");

            products.Add(new OrderDetails
            {
                Amount = amount,
                Product = new Product {ProductName = productName, Price = price}
            });
            return this;
        }

        public List<OrderDetails> GetOrderDetails()
        {
            products.Sort((od1, od2) => od2.Total.CompareTo(od1.Total));
            return products;
        }
    }
}
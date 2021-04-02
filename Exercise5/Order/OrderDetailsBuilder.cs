using System;
using System.Collections.Generic;

namespace Order
{
    public class OrderDetailsBuilder
    {
        private readonly List<OrderDetails> _products;

        public OrderDetailsBuilder()
        {
            _products = new List<OrderDetails>();
        }

        public OrderDetailsBuilder AddProduct(Product product, uint amount)
        {
            if (_products.Exists(o => o.Product == product))
                throw new ArgumentException("product already in order details!");

            _products.Add(new OrderDetails {Amount = amount, Product = product});
            return this;
        }

        public OrderDetailsBuilder AddProduct(string productName, double price, uint amount)
        {
            if (_products.Exists(o => o.Product.ProductName == productName))
                throw new ArgumentException("product already in order details!");

            _products.Add(new OrderDetails
            {
                Amount = amount,
                Product = new Product {ProductName = productName, Price = price}
            });
            return this;
        }

        public List<OrderDetails> GetOrderDetails()
        {
            _products.Sort((od1, od2) => od2.Price.CompareTo(od1.Price));
            return _products;
        }
    }
}
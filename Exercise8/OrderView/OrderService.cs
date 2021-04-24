using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace OrderView
{
    public class OrderService
    {
        public enum SortMethod
        {
            Ascending,
            Descending
        }

        private IEnumerable<Order> lastResult;
        private List<Order> orders;

        private uint pKeyCounter;

        public OrderService()
        {
            orders = new List<Order>();
            lastResult = orders;
            pKeyCounter = 1;
        }

        private uint Create(Order order)
        {
            var f = orders.Find(o => o == order);
            if (f is not null && f.OrderId != 0) throw new Exception("Already Existed");

            order.OrderId = pKeyCounter++;
            order.CreatedAt = DateTime.Now;
            orders.Add(order);

            lastResult = orders;
            return pKeyCounter - 1;
        }

        public OrderService Create(Order order, out uint index)
        {
            index = Create(order);
            return this;
        }

        public OrderService Create(string customerName, List<OrderDetails> products, out uint index)
        {
            var order = new Order {Customer = customerName, Products = products};
            index = Create(order);
            return this;
        }

        public OrderService Where(Func<Order, bool> predicate)
        {
            lastResult = lastResult.Where(predicate);
            return this;
        }

        public OrderService Sort(SortMethod sort)
        {
            lastResult = sort switch
            {
                SortMethod.Ascending => lastResult.OrderBy(od => od.Price),
                SortMethod.Descending => lastResult.OrderByDescending(od => od.Price),
                _ => throw new ArgumentOutOfRangeException(nameof(sort), sort, null)
            };

            return this;
        }

        public OrderService Sort<TKey>(SortMethod sort, Func<Order, TKey> selector)
        {
            lastResult = sort switch
            {
                SortMethod.Ascending => lastResult.OrderBy(selector),
                SortMethod.Descending => lastResult.OrderByDescending(selector),
                _ => throw new ArgumentOutOfRangeException(nameof(sort), sort, null)
            };

            return this;
        }

        public OrderService Delete()
        {
            orders = orders.Except(lastResult).ToList();
            lastResult = orders;
            return this;
        }

        public OrderService Find(out List<Order> orderList)
        {
            orderList = lastResult.ToList();
            lastResult = orders;
            return this;
        }

        public OrderService First(out Order order)
        {
            order = lastResult.ToList().First();
            lastResult = orders;
            return this;
        }

        public OrderService Update(string customerName)
        {
            if (!lastResult.Any()) throw new Exception("Nothing to update!");
            if (lastResult.Count() > 1) throw new Exception("Try to update more than one record");
            lastResult.First().Customer = customerName;
            lastResult.First().UpdatedAt = DateTime.Now;
            lastResult = orders;
            return this;
        }

        public OrderService Update(List<OrderDetails> products)
        {
            if (!lastResult.Any()) throw new Exception("Nothing to update!");
            if (lastResult.Count() > 1) throw new Exception("Try to update more than one record");
            var order = lastResult.First();
            order.Products = products;
            order.UpdatedAt = DateTime.Now;
            lastResult = orders;
            return this;
        }

        public OrderService Reset()
        {
            lastResult = orders;
            return this;
        }

        public OrderService Export(string filename)
        {
            using var writer = new StreamWriter(filename);
            var serializer = new XmlSerializer(typeof(List<Order>));
            serializer.Serialize(writer, lastResult);
            lastResult = orders;
            return this;
        }

        public OrderService Export(Stream stream)
        {
            var serializer = new XmlSerializer(typeof(List<Order>));
            serializer.Serialize(stream, lastResult);
            lastResult = orders;
            return this;
        }

        public OrderService Import(string filename)
        {
            using var fs = new FileStream(filename, FileMode.Open);
            var serializer = new XmlSerializer(typeof(List<Order>));
            var o = (List<Order>) serializer.Deserialize(fs);
            if (o is null)
            {
                o = new List<Order>();
                pKeyCounter = 1;
            }
            else
            {
                pKeyCounter = o.OrderByDescending(od => od.OrderId).First().OrderId + 1;
            }

            orders = o;
            lastResult = orders;
            return this;
        }

        public OrderService Import(Stream stream)
        {
            var serializer = new XmlSerializer(typeof(List<Order>));
            var o = (List<Order>) serializer.Deserialize(stream);
            if (o is null)
            {
                o = new List<Order>();
                pKeyCounter = 1;
            }
            else
            {
                pKeyCounter = o.OrderByDescending(od => od.OrderId).First().OrderId + 1;
            }

            orders = o;
            lastResult = orders;
            return this;
        }

        public uint GetNextKey()
        {
            return pKeyCounter;
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Order
{
    public class OrderService
    {
        public enum SortMethod
        {
            Ascending,
            Descending
        }

        private IEnumerable<Order> _lastResult;
        private List<Order> _orders;

        private uint _pKeyCounter;

        public OrderService()
        {
            _orders = new List<Order>();
            _lastResult = _orders;
            _pKeyCounter = 1;
        }

        private uint Create(Order order)
        {
            var f = _orders.Find(o => o == order);
            if (f is not null && f.OrderId != 0) throw new Exception("Already Existed");

            order.OrderId = _pKeyCounter++;
            order.CreatedAt = DateTime.Now;
            _orders.Add(order);

            _lastResult = _orders;
            return _pKeyCounter - 1;
        }

        public OrderService Create(string customerName, List<OrderDetails> products, out uint index)
        {
            var order = new Order {Customer = customerName, Products = products};
            index = Create(order);
            return this;
        }

        public OrderService Where(Func<Order, bool> predicate)
        {
            _lastResult = _lastResult.Where(predicate);
            return this;
        }

        public OrderService Sort(SortMethod sort)
        {
            _lastResult = sort switch
            {
                SortMethod.Ascending => _lastResult.OrderBy(od => od.Price),
                SortMethod.Descending => _lastResult.OrderByDescending(od => od.Price),
                _ => throw new ArgumentOutOfRangeException(nameof(sort), sort, null)
            };

            return this;
        }

        public OrderService Sort<TKey>(SortMethod sort, Func<Order, TKey> selector)
        {
            _lastResult = sort switch
            {
                SortMethod.Ascending => _lastResult.OrderBy(selector),
                SortMethod.Descending => _lastResult.OrderByDescending(selector),
                _ => throw new ArgumentOutOfRangeException(nameof(sort), sort, null)
            };

            return this;
        }

        public OrderService Delete()
        {
            _orders = _orders.Except(_lastResult).ToList();
            _lastResult = _orders;
            return this;
        }

        public OrderService Find(out List<Order> orderList)
        {
            orderList = _lastResult.ToList();
            _lastResult = _orders;
            return this;
        }

        public OrderService First(out Order order)
        {
            order = _lastResult.ToList().First();
            _lastResult = _orders;
            return this;
        }

        public OrderService Update(string customerName)
        {
            if (!_lastResult.Any()) throw new Exception("Nothing to update!");
            if (_lastResult.Count() > 1) throw new Exception("Try to update more than one record");
            _lastResult.First().Customer = customerName;
            _lastResult.First().UpdatedAt = DateTime.Now;
            _lastResult = _orders;
            return this;
        }

        public OrderService Update(List<OrderDetails> products)
        {
            if (!_lastResult.Any()) throw new Exception("Nothing to update!");
            if (_lastResult.Count() > 1) throw new Exception("Try to update more than one record");
            var order = _lastResult.First();
            order.Products = products;
            order.UpdatedAt = DateTime.Now;
            _lastResult = _orders;
            return this;
        }

        public OrderService Reset()
        {
            _lastResult = _orders;
            return this;
        }

        public OrderService Export(string filename)
        {
            using var writer = new StreamWriter(filename);
            var serializer = new XmlSerializer(typeof(List<Order>));
            serializer.Serialize(writer, _lastResult);
            _lastResult = _orders;
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
                _pKeyCounter = 1;
            }
            else
            {
                _pKeyCounter = o.OrderByDescending(od => od.OrderId).First().OrderId + 1;
            }

            _orders = o;
            _lastResult = _orders;
            return this;
        }
    }
}
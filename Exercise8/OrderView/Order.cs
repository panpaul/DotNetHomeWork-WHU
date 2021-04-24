using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderView
{
    public class Order
    {
        public uint OrderId { get; set; }
        public List<OrderDetails> Products { get; set; }
        public string Customer { get; set; }
        public double Price => Products.Sum(p => p.Total);
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public override string ToString()
        {
            return
                $"Order ID: {OrderId}; Customer: {Customer}; Total: {Price}; Created At: {CreatedAt}; Updated At: {UpdatedAt}";
        }

        protected bool Equals(Order other)
        {
            return OrderId == other.OrderId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Order) obj);
        }

        public override int GetHashCode()
        {
            return (int) OrderId;
        }

        public static bool operator ==(Order left, Order right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Order left, Order right)
        {
            return !Equals(left, right);
        }
    }
}
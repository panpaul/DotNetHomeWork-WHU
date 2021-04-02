using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Order
{
    public class Order
    {
        [Key] public uint OrderId { get; set; }
        [Required] public List<OrderDetails> Products { get; set; }
        [Required] public string Customer { get; set; }
        public double Price => Products.Sum(p => p.Price);
        [Required] public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public override string ToString()
        {
            return $"Order ID: {OrderId}; Customer: {Customer}; Price: {Price}; Created At: {CreatedAt}; Updated At: {UpdatedAt}";
        }

        private bool Equals(Order other)
        {
            return OrderId == other.OrderId || Equals(Products, other.Products);
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
            return HashCode.Combine(OrderId, Products);
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
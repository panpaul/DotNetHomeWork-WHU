using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace OrderAPI.Models
{
    public class Order
    {
        [Key] public int OrderId { get; set; }

        public virtual List<OrderDetails> Products { get; set; }
        public string Customer { get; set; }
        public double Price => Products.Sum(p => p.Total);

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
            return OrderId;
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
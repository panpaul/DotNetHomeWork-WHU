using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderView
{
    public class Order
    {
        [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerat‌ed(System.ComponentM‌​odel.DataAnnotations‌​.Schema.DatabaseGeneratedOp‌​tion.None)]
        public int OrderID { get; set; }
        public virtual List<OrderDetails> Products { get; set; }
        public string Customer { get; set; }
        public double Price => Products.Sum(p => p.Total);
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public override string ToString()
        {
            return
                $"Order ID: {OrderID}; Customer: {Customer}; Total: {Price}; Created At: {CreatedAt}; Updated At: {UpdatedAt}";
        }

        protected bool Equals(Order other)
        {
            return OrderID == other.OrderID;
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
            return OrderID;
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
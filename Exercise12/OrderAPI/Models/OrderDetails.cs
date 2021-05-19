using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderAPI.Models
{
    public class OrderDetails
    {
        public OrderDetails()
        {
            Product = new Product();
            Amount = 0;
        }

        [NotMapped] public Product Product { get; set; }

        [Key] public int Id { get; set; }

        public string ProductName
        {
            get => Product.ProductName;
            set => Product.ProductName = value;
        }

        public double Price
        {
            get => Product.Price;
            set => Product.Price = value;
        }

        public int Amount { get; set; }
        public double Total => Amount * Product.Price;

        public override string ToString()
        {
            return $"OrderDetails: {Product}: {Price} * {Amount} Total: {Total}";
        }

        protected bool Equals(OrderDetails other)
        {
            return Equals(Product, other.Product);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((OrderDetails) obj);
        }

        public override int GetHashCode()
        {
            return Product != null ? Product.GetHashCode() : 0;
        }

        public static bool operator ==(OrderDetails left, OrderDetails right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(OrderDetails left, OrderDetails right)
        {
            return !Equals(left, right);
        }
    }
}
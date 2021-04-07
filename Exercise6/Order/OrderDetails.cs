using System.ComponentModel.DataAnnotations;

namespace Order
{
    public class OrderDetails
    {
        [Required] public Product Product { get; set; }
        [Required] public uint Amount { get; set; }
        public double Price => Amount * Product.Price;

        public override string ToString()
        {
            return $"OrderDetails: {Product} * {Amount} Price: {Price}";
        }

        private bool Equals(OrderDetails other)
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
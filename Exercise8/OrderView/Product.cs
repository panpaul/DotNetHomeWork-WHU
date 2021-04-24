namespace OrderView
{
    public class Product
    {
        public string ProductName { get; set; }

        public double Price { get; set; }

        private bool Equals(Product other)
        {
            return ProductName == other.ProductName;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Product) obj);
        }

        public override int GetHashCode()
        {
            return ProductName != null ? ProductName.GetHashCode() : 0;
        }

        public static bool operator ==(Product left, Product right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Product left, Product right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return $"Product: {ProductName}(￥{Price})";
        }
    }
}
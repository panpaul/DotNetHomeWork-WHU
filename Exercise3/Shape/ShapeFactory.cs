namespace Shape
{
    internal static class ShapeFactory
    {
        public static IShape GetShape(string type, params double[] args)
        {
            return type switch
            {
                "Rectangle" => args.Length >= 2 ? new Rectangle(args[0], args[1]) : null,
                "Square" => args.Length >= 1 ? new Square(args[0]) : null,
                "Triangle" => args.Length >= 3 ? new Triangle(args[0], args[1], args[2]) : null,
                null => null,
                _ => null
            };
        }
    }
}
using System;

namespace Shape
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var rectangle = new Rectangle(5, 6);
            Console.WriteLine($"Rectangle: Area({rectangle.GetArea()}) Valid({(rectangle.IsValid() ? "Y" : "N")})");
            var square = new Square(5);
            Console.WriteLine($"Square: Area({square.GetArea()}) Valid({(square.IsValid() ? "Y" : "N")})");
            var triangle = new Triangle(3, 4, 5);
            Console.WriteLine($"Triangle: Area({triangle.GetArea()}) Valid({(triangle.IsValid() ? "Y" : "N")})");

            Console.WriteLine("");

            var rnd = new Random();
            string[] name = {"Rectangle", "Square", "Triangle"};
            double sum = 0;
            for (var i = 0; i < 10; i++)
            {
                var id = rnd.Next(0, 3);
                IShape shape;
                do
                {
                    shape = ShapeFactory.GetShape(name[id],
                        rnd.Next(1, 10), rnd.Next(1, 10), rnd.Next(1, 10));
                } while (!shape.IsValid());

                Console.WriteLine($"Generated a {name[id]}");
                sum += shape.GetArea();
            }

            Console.WriteLine($"Total Area: {sum}");
        }
    }
}
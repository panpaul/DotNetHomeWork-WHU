namespace Shape
{
    internal class Rectangle : IShape
    {
        private readonly double _height;
        private readonly double _width;

        public Rectangle(double height, double width)
        {
            _height = height;
            _width = width;
        }

        public double GetArea()
        {
            return _height * _width;
        }

        public bool IsValid()
        {
            return _height > 0 && _width > 0;
        }
    }
}
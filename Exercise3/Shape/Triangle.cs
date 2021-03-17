using System;

namespace Shape
{
    internal class Triangle : IShape
    {
        private readonly double _a, _b, _c;

        public Triangle(double a, double b, double c)
        {
            _a = a;
            _b = b;
            _c = c;
        }

        public double GetArea()
        {
            var s = (_a + _b + _c) / 2;
            return Math.Sqrt(s * (s - _a) * (s - _b) * (s - _c));
        }

        public bool IsValid()
        {
            return _a > 0 && _b > 0 && _c > 0 &&
                   _a + _b > _c && _a + _c > _b && _b + _c > _a;
        }
    }
}
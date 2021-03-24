using System;

namespace Chain
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var intList = new GenericList<int>();

            for (var x = -5; x < 5; x++)
                intList.Add(x);

            var sum = 0;
            var max = int.MinValue;
            var min = int.MaxValue;

            static void Println(int i) => Console.WriteLine(i);
            void CalcSum(int i) => sum += i;
            void CalcMax(int i) => max = Math.Max(max, i);
            void CalcMin(int i) => min = Math.Min(min, i);

            intList.ForEach(Println);
            intList.ForEach(CalcSum);
            intList.ForEach(CalcMax);
            intList.ForEach(CalcMin);

            Console.WriteLine($"Max:{max}, Min:{min}, Sum:{sum}");
        }
    }
}
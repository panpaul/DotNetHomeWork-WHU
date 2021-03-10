using System;
using System.Linq;

namespace Statistic
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Please input an array of numbers in one line:");

            var str = Console.ReadLine()?.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (str == null)
            {
                Console.WriteLine("Wrong Input");
                return;
            }

            try
            {
                var num = Array.ConvertAll(str, Convert.ToInt32);
                var max = num.Max();
                var min = num.Min();
                var avg = num.Average();
                var sum = num.Sum();
                Console.WriteLine(@$"Max:{max}
                                     Min:{min}
                                     Average:{avg}
                                     Sum:{sum}".Replace(' ', '\0'));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
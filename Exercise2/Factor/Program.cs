using System;

namespace Factor
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Please input a number:");
            if (!int.TryParse(Console.ReadLine(), out var num))
            {
                Console.WriteLine("Wrong Input!");
                return;
            }

            Console.Write("Factors: ");
            for (var i = 2; i <= num; i++)
                if (num % i == 0)
                {
                    Console.Write($"{i} ");
                    num /= i;
                }

            Console.WriteLine();
        }
    }
}
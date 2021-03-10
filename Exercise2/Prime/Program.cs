using System;
using System.Collections.Generic;
using System.Linq;

namespace Prime
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var num = Enumerable.Range(2, 99).ToArray();
            
            Console.Write("Primes(tail): ");
            TailPrime(num);
            Console.WriteLine();

            Console.Write("Primes(loop): ");
            LoopPrime(num);
            Console.WriteLine();
        }

        private static void TailPrime(int[] num)
        {
            if (num.Length == 0) return;

            var p = num[0];
            Console.Write($"{p} ");

            var next = new List<int>();
            Array.ForEach(num, n =>
            {
                if (n % p != 0)
                    next.Add(n);
            });
            TailPrime(next.ToArray());
        }

        private static void LoopPrime(int[] num)
        {
            while (num.Length != 0)
            {
                var p = num[0];
                Console.Write($"{p} ");
                var next = new List<int>();
                Array.ForEach(num, n =>
                {
                    if (n % p != 0)
                        next.Add(n);
                });
                num = next.ToArray();
            }
        }
    }
}
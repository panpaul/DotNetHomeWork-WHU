using System;

namespace Matrix
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Please input M: ");
            if (!int.TryParse(Console.ReadLine()?.Trim(), out var m))
            {
                Console.WriteLine("Wrong Input");
                return;
            }

            Console.WriteLine("Please input N: ");
            if (!int.TryParse(Console.ReadLine()?.Trim(), out var n))
            {
                Console.WriteLine("Wrong Input");
                return;
            }

            var matrix = new int[m, n];
            Console.WriteLine("Please input the matrix: ");
            try
            {
                for (var i = 0; i < m; i++)
                {
                    var line = Console.ReadLine()?.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (line?.Length != n)
                    {
                        Console.WriteLine("Wrong Input");
                        return;
                    }

                    var num = Array.ConvertAll(line, Convert.ToInt32);
                    for (var j = 0; j < n; j++)
                        matrix[i, j] = num[j];
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            for (var i = 0; i < n; i++)
            for (var j = 0; j < m && j + i < n; j++)
                if (matrix[j, i + j] != matrix[0, i])
                {
                    Console.WriteLine("False");
                    return;
                }

            for (var j = 0; j < m; j++)
            for (var i = 0; i < n && i + j < m; i++)
                if (matrix[j + i, i] != matrix[j, 0])
                {
                    Console.WriteLine("False");
                    return;
                }

            Console.WriteLine("True");
        }
    }
}
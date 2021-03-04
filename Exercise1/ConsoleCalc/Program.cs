using System;

namespace ConsoleCalc
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Simple Calculator");
            Console.Write("Please input the first number: ");
            var num1 = Convert.ToDouble(Console.ReadLine());
            Console.Write("Please input the second number: ");
            var num2 = Convert.ToDouble(Console.ReadLine());
            Console.Write("Please input the operator: ");
            var op = Console.ReadLine();

            switch (op)
            {
                case "+":
                    Console.WriteLine($"Result: {num1 + num2}");
                    break;
                case "-":
                    Console.WriteLine($"Result: {num1 - num2}");
                    break;
                case "*":
                    Console.WriteLine($"Result: {num1 * num2}");
                    break;
                case "/":
                    Console.WriteLine($"Result: {num1 / num2}");
                    break;

                default:
                    Console.WriteLine("Invalid OP!");
                    return;
            }
        }
    }
}
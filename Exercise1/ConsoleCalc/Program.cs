using System;

namespace ConsoleCalc
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            double num1, num2;

            Console.WriteLine("Simple Calculator");

            Console.Write("Please input the first number: ");
            if (!double.TryParse(Console.ReadLine(), out num1))
            {
                Console.WriteLine("Please input a number!");
                return;
            }


            Console.Write("Please input the second number: ");
            if (!double.TryParse(Console.ReadLine(), out num2))
            {
                Console.WriteLine("Please input a number!");
                return;
            }

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
                    if (num2 == 0)
                    {
                        Console.WriteLine("Divided by zero!");
                        return;
                    }

                    Console.WriteLine($"Result: {num1 / num2}");
                    break;

                default:
                    Console.WriteLine("Invalid OP!");
                    return;
            }
        }
    }
}
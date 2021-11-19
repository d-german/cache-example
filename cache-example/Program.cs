using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace cache_example
{
    public class Program
    {
        private static readonly IDictionary<int, int> Cache = new Dictionary<int, int>();

        private static int ExpensiveCalculation(int value)
        {
            Thread.Sleep(2000);
            return value * 50;
        }

        private static int GetValue(int key)
        {
            if (!Cache.ContainsKey(key))
            {
                Cache[key] = ExpensiveCalculation(key);
            }

            return Cache[key];
        }

        private static void Main(string[] args)
        {
            var sw = new Stopwatch();
            sw.Start();
            Console.WriteLine("....");
            Console.WriteLine(GetValue(5));
            Console.WriteLine(GetValue(10));
            Console.WriteLine(GetValue(5));
            Console.WriteLine(GetValue(10));
            Console.WriteLine($"{sw.ElapsedMilliseconds} ms");
        }
    }
}

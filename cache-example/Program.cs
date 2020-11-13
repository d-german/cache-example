using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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

        private static Task<int> GetValue(int key)
        {
            return Task.Run(() =>
            {
                if (!Cache.ContainsKey(key))
                {
                    Cache[key] = ExpensiveCalculation(key);
                }

                return Cache[key];
            });
        }

        private static async Task Main(string[] args)
        {
            Console.WriteLine("....");
            Console.WriteLine(await GetValue(5));
            Console.WriteLine(await GetValue(10));
            Console.WriteLine(await GetValue(5));
            Console.WriteLine(await GetValue(10));
        }
    }
}

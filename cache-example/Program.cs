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
            Console.WriteLine($"Calculating {value}");
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

                Console.WriteLine(Cache[key]);
                return Cache[key];
            });
        }

        private static async Task Main(string[] args)
        {
            Console.WriteLine("....");
            await Task.WhenAll(
                GetValue(5),
                GetValue(10),
                GetValue(5),
                GetValue(10)
            );
        }
    }
}

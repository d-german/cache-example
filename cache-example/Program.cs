using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace cache_example
{
    public class Program
    {
        private static readonly IDictionary<int, int> Cache = new ConcurrentDictionary<int, int>();

        private static Task<int> ExpensiveCalculation(int value)
        {
            return Task.Run(() =>
            {
                Thread.Sleep(2000);
                return value * 50;
            });
        }

        private static async Task<int> GetValue(int key)
        {
            if (!Cache.ContainsKey(key))
            {
                Cache[key] = await ExpensiveCalculation(key);
            }

            return Cache[key];
        }

        private static async Task Main(string[] args)
        {
            var sw = new Stopwatch();
            sw.Start();
            Console.WriteLine("....");
            await Task.WhenAll(
                GetValue(5),
                GetValue(10),
                GetValue(5),
                GetValue(10)
            );

            Console.WriteLine($"{sw.ElapsedMilliseconds} ms");
        }
    }
}

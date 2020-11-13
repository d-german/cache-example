﻿using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace cache_example
{
    public class Program
    {
        private static readonly ConcurrentDictionary<int, int> Cache = new ConcurrentDictionary<int, int>();
        private static readonly SemaphoreSlim CacheLock = new SemaphoreSlim(initialCount: 1, maxCount: 1);


        private static int ExpensiveCalculation(int value)
        {
            Console.WriteLine($"Calculating {value}");
            Thread.Sleep(2000);
            return value * 50;
        }

        private static Task<int> GetValue(int key)
        {
            return Task.Run(async () =>
            {
                await CacheLock.WaitAsync();
                try
                {
                    var result = Cache.GetOrAdd(key, ExpensiveCalculation);
                    Console.WriteLine(result);
                    return result;
                }
                finally
                {
                    CacheLock.Release();
                }
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

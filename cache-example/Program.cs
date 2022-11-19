﻿using System;
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
        private static readonly SemaphoreSlim CacheLock = new(initialCount: 1, maxCount: 1);

        private static int ExpensiveCalculation(int value)
        {
            Console.WriteLine($"Calculating {value}");
            Thread.Sleep(2000);
            return value * 50;
        }

        private static async Task<int> GetValue(int key)
        {
            await CacheLock.WaitAsync();
            try
            {
                if (!Cache.ContainsKey(key))
                {
                    Cache[key] = ExpensiveCalculation(key);
                }

                Console.WriteLine(Cache[key]);
                return Cache[key];
            }
            finally
            {
                CacheLock.Release();
            }
        }

        private static async Task Main(string[] args)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            Console.WriteLine("....");
            await Task.WhenAll(
                GetValue(5),
                GetValue(10),
                GetValue(5),
                GetValue(10)
            );
            stopwatch.Stop();
            Console.WriteLine($"Elapsed: {stopwatch.ElapsedMilliseconds}ms");
        }
    }
}

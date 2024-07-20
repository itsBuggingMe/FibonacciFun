using FibonacciFun.Impl;
using System.Diagnostics;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace FibonacciFun
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("Getting numbers...\n");
            var t = GenerateValidationData();

            Console.WriteLine("Benchmarking...\n");
            var table = GenerateData(t);

            Console.WriteLine("Done!");

            foreach (var (k, v) in table)
            {
                Console.WriteLine($"{k.Name}: {v.Count}");
            }
        }

        private static Dictionary<Type, List<ushort>> GenerateData(BigInteger[] validationTable)
        {
            Stopwatch timing = new Stopwatch();
            object[] parameterList = new object[1];
            Recursive.CalculateFibonacci(0);

            Dictionary<Type, List<ushort>> table = new();

            foreach (var (method, type) in Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => !t.IsInterface)
                .Where(typeof(IFibonacciImplementation).IsAssignableFrom)
                .Select(t => (t.GetMethod("CalculateFibonacci", BindingFlags.Static | BindingFlags.Public)!, t)))
            {
                Console.WriteLine($"\nTesting {type.Name}...");

                int count = 0;

                //jit warmup
                parameterList[0] = 10;
                method.Invoke(null, parameterList);
                List<ushort> times = [];

                while (true)
                {
                    count++;
                    parameterList[0] = count;

                    GC.Collect();
                    timing.Reset();
                    timing.Start();
                    object? result = method.Invoke(null, parameterList);
                    timing.Stop();

                    BigInteger bigint = default;
                    if (result is null || 
                        (bigint = new BigInteger(MemoryMarshal.Cast<ulong, byte>((ulong[])result))) != validationTable[count])
                    {
                        Console.WriteLine($"{type.Name} failed on index {count}\nExpected: {validationTable[count]}\nReceived: {bigint}");
                        break;
                    }

                    times.Add((ushort)timing.ElapsedMilliseconds);

                    if (timing.ElapsedMilliseconds > 1000)
                        break;
                }

                table[type] = times;
            }

            return table;
        }

        private static BigInteger[] GenerateValidationData()
        {
            const string url = @"https://oeis.org/A000045/b000045.txt";

            HttpClient httpClient = new HttpClient();
            string s = httpClient.GetStringAsync(url).Result;

            Console.WriteLine("Parsing table...");
            return s.Split('\n').Skip(1).Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => BigInteger.Parse(s.Split(' ')[1])).ToArray();
        }
    }
}

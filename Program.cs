using FibonacciFun.Impl;
using System.Diagnostics;
using System.Reflection;

namespace FibonacciFun
{
    internal class Program
    {
        static void Main()
        {
            var table = GenerateData();
            foreach (var (k, v) in table)
            {
                Console.WriteLine($"{k.Name}:\t{v.Count - 1}");
            }
        }

        private static Dictionary<Type, List<ushort>> GenerateData()
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

                    times.Add((ushort)timing.ElapsedMilliseconds);

                    if (timing.ElapsedMilliseconds > 1000)
                        break;
                }

                table[type] = times;
            }

            return table;
        }
    }
}

using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FibonacciFun.Impl
{
    internal class Recursive : IFibonacciImplementation
    {
        public static ulong[] CalculateFibonacci(int n)
        {
            return [Calculate(n)];
        }

        private static ulong Calculate(int n)
        {
            if(n < 2)
                return 1;

            return Calculate(n - 2) + Calculate(n - 1);
        }
    }
}

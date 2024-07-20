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
        public static uint[] CalculateFibonacci(int n)
        {
            return [Calculate(n)];
        }

        private static uint Calculate(int n)
        {
            if(n < 3)
                return 1;

            return Calculate(n - 2) + Calculate(n - 1);
        }
    }
}

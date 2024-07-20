using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FibonacciFun.Impl
{
    internal class Linear : IFibonacciImplementation
    {
        public static ulong[] CalculateFibonacci(int n)
        {
            ulong first = 1;
            ulong second = 1;

            for(int i = 0; i < n; i++)
            {
                ulong tmp = first;
                first += second;
                second = tmp;
            }

            return [second];
        }
    }
}

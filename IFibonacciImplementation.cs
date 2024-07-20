using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FibonacciFun
{
    internal interface IFibonacciImplementation
    {
        abstract static uint[] CalculateFibonacci(int n);
    }
}

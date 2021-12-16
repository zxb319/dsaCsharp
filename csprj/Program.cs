using System;
using System.Collections.Generic;
using System.Linq;
using csprj.dsa;

namespace csprj {
class Program {
    static void Main(string[] args) {
        SortedDictionary<string, int> d = new SortedDictionary<string, int>();
        d.Add("r",1);
        d.Add("d",1);
        d.Add("b",1);
        d.Add("c",1);
        d.Add("a",1);

        foreach (var kv in d) {
            Console.Out.WriteLine(kv.Key+"-->"+kv.Value);
        }
    }

    static int fib(int a, int b, int n) {
        if (n == 1) {
            return b;
        }
        else {
            return fib(b, a + b, n - 1);
        }
    }
}
}
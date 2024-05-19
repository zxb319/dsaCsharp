using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using csprj.dsa;
using csprj.dsa.fin;

namespace csprj {
    class Program {

        static void Main(string[] args) {
            var a=BitConverter.GetBytes(1).Reverse();
            foreach(var b in a) {
                Console.WriteLine(b);
            }
        }

        static void test1() {
            var st = DateTime.Now;


            long a = 0;
            for (int i = 0; i < 1e9; i++) {
                a += i;
            }
            Console.WriteLine(a);

            var et = DateTime.Now;
            Console.WriteLine($"{(et - st).TotalSeconds}s");
            Console.WriteLine("*****************");
        }

        static void ttt() {
            var f = File.CreateText(@"d:\aa.txt");
            f.WriteLine("zxb");
            f.Close();
        }

    }
}
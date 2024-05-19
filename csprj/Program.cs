using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Threading;
using csprj.dsa;
using csprj.dsa.fin;
using Newtonsoft.Json.Linq;

namespace csprj {

    class Program {

        static void Main(string[] args) {

            var js = new JArray();
            var o = new JObject();
            o["aa"] = 1;

            js.Add(o);

            Console.WriteLine(js.ToString());
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
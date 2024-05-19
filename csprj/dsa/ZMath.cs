using System;
using System.Collections.Generic;
using System.Text;

namespace csprj.dsa {
    public class ZMath {
        public static double Root(Func<double, double> f, double lo, double hi) {
            var flo = f(lo);
            var fhi = f(hi);
            if (flo * fhi > 0) {
                throw new ArithmeticException("给定区间不包含零点！");
            }
            while (true) {
                var mi = (lo + hi) / 2;
                if (mi == lo || mi == hi) { return mi; }
                var fmi = f(mi);
                if (flo * fmi <= 0) { hi = mi; }
                else {
                    lo = mi;
                }
            }
        }

    }
}

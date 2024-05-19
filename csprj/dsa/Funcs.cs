using System.Collections.Generic;

namespace csprj.dsa {
    public class Funcs {
        public static IEnumerable<(int, T)> Enumerate<T>(IEnumerable<T>elems) {
            int i = 0;
            foreach (var e in elems) {
                yield return (i++, e);
            }
        }
        
        public static IEnumerable<(T1, T2)> Zip<T1,T2>(IEnumerable<T1> elems1, IEnumerable<T2> elems2) {
            var es1 = elems1.GetEnumerator();
            var es2 = elems2.GetEnumerator();
            while (es1.MoveNext() && es2.MoveNext()) {
                yield return (es1.Current, es2.Current);
            }
        }
    }
}
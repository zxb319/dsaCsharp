using System.Collections;
using System.Collections.Generic;

namespace csprj.dsa {
public class FibSeq:IEnumerable<int> {
    
    
    public IEnumerator<int> GetEnumerator() {
        int f0 = 0;
        int f1 = 1;
        while (true) {
            yield return f1;
            f1 += f0;
            f0 = f1 - f0;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }
}
}
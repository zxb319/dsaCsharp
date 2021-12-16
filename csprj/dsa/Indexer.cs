using System.Collections;
using System.Collections.Generic;

namespace csprj.dsa {
public class Indexer<T>:IEnumerable<(int,T)> {
    private IEnumerable<T> elems;
    public Indexer(IEnumerable<T> elems) {
        this.elems = elems;
    }
    
    
    public IEnumerator<(int,T)> GetEnumerator() {
        int index = 0;
        foreach (var e in elems) {
            yield return (index++, e);
        }
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }
}
}
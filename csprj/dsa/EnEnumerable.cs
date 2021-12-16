using System;
using System.Collections;
using System.Collections.Generic;

namespace csprj.dsa {

public interface EnEnumerable<T> : IEnumerable<T> {
    public EnEnumerable<T2> Map<T2>(Func<T, T2> func) {
        return new EnEnumerableMap<T, T2>(this, func);
    }

    public EnEnumerable<T> Filter(Func<T, bool> func) {
        return new EnEnumerableFilter<T>(this, func);
    }
}

public class EnEnumerableStd<T> : EnEnumerable<T> {
    private IEnumerable<T> elems;
    
    public EnEnumerableStd(IEnumerable<T>elems) {
        this.elems = elems;
    }
    public IEnumerator<T> GetEnumerator() {
        foreach (var e in elems) {
            yield return e;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }
}


public class EnEnumerableMap<T,T2> : EnEnumerable<T2> {
    private EnEnumerable<T> elems;
    private Func<T, T2> func;
    
    public EnEnumerableMap(EnEnumerable<T>elems,Func<T,T2>func) {
        this.elems = elems;
        this.func = func;
    }
    public IEnumerator<T2> GetEnumerator() {
        foreach (var e in elems) {
            yield return func(e);
        }
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }
    
}
public class EnEnumerableFilter<T> : EnEnumerable<T> {
    private EnEnumerable<T> elems;
    private Func<T, bool> func;
    
    public EnEnumerableFilter(EnEnumerable<T>elems,Func<T,bool>func) {
        this.elems = elems;
        this.func = func;
    }
    public IEnumerator<T> GetEnumerator() {
        foreach (var e in elems) {
            if(func(e))
                yield return e;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }
}

}
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace csprj.dsa {
public class Heap<T> {
    private Func<T, T, int> cmp;
    private List<T> elems;
    public Heap(Func<T,T,int>cmp,IEnumerable<T>elems=null) {
        this.cmp = cmp;
        if (elems == null)
            this.elems = new List<T>();
        else
            this.elems = new List<T>(elems);
        
    }

    private static int parent(int child) {
        return (child - 1) / 2;
    }

    private static int left(int parent) {
        return parent * 2 + 1;
    }

    private static int right(int parent) {
        return parent * 2 + 2;
    }

    private static void Heapfy(List<T> elems,Func<T, T, int> cmp) {
        for (int i = parent(elems.Count - 1); i >= 0; --i) {
            int l = left(i);
            int r = right(i);
            if (cmp(elems[i], elems[l])>0) {
                (elems[i], elems[l]) = (elems[l], elems[i]);
            }

            if (r < elems.Count && cmp(elems[i], elems[r]) > 0) {
                (elems[i], elems[l]) = (elems[l], elems[i]);
            }
        }
    }
    
    
    
}
}
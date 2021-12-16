using System;
using System.Collections.Generic;
using System.Text;

namespace csprj.dsa {
public class Vector<T>where T:new() {
    private T[] elems;
    public Vector(T[] elems) {
        this.elems = elems;
    }

    public static Vector<T> operator -(Vector<T> v) {
        var res = new Vector<T>(new T[v.elems.Length]);
        for (int i = 0; i < v.elems.Length; ++i) {
            res.elems[i] = -(dynamic)v.elems[i];
        }

        return res;
    }

    public static Vector<T> operator +(Vector<T> a, Vector<T> b) {
        if (a.elems.Length != b.elems.Length) throw new ArgumentException("two Vector dimensions do not match!");
        
        var res = new Vector<T>(new T[a.elems.Length]);
        for (int i = 0; i < a.elems.Length; ++i) {
            res.elems[i] = (dynamic)a.elems[i] + b.elems[i];
        }

        return res;
    }

    public static Vector<T> operator -(Vector<T> a, Vector<T> b) => a + -b;
    
    public static T operator *(Vector<T>a, Vector<T>b) {
        if (a.elems.Length != b.elems.Length) throw new ArgumentException("two Vector dimensions do not match!");
        
        T res=new T();
        for (int i = 0; i < a.elems.Length; ++i) {
            res+= (dynamic)a.elems[i] + b.elems[i];
        }

        return res;
    }

    public T Norm {
        get {
            T res = new T();
            foreach (var e in elems) {
                res += (dynamic) e * e;
            }

            return (T)Math.Sqrt((dynamic)res);
        }
    }

    public Vector<T> Unit {
        get {
            var res = new Vector<T>(new T[elems.Length]);
            var norm = Norm;
            if (norm == (dynamic) new T()) return res;
            
            for (int i = 0; i < elems.Length; ++i) {
                res.elems[i] = (dynamic)elems[i]/norm;
            }

            return res;
        }
    }

    public override string ToString() {
        StringBuilder sb = new StringBuilder();
        sb.Append('[');
        foreach (var e in elems) {
            sb.Append(e);
            sb.Append(',');
        }

        sb[^1] = ']';
        return sb.ToString();
    }
}
}
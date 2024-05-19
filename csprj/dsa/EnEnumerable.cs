using System;
using System.Collections;
using System.Collections.Generic;

namespace csprj.dsa {

    public class EnEnumerable<T> : IEnumerable<T> {
        private IEnumerable<T> elems;
        public EnEnumerable(IEnumerable<T> elems) {
            this.elems = elems;
        }

        public EnEnumerable<R> Map<R>(Func<T, R> func) {
            IEnumerable<R> f(EnEnumerable<T> elems) {
                foreach (var e in elems) {
                    yield return func(e);
                }
            }

            return new EnEnumerable<R>(f(this));
        }

        public EnEnumerable<T> Filter(Func<T, bool> func) {
            IEnumerable<T> f(EnEnumerable<T> elems) {
                foreach (var e in elems) {
                    if (func(e)) {
                        yield return e;
                    }
                }
            }

            return new EnEnumerable<T>(f(this));
        }

        public R Reduce<R>(Func<R, T, R> func, R init) {
            R res = init;
            foreach (var e in elems) {
                res = func(res, e);
            }

            return res;
        }

        public void Foreach(Action<T> action) {
            foreach (var e in this.elems) {
                action(e);
            }
        }

        public IEnumerator<T> GetEnumerator() {
            foreach (var e in this.elems) {
                yield return e;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
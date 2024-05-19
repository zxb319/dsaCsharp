using System;

namespace csprj.dsa {
    public class Fraction : IComparable<Fraction>, IEquatable<Fraction> {
        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Fraction)obj);
        }

        private readonly int _num;
        private readonly int _den;

        public Fraction(int num = 0, int den = 1) {
            if (den == 0) throw new DivideByZeroException("Fraction:den is zero!");

            int g = Gcd(num, den);
            num /= g;
            den /= g;

            if (den < 0) {
                num = -num;
                den = -den;
            }

            this._num = num;
            this._den = den;
        }

        public int CompareTo(Fraction other) {
            return _num * other._den - other._num * _den;
        }

        public bool Equals(Fraction other) {
            return CompareTo(other) == 0;
        }

        public override int GetHashCode() {
            return HashCode.Combine(_num, _den);
        }

        public override string ToString() {
            return _num + "/" + _den;
        }

        public static bool operator <(Fraction a, Fraction b) {
            return a.CompareTo(b) < 0;
        }

        public static bool operator ==(Fraction a, Fraction b) {
            return a.CompareTo(b) == 0;
        }
        public static bool operator !=(Fraction a, Fraction b) {
            return !(a == b);
        }
        public static bool operator <=(Fraction a, Fraction b) {
            return !(a > b);
        }
        public static bool operator >=(Fraction a, Fraction b) {
            return !(a < b);
        }

        public static bool operator >(Fraction a, Fraction b) {
            return b.CompareTo(a) < 0;
        }

        public Fraction Rep => new Fraction(_den, _num);

        public static Fraction operator -(Fraction f) => new Fraction(-f._num, f._den);

        public static Fraction operator +(Fraction a, Fraction b) =>
            new Fraction(a._num * b._den + a._den * b._num, a._den * b._den);

        public static Fraction operator -(Fraction a, Fraction b) => a + -b;

        public static Fraction operator *(Fraction a, Fraction b) => new Fraction(a._num * b._num, a._den * b._den);

        public static Fraction operator /(Fraction a, Fraction b) => a * b.Rep;




        public static int Gcd(int a, int b) {
            if (a == 0) return b;
            return Gcd(b % a, a);
        }
    }
}
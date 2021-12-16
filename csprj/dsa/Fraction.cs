using System;

namespace csprj.dsa {
public class Fraction:IComparable<Fraction>,IEquatable<Fraction> {
    private readonly int num;
    private readonly int den;

    public Fraction(int num=0, int den=1) {
        if (den == 0) throw new DivideByZeroException("Fraction:den is zero!");

        int g = Gcd(num, den);
        num /= g;
        den /= g;

        if (den < 0) {
            num = -num;
            den = -den;
        }

        this.num = num;
        this.den = den;
    }

    public int CompareTo(Fraction other) {
        return num * other.den - other.num * den;
    }

    public bool Equals(Fraction other) {
        return CompareTo(other) == 0;
    }

    public override int GetHashCode() {
        return HashCode.Combine(num, den);
    }

    public override string ToString() {
        return num + "/" + den;
    }

    public static bool operator <(Fraction a, Fraction b) {
        return a.CompareTo(b) < 0;
    }

    public static bool operator >(Fraction a, Fraction b) {
        return b.CompareTo(a) < 0;
    }

    public Fraction Rep => new Fraction(den, num);

    public static Fraction operator -(Fraction f) => new Fraction(-f.num, f.den);

    public static Fraction operator +(Fraction a, Fraction b) =>
        new Fraction(a.num * b.den + a.den * b.num, a.den * b.den);

    public static Fraction operator -(Fraction a, Fraction b) => a + -b;

    public static Fraction operator *(Fraction a, Fraction b) => new Fraction(a.num * b.num, a.den * b.den);

    public static Fraction operator /(Fraction a, Fraction b) => a * b.Rep;
    
    


    public static int Gcd(int a, int b) {
        if (a == 0) return b;
        return Gcd(b % a, a);
    }
}
}
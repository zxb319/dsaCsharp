using System;

namespace csprj.dsa {
    public class Complex {
        private double real;
        private double imag;

        public Complex(double real=0, double imag=0) {
            this.real = real;
            this.imag = imag;
        }

        public bool Equals(Complex other) {
            if (other == null) return false;
            return Math.Abs(real - other.real) < 1e-9 && Math.Abs(imag - other.imag) < 1e-9;
        }

        public override string ToString() {
            if (this.imag < 0)
                return $"{this.real}{this.imag}i";
            else
                return $"{this.real}+{this.imag}i";
        }

        public Complex Conj => new Complex(real, -imag);
        public static Complex operator -(Complex c) => new Complex(-c.real, -c.imag);
        public static Complex operator +(Complex a, Complex b) => new Complex(a.real + b.real, a.imag + b.imag);
        public static Complex operator -(Complex a, Complex b) => a + -b;

        public static Complex operator *(Complex a, Complex b) =>
            new Complex(a.real * b.real - a.imag * b.imag, a.real * b.imag + a.imag * b.real);

        public static Complex operator /(Complex a, Complex b) {
            var c = a * b.Conj;
            var d = (b * b.Conj).real;

            return new Complex(c.real / d, c.imag / d);
        }
    }
}
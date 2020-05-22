using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm
{
    public class Fraction : IComparable
    {
        public readonly int numerator;
        public readonly int denominator;

        public Fraction(int numerator, int denominator)
        {
            if (denominator == 0)
                throw new DivideByZeroException();
            if(denominator < 0 && numerator >=0 || denominator < 0 && numerator <= 0)
            {
                denominator *= -1;
                numerator *= -1;
            }
            int gcd = Gcd(numerator, denominator);
            this.numerator = numerator / gcd;
            this.denominator = denominator / gcd;

        }
        private int Gcd(int a, int b)
        {
            if (b == 0)
                return a;
            return Gcd(b, a % b);
        }
        public int CompareTo(object obj)
        {
            var franc = obj as Fraction;
            var divided = new Fraction(numerator, denominator) - franc;
            if ((double)divided.numerator / divided.denominator < 0)
                return -1;
            if ((double)divided.numerator / divided.denominator > 0)
                return 1;
            return 0;
        }
        public override string ToString()
        {
            if (denominator != 1)
                return String.Format("{0} / {1}", numerator, denominator);
            else if (numerator == 0)
                return "0";
            else
                return String.Format("{0}", numerator);
        }

        public static Fraction operator +(Fraction fraction1, Fraction fraction2)
        {
            return new Fraction(fraction1.numerator * fraction2.denominator + fraction2.numerator * fraction1.denominator, fraction1.denominator * fraction2.denominator);
        }
        public static Fraction operator -(Fraction fraction1, Fraction fraction2)
        {
            return new Fraction(fraction1.numerator * fraction2.denominator - fraction2.numerator * fraction1.denominator, fraction1.denominator * fraction2.denominator);
        }
        public static Fraction operator *(Fraction fraction1, Fraction fraction2)
        {
            return new Fraction(fraction1.numerator * fraction2.numerator, fraction1.denominator * fraction2.denominator);
        }
        public static Fraction operator /(Fraction fraction1, Fraction fraction2)
        {
            return new Fraction(fraction1.numerator * fraction2.denominator, fraction1.denominator * fraction2.numerator);
        }
        public static bool operator >(Fraction fraction1, Fraction fraction2)
        {
            return fraction1.CompareTo(fraction2) > 0;
        }
        public static bool operator <(Fraction fraction1, Fraction fraction2)
        {
            return fraction1.CompareTo(fraction2) < 0;
        }
        public static bool operator ==(Fraction fraction1, Fraction fraction2)
        {
            return fraction1.CompareTo(fraction2) == 0;
        }
        public static bool operator !=(Fraction fraction1, Fraction fraction2)
        {
            return fraction1.CompareTo(fraction2) != 0;
        }
        public static Fraction operator *(Fraction fraction, int number)
        {
            return new Fraction(fraction.numerator * number, fraction.denominator);
        }
        public static Fraction ZeroFraction =  new Fraction(0, 1);
    }
}

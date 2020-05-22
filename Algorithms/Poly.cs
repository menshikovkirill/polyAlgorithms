using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm
{
    public class Poly : IComparable
    {
        readonly Fraction[] coefficients;
        public int Degree { get; }


        public Poly(Fraction[] coefficients)
        {
            this.coefficients = coefficients;
            int deg = coefficients.Length - 1;
            while (coefficients[deg] == Fraction.ZeroFraction && deg > 0)
                deg--;
            Degree = deg;
        }
        /// <summary>
        /// Создаем полином из одного элемента степени deg
        /// </summary>
        /// <param name="coefficient"></param>
        /// <param name="deg"></param>
        public Poly(Fraction coefficient, int deg)
        {
            coefficients = Enumerable.Repeat(Fraction.ZeroFraction, deg + 1).ToArray();
            coefficients[deg] = coefficient;
            Degree = deg;
        }
        public Fraction this[int i]
        {
            get
            {
                return coefficients[i];
            }
            set
            {
                coefficients[i] = value;
            }
        }

        public int CompareTo(object obj)
        {
            var poly = obj as Poly;
            if (poly.Degree == Degree)
            {
                int i = Degree;
                while (i >= 0 && coefficients[i] == poly[i])
                    i--;
                if (i < 0 || coefficients[i] == poly[i])
                    return 0;
                if (coefficients[i] < poly[i])
                    return -1;
                else
                    return 1;
            }
            else if (poly.Degree < Degree)
                return 1;
            else
                return -1;
        }

        public static Poly operator +(Poly poly1, Poly poly2)
        {
            var polyAddition = Enumerable.Repeat(Fraction.ZeroFraction, Math.Max(poly1.Degree, poly2.Degree) + 1).ToArray();
            int degreePoly1 = 0, degreePoly2 = 0;
            int degreePolyAddition = 0;
            while (degreePolyAddition < polyAddition.Length)
            {
                if (degreePoly1 < poly1.Degree + 1)
                {
                    polyAddition[degreePolyAddition] += poly1[degreePoly1];
                    degreePoly1++;
                }
                if (degreePoly2 < poly2.Degree + 1)
                {
                    polyAddition[degreePolyAddition] += poly2[degreePoly2];
                    degreePoly2++;
                }
                degreePolyAddition++;
            }
            return new Poly(polyAddition);
        }

        public static Poly operator -(Poly poly1, Poly poly2)
        {
            var poly2Cofficient = poly2.coefficients.Select(x => x * (-1)).ToArray();
            return poly1 + new Poly(poly2Cofficient);
        }

        public static Poly operator *(Poly poly1, Poly poly2)
        {
            var polyMultiplication = Enumerable.Repeat(Fraction.ZeroFraction, (poly1.Degree + poly2.Degree) + 1).ToArray();
            for (int i = 0; i < poly1.Degree + 1; i++)
            {
                for (int j = 0; j < poly2.Degree + 1; j++)
                {
                    polyMultiplication[i + j] += poly1[i] * poly2[j];
                }
            }
            return new Poly(polyMultiplication);
        }

        /// <summary>Возращает результат деления </summary>
        public static Poly operator /(Poly dividend, Poly divider)
        {
            return LongDivision(dividend, divider).Item1;
        }

        ///<summary> Возращает остаток от деления</summary>
        public static Poly operator %(Poly dividend, Poly divider)
        {
            return LongDivision(dividend, divider).Item2;
        }

        ///<summary>Возращает сначала результат деления, потом остаток от деления</summary>
        static private Tuple<Poly, Poly> LongDivision(Poly dividend, Poly divider)
        {
            if(divider.Degree == 0)
            {
                return new Tuple<Poly, Poly>(new Poly(dividend.coefficients.Select(x => x / divider.coefficients[0]).ToArray()), new Poly(new Fraction[] {Fraction.ZeroFraction }));
             }
            var resultPolyArray = Enumerable.Repeat(Fraction.ZeroFraction, Math.Max(0, dividend.Degree - divider.Degree) + 1).ToArray();
            while (dividend.Degree >= divider.Degree)
            {
                resultPolyArray[dividend.Degree - divider.Degree] = dividend[dividend.Degree] / divider[divider.Degree];
                var tempPoly = new Poly(resultPolyArray[dividend.Degree - divider.Degree], dividend.Degree - divider.Degree);
                dividend = dividend - (tempPoly * divider);
            }
            var resultPoly = new Poly(resultPolyArray);
            return new Tuple<Poly, Poly>(resultPoly, dividend);
        }


        public override string ToString()
        {
            StringBuilder polyString = new StringBuilder();

            for (int i = Degree; i >= 0; i--)
            {
                polyString.Append(String.Format("{0}", coefficients[i], i));
                if (i > 0)
                {
                    polyString.Append(String.Format("x^{0}", i));
                    if ((double)(coefficients[i].numerator / coefficients[i].denominator) >= 0)
                        polyString.Append(" + ");
                }

            }
            return polyString.ToString();
        }

        public static Poly GetGcd(Poly polyA, Poly polyB)
        {
            if (polyB.Degree == 0 && polyB.coefficients[0] == Fraction.ZeroFraction || polyB is null)
                return polyA;
            return GetGcd(polyB, polyA %  polyB);
        }
    }
}

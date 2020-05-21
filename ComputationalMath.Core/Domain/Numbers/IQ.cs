using System;
using System.Collections.Generic;
using System.Text;

namespace ComputationalMath.Core.Domain.Numbers
{
    public static class IQ
    {
        public class RationalNumber : IZ.Integer
        {
            internal RationalNumber(int numerator, int denominator = 1)
            : base(numerator)
            {
                N = numerator;
                D = denominator;
            }

            private int _numerator;
            private int _denominator;

            public bool IsInteger => D == 1 ? true : D == 0 && N == 0 ? true : false;
            public decimal ApproximateValue => (decimal)N / (decimal)D;

            public new int N { get; set; }
            public int D
            {
                get { return _denominator; }
                set
                {
                    if (value != 0)
                    {
                        if (value < 0)
                        {
                            _denominator = -value;
                            _numerator = -_numerator;
                        }
                        else
                        {
                            _denominator = value;
                        }
                    }
                    else if(N != 0)
                    {
                        Console.WriteLine("D cannot be equal to 0.");
                        throw new Exception("D cannot be equal to 0.");
                    }
                }
            }

            public override string ToString()
            {
                return (D != 1 ? N + "/" + D.ToString()
                    : ((decimal)N).ToString(format: "N"));
            }

            #region Binary Operators
            public static RationalNumber operator *(RationalNumber a, RationalNumber b)
            {
                if (a.N == 0 || b.N == 0)
                    return GetZeroDivideZeroRational();

                int numerator = a.N * b.N;
                int denominator = a.D * b.D;

                if (numerator % denominator == 0)
                {
                    numerator = numerator / denominator;
                    denominator = 1;
                }

                return New(numerator, denominator);
            }
            public static RationalNumber operator /(RationalNumber a, RationalNumber b)
            {
                if (a.N == 0 || b.D == 0)
                    return GetZeroDivideZeroRational();

                return New(a.N * b.D, a.D * b.N);
            }
            public static RationalNumber operator +(RationalNumber a, RationalNumber b)
            {
                if (a.N == 0)
                    return b;
                else if (b.N == 0)
                    return a;

                return New((b.D * a.N) + (b.N * a.D)
                    , (a.D * b.D));
            }
            public static RationalNumber operator -(RationalNumber a, RationalNumber b)
            {
                return a + (-b);
            }
            public static RationalNumber operator -(RationalNumber a)
            {
                return New(-a.N, a.D);
            }

            public static bool operator ==(RationalNumber a, RationalNumber b)
            {
                var tempA = New(a.N, a.D);
                var tempB = New(b.N, b.D);
                return tempA.N == tempB.N && tempA.D == tempB.D;
            }
            public static bool operator !=(RationalNumber a, RationalNumber b)
            {
                var tempA = New(a.N, a.D);
                var tempB = New(b.N, b.D);
                return tempA.N != tempB.N || tempA.D != tempB.D;
            }
            //int IComparable<RationalNumber>.CompareTo(RationalNumber other)
            //{
            //    // If other is not a valid object reference, this instance is greater.
            //    if (other == null)
            //        return 1;

            //    // The temperature comparison depends on the comparison of
            //    // the underlying Double values.
            //    return other.N.CompareTo(other.N) && other.D.CompareTo(other.D);
            //}
            public override bool Equals(object obj)
            {
                return Equals(obj as RationalNumber);
            }
            public virtual bool Equals(RationalNumber other)
            {
                if (other == null)
                    return false;

                if (New(other.N, other.D) == New(this.N, this.D))
                    return true;

                return false;
            }
            //public override int GetHashCode()
            //{
            //    if (Equals(N, default(int)) && Equals(D, default(int)))
            //        return base.GetHashCode();
            //    return Value.GetHashCode();
            //}
            #endregion

            #region decimal Integrity
            public static RationalNumber operator *(decimal a, RationalNumber b)
            {
                if (a == 0 || b.N == 0)
                    return GetZeroDivideZeroRational();

                if (a % 1 == 0)
                    return New(b.N * (int)a, b.D);

                return IR.New(a * b.ApproximateValue);
            }
            public static RationalNumber operator* (RationalNumber a, decimal b)
            {
                return b * a;
            }
            public static RationalNumber operator /(decimal a, RationalNumber b)
            {
                if (a == 0)
                    return IR.New(GetZeroDivideZeroRational());

                if (a % 1 == 0)
                    return New(b.D * (int)a, b.D);

                return IR.New(a / b.ApproximateValue);
            }
            public static RationalNumber operator /(RationalNumber a, decimal b)
            {
                if (a.N == 0)
                    return GetZeroDivideZeroRational();

                if (b % 1 == 0)
                    return New(a.N, a.D * (int)b);

                return IR.New(a.ApproximateValue / b);
            }
            public static RationalNumber operator +(decimal a, RationalNumber b)
            {
                if (a == 0)
                    return b;

                if(a % 1 == 0)
                {
                    if (b.N == 0)
                        return New((int)a);

                    return New(((int)a * b.D) + b.N, b.D);
                }
                
                return IR.New(a + b.ApproximateValue);
            }
            public static RationalNumber operator +(RationalNumber a, decimal b)
            {
                return b + a;
            }
            public static RationalNumber operator -(decimal a, RationalNumber b)
            {
                return a + (-b);
            }
            public static RationalNumber operator -(RationalNumber a, decimal b)
            {
                return (-b) + a;
            }
            #endregion

            #region int Integrity
            public static RationalNumber operator *(int a, RationalNumber b)
            {
                if (a == 0 || b.N == 0)
                    return GetZeroDivideZeroRational();

                return New(a * b.N, b.D);
            }
            public static RationalNumber operator *(RationalNumber a, int b)
            {
                return b * a;
            }
            public static RationalNumber operator /(int a, RationalNumber b)
            {
                if (a == 0)
                    return New(0,0);

                return a * New(b.D, b.N);
            }
            public static RationalNumber operator /(RationalNumber a, int b)
            {
                if (a.N == 0)
                    return New(0, 0);

                return New(a.N, a.D * b);
            }
            public static RationalNumber operator +(int a, RationalNumber b)
            {
                if (a == 0)
                    return b;
                else if (b.N == 0)
                    return GetZeroDivideZeroRational();

                return New((a * b.D) + b.N, b.D);
            }
            public static RationalNumber operator +(RationalNumber a, int b)
            {
                return b + a;
            }
            public static RationalNumber operator -(int a, RationalNumber b)
            {
                return a + (-b);
            }
            public static RationalNumber operator -(RationalNumber a, int b)
            {
                return (-b) + a;
            }
            #endregion

            #region Integer Integrity

            public static IQ.RationalNumber operator *(RationalNumber a, IZ.Integer b)
            {
                return IQ.New(a.N * b.N, a.D);
            }

            public static IQ.RationalNumber operator *(IZ.Integer a, IQ.RationalNumber b)
            {
                return IQ.New(a.N * b.N, b.D);
            }

            public static IQ.RationalNumber operator /(IQ.RationalNumber a, IZ.Integer b)
            {
                return IQ.New(a.N, a.D * b.N);
            }

            public static IQ.RationalNumber operator /(IZ.Integer a, IQ.RationalNumber b)
            {
                return IQ.New(a.N * b.D, b.N);
            }

            public static IQ.RationalNumber operator +(IQ.RationalNumber a, IZ.Integer b)
            {
                return IQ.New((b.N * a.D) + a.N, a.D);
            }

            public static IQ.RationalNumber operator +(IZ.Integer a, IQ.RationalNumber b)
            {
                return b + a;
            }

            public static IQ.RationalNumber operator -(IQ.RationalNumber a, IZ.Integer b)
            {
                return a + (-b);
            }

            public static IQ.RationalNumber operator -(IZ.Integer a, IQ.RationalNumber b)
            {
                return (-b) + a;
            }

            #endregion

            #region Conversion Operators

            #endregion
        }

        public static RationalNumber New(int numerator, int denominator = 1)
        {
            if (denominator == 0)
            {
                if (numerator != 0)
                {
                    Console.WriteLine("D cant be equal to 0. X/0");
                    throw new Exception("D cant be equal to 0. X/0");
                }
                else
                    return GetZeroDivideZeroRational();
            }
            else if (denominator <= 0)
            {
                numerator = -numerator;
                denominator = -denominator;
            }

            if (numerator % denominator == 0)
            {
                numerator = numerator / denominator;
                denominator = 1;
            }
            else if (denominator % numerator == 0)
            {
                denominator = denominator / numerator;
                numerator = 1;
            }

            return new RationalNumber(numerator, denominator);
        }

        public static RationalNumber GetZeroDivideZeroRational()
        {
            return new RationalNumber(0, 0);
        }

        public static RationalNumber ToRational(this IR.RealNumber real)
        {
            if (!real.IsRational)
            {
                Console.WriteLine("Irrational real number cannot be converted to rational number.");
                throw new Exception("Irrational real number cannot be converted to rational number.");
            }

            return New(real.N.Value, real.D.Value);
        }

        public static RationalNumber SimplifyRationalNumber(int numerator, int denominator, int gcd)
        {
            return New(numerator / gcd, denominator / gcd);
        }



        //public static bool IsRepeating(RationalNumber r)
        //{
        //    bool isRepeating = false;
        //    int N = r.N;
        //    int D = r.D;
        //    int Q;
        //    int R;
        //    while (true)
        //    {

        //    }

        //    return isRepeating;
        //}
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace ComputationalMath.Core.Domain.Numbers
{
    public static class IZ
    {
        public class Integer 
        {
            internal Integer(int n)
            {
                N = n;
            }

            public int N { get; set; }
            //int IComparable<Integer>.CompareTo(Integer other)
            //{
            //    // If other is not a valid object reference, this instance is greater.
            //    if (other == null)
            //        return 1;

            //    // The temperature comparison depends on the comparison of
            //    // the underlying Double values.
            //    return other.Value.CompareTo(other.Value);
            //}
            public override string ToString()
            {
                return N.ToString("N");
            }

            #region Binary Operators
            
            public static Integer operator *(Integer a, Integer b)
            {
                return New(a.N * b.N);
            }

            public static IQ.RationalNumber operator /(Integer a, Integer b)
            {
                return IQ.New(a.N / b.N);
            }

            public static Integer operator +(Integer a, Integer b)
            {
                return New(a.N + b.N);
            }

            public static Integer operator -(Integer a, Integer b)
            {
                return New(a.N - b.N);
            }

            public static Integer operator -(Integer a)
            {
                return New(-a.N);
            }
            #endregion

            #region int Integrity

            public static Integer operator *(int a, Integer b)
            {
                return New(a * b.N);
            }

            public static Integer operator *(Integer a, int b)
            {
                return b * a;
            }

            public static IQ.RationalNumber operator /(Integer a, int b)
            {
                return IQ.New(a.N / b);
            }

            public static IQ.RationalNumber operator /(int a, Integer b)
            {
                return IQ.New(a / b.N);
            }

            public static Integer operator +(int a, Integer b)
            {
                return New(a + b.N);
            }

            public static Integer operator +(Integer a, int b)
            {
                return b + a;
            }

            public static Integer operator -(int a, Integer b)
            {
                return New(a - b.N);
            }

            public static Integer operator -(Integer a, int b)
            {
                return New(a.N - b);
            }

            #endregion

            #region Decimal Integrity

            public static Integer operator *(decimal a, Integer b)
            {
                if (a % 1 == 0)
                    return New((int)a + b.N);

                return IR.New(a * (decimal)b.N);
            }

            public static Integer operator *(Integer a, decimal b)
            {
                return b * a;
            }

            public static Integer operator /(Integer a, decimal b)
            {
                if (b % 1 == 0)
                    return IQ.New(a.N, (int)b);

                return IR.New(value:a.N / (int) b);
            }

            public static Integer operator /(decimal a, Integer b)
            {
                if (a % 1 == 0)
                    return IQ.New((int)a, b.N);

                return IR.New(value: a / (decimal) b.N);
            }

            public static Integer operator +(decimal a, Integer b)
            {
                if (a == 0)
                    return b;

                return IR.New(a + (decimal)b.N);
            }

            public static Integer operator +(Integer a, decimal b)
            {
                return b + a;
            }

            public static Integer operator -(decimal a, Integer b)
            {
                return a + (-b);
            }

            public static Integer operator -(Integer a, decimal b)
            {
                return a + (-b);
            }
            #endregion

            

            

            #region Conversion Operators

            public static implicit operator Integer(int a) => New(a);
            public static explicit operator Integer(decimal a)
            {
                if (a % 1 == 0)
                    return New((int)a);
                else
                    throw new InvalidOperationException("Floating points cannot convert to Integers.");
            }

            #endregion
        }

        //public static int GetGCD(ulong n1, ulong n2)
        //{
        //    if (n1 == 0)
        //    {
        //        return (int) n2;
        //    }
        //    else if (n2 == 0)
        //    {
        //        return (int)n1;
        //    }
        //    else if (n1 == n2)
        //    {
        //        return (int) n1;
        //    }
        //    else if (n1 > n2)
        //    {
        //        return GetGCD(n1 % n2, n2);
        //    }
        //    else
        //    {
        //        return GetGCD(n1, n2 % n1);
        //    }
        //}

        public static ulong GetGCD(ulong n1, ulong n2)
        {
            if (n1 == 0) return n2;
            if (n2 == 0) return n1;
            if (n1 == n2) return n1;

            bool value1IsEven = (n1 & 1u) == 0;
            bool value2IsEven = (n2 & 1u) == 0;

            if (value1IsEven && value2IsEven)
                return GetGCD(n1 >> 1, n2 >> 1) << 1;
            else if (value1IsEven && !value2IsEven)
                return GetGCD(n1 >> 1, n2);
            else if (value2IsEven)
                return GetGCD(n1, n2 >> 1);
            else if (n1 > n2)
                return GetGCD((n1 - n2) >> 1, n2);
            else
                return GetGCD(n1, (n2 - n1) >> 1);
        }


        // Function to implement Stein's 
        // Algorithm 
        //public static int GetGCD(ulong a, ulong b)
        //{

        //    // GCD(0, b) == b; GCD(a, 0) == a,  
        //    // GCD(0, 0) == 0 
        //    if (a == 0)
        //        return (int)b;
        //    if (b == 0)
        //        return (int)a;

        //    // Finding K, where K is the greatest  
        //    // power of 2 that divides both a and b 
        //    int k;
        //    for (k = 0; ((a | b) & 1) == 0; ++k)
        //    {
        //        a >>= 1;
        //        b >>= 1;
        //    }

        //    // Dividing a by 2 until a becomes odd 
        //    while ((a & 1) == 0)
        //        a >>= 1;

        //    // From here on, 'a' is always odd 
        //    do
        //    {
        //        // If b is even, remove  
        //        // all factor of 2 in b  
        //        while ((b & 1) == 0)
        //            b >>= 1;

        //        /* Now a and b are both odd. Swap  
        //        if necessary so a <= b, then set  
        //        b = b - a (which is even).*/
        //        if (a > b)
        //        {

        //            // Swap u and v. 
        //            ulong temp = a;
        //            a = b;
        //            b = temp;
        //        }

        //        b = (b - a);
        //    } while (b != 0);

        //    /* restore common factors of 2 */
        //    return (int)(a << k);
        //}
        //public static int GetGCD(uint u, uint v)
        //{
        //    // simple cases (termination)
        //    if (u == v)
        //        return u;

        //    if (u == 0)
        //        return v;

        //    if (v == 0)
        //        return u;

        //    // look for factors of 2
        //    if (~u & 1) // u is even
        //        if (v & 1) // v is odd
        //            return gcd(u >> 1, v);
        //        else // both u and v are even
        //            return gcd(u >> 1, v >> 1) << 1;

        //    if (~v & 1) // u is odd, v is even
        //        return gcd(u, v >> 1);

        //    // reduce larger argument
        //    if (u > v)
        //        return gcd((u - v) >> 1, v);

        //    return gcd((v - u) >> 1, u);
        //}

        

        public static Integer New(int n)
        {
            return new Integer(n);
        }
    }
}

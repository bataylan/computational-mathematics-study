using ComputationalMath.Core.Domain.Sets;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ComputationalMath.Core.Domain.Numbers
{
    public static class IR
    {
        public class RealNumber : IQ.RationalNumber, IEquatable<RealNumber>
        {
            internal RealNumber(decimal value)
                : base(0, 0)
            {
                Value = value;
            }
            internal RealNumber(int numerator, int denominator)
                : base(numerator, denominator)
            {
                N = numerator;
                D = denominator;
            }
            
            //public RealNumber(string name)
            //    :base(0,0)
            //{
            //    IsConstant = false;
            //    VariableName = name;
            //}
            //private RealNumber _multiplierPart;

            //private RealNumber _dividerPart;

            //private RealNumber _sumPart;

            //private IQ.RationalNumber _powerPart;

            //private string _expression = "{0}{1}{2}{3}";

            //private string _variableName;
            //public string VariableName
            //{
            //    get { return _variableName; }
            //    set
            //    {
            //        if (!IsConstant)
            //            _variableName = value;
            //        else
            //            throw new Exception("Variable name cannot be set a real number.");
            //    }
            //}

            private int? _numerator;
            private int? _denominator;
            public new int? N { get; set; }
            public new int? D
            {
                get { return _denominator; }
                set
                {
                    if (value.HasValue)
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
                        else if (N != 0)
                        {
                            Console.WriteLine("Denominator cannot be equal to 0 when numerator is not equal to 0.");
                            throw new Exception("Denominator cannot be equal to 0 when numerator is not equal to 0.");
                        }
                        else
                        {
                            _denominator = value;
                        }
                    }
                    else if (N.HasValue)
                    {
                        Console.WriteLine("Null denominator cannot assign when numerator has value.");
                        throw new Exception("Null denominator cannot assign when numerator has value");
                    }

                }
            }

            //private bool _isRational;
            public new bool IsInteger => Value.HasValue ? false : D == 1 ? true : D == 0 && N == 0 ? true : false;
            public bool IsRational => N.HasValue && D.HasValue ? true : false;

            //public bool IsConstant { get; set; } 

            private decimal? _value;
            public decimal? Value
            {
                get
                {
                    if (IsRational)
                        return null;
                    else
                        return _value.Value;
                }
                set
                {
                    if (!IsRational)
                        _value = value;
                }
            }
            //public static decimal ToDecimal(RealNumber a)
            //{
            //    return a.Value;
            //}

            public static bool operator <(RealNumber a, RealNumber b)
            {
                if (a.IsRational || b.IsRational)
                    return a.ApproximateValue < b.ApproximateValue;

                return a.Value < b.Value;
            }
            public static bool operator >(RealNumber a, RealNumber b)
            {
                if (a.IsRational || b.IsRational)
                    return a.ApproximateValue > b.ApproximateValue;

                return a.Value > b.Value;
            }
            public static bool operator <=(RealNumber a, RealNumber b)
            {
                return !(a > b);
            }
            public static bool operator >=(RealNumber a, RealNumber b)
            {
                return !(a < b);
            }
            public static bool operator ==(RealNumber a, RealNumber b)
            {
                if (a.IsRational)
                {
                    if (b.IsRational)
                        return a.N == b.N && a.D == b.D;
                    return false;
                }
                else if (b.IsRational)
                    return false;

                return a.Value == b.Value;
            }
            public static bool operator !=(RealNumber a, RealNumber b)
            {
                return !(a == b);
            }

            //int IComparable<RealNumber>.CompareTo(RealNumber other)
            //{
            //    // If other is not a valid object reference, this instance is greater.
            //    if (other == null)
            //        return 1;

            //    // The temperature comparison depends on the comparison of
            //    // the underlying Double values.
            //    return other.Value.CompareTo(other.Value);
            //}

            public override bool Equals(object obj)
            {
                return Equals(obj as RealNumber);
            }
            public virtual bool Equals(RealNumber other)
            {
                if (other == null)
                    return false;

                if (other.Value == this.Value)
                    return true;

                return false;
            }
            public override int GetHashCode()
            {
                if (Equals(Value, default(decimal)))
                    return base.GetHashCode();
                return Value.GetHashCode();
            }
            public override string ToString()
            {
                if (IsRational)
                {
                    if (IsInteger)
                        return N.Value.ToString("N");

                    return (D != 1 ? N + "/" + D.ToString()
                    : ((decimal)N).ToString(format: "N"));
                }
                return Value.Value.ToString("N");
            }

            //public string ToString(string format)
            //{
            //    return this.ToString(format, CultureInfo.CurrentCulture);
            //}

            //public string ToString(string format, IFormatProvider provider)
            //{
            //    if (String.IsNullOrEmpty(format)) format = "N";
            //    if (provider == null) provider = CultureInfo.CurrentCulture;

            //    return Value.ToString(format, provider);
            //}

            //public RealNumber ConvertToRealNumber(RealNumber number)
            //{
            //    return number;
            //}

            #region Binary Operation Operators
            public static RealNumber operator *(RealNumber a, RealNumber b)
            {
                if (a.IsRational)
                {
                    if (b.IsRational)
                    {
                        var test = New(a.ToRational() * b.ToRational());
                        var tbool = test.IsRational;
                        var value = test.Value;
                        return New(a.ToRational() * b.ToRational());
                    }

                    return a.ToRational() * b;
                }
                else if (b.IsRational)
                {
                    return a * b.ToRational();
                }

                return New(a.Value.Value * b.Value.Value);
            }
            public static RealNumber operator /(RealNumber a, RealNumber b)
            {
                if (a.IsRational)
                {
                    if (b.IsRational)
                        return New(a.ToRational() / b.ToRational());
                    return a.ToRational() / b;
                }
                else if (b.IsRational)
                {
                    return a / b.ToRational();
                }

                return New(a.Value.Value / b.Value.Value);
            }
            public static RealNumber operator +(RealNumber a, RealNumber b)
            {
                if (a.IsRational)
                {
                    if (b.IsRational)
                        return New(a.ToRational() + b.ToRational());
                    return a.ToRational() + b;
                }
                else if (b.IsRational)
                {
                    return a + b.ToRational();
                }

                return New(a.Value.Value + b.Value.Value);
            }
            public static RealNumber operator -(RealNumber a, RealNumber b)
            {
                return a + (-b);
            }
            public static RealNumber operator -(RealNumber a)
            {
                if (a.IsRational)
                {
                    return New(IQ.New(-a.N.Value, a.D.Value));
                }

                return New(-a.Value.Value);
            }
            #endregion

            #region int Integrity
            public static RealNumber operator *(int a, RealNumber b)
            {
                if (b.IsRational)
                    return New(a * b.ToRational());

                return New((decimal)a * b.Value.Value);
            }
            public static RealNumber operator *(RealNumber a, int b)
            {
                return b * a;
            }
            public static RealNumber operator /(RealNumber a, int b)
            {
                if (a.IsRational)
                    return New(a.ToRational() / b);

                return New(a.Value.Value / (decimal)b);
            }
            public static RealNumber operator /(int a, RealNumber b)
            {
                if (b.IsRational)
                    return New(a / b.ToRational());

                return New((decimal)a / b.Value.Value);
            }
            public static RealNumber operator +(int a, RealNumber b)
            {
                if (b.IsRational)
                    return New(a + b.ToRational());

                return New((decimal)a + b.Value.Value);
            }
            public static RealNumber operator +(RealNumber a, int b)
            {
                return b + a;
            }
            public static RealNumber operator -(int a, RealNumber b)
            {
                return a + (-b);
            }
            public static RealNumber operator -(RealNumber a, int b)
            {
                return (-b) + a;
            }
            #endregion

            #region Decimal Integrity
            public static RealNumber operator *(decimal a, RealNumber b)
            {
                if (b.IsRational)
                    return New(a * b.ToRational());

                return New((decimal)a * b.Value.Value);
            }
            public static RealNumber operator *(RealNumber a, decimal b)
            {
                return b * a;
            }
            public static RealNumber operator /(RealNumber a, decimal b)
            {
                if (a.IsRational)
                    return New(a.ToRational() / b);

                return New(value: a.Value.Value / b);
            }
            public static RealNumber operator /(decimal a, RealNumber b)
            {
                if (b.IsRational)
                    return New(a / b.ToRational());

                return New(a / b.Value.Value);
            }
            public static RealNumber operator +(decimal a, RealNumber b)
            {
                if (b.IsRational)
                    return New(a + b.ToRational());

                return New((decimal)a + b.Value.Value);
            }
            public static RealNumber operator +(RealNumber a, decimal b)
            {
                return b + a;
            }
            public static RealNumber operator -(decimal a, RealNumber b)
            {
                return a + (-b);
            }
            public static RealNumber operator -(RealNumber a, decimal b)
            {
                return (-b) + a;
            }
            #endregion

            #region Rational Number Integrity
            public static RealNumber operator *(RealNumber a, IQ.RationalNumber b)
            {
                if (a.IsRational)
                {
                    if (a.N == 0 || b.N == 0)
                        return New(IQ.GetZeroDivideZeroRational());
                    return New(a.ToRational() * b);
                }

                return New(value: a.Value.Value * b.ApproximateValue);
            }
            public static RealNumber operator *(IQ.RationalNumber a, RealNumber b)
            {
                return b * a;
            }
            public static RealNumber operator /(RealNumber a, IQ.RationalNumber b)
            {
                if (a.IsRational)
                {
                    if (a.N == 0)
                        return New(IQ.GetZeroDivideZeroRational());

                    return New(a.ToRational() / b);
                }

                if (a.Value.Value == 0)
                    return New(value: 0);

                return New(value: a.Value.Value / b.ApproximateValue);

            }
            public static IR.RealNumber operator /(IQ.RationalNumber a, IR.RealNumber b)
            {
                if (b.IsRational)
                {
                    if (a.N == 0)
                        return IR.New(IQ.GetZeroDivideZeroRational());

                    return IR.New(a / b.ToRational());
                }

                if (a.N == 0)
                    return IR.New(value: 0);

                return IR.New(value: a.ApproximateValue / b.Value.Value);
            }
            public static IR.RealNumber operator +(IR.RealNumber a, IQ.RationalNumber b)
            {
                if (a.IsRational)
                {
                    if (a.N == 0)
                        return IR.New(b);
                    else if (b.N == 0)
                        return a;

                    return IR.New(a.ToRational() + b);
                }

                if (a.Value.Value == 0)
                    return IR.New(b);
                else if (b.N == 0)
                    return a;

                return IR.New(a.Value.Value * b.ApproximateValue);
            }
            public static IR.RealNumber operator +(IQ.RationalNumber a, IR.RealNumber b)
            {
                return b + a;
            }
            public static IR.RealNumber operator -(IR.RealNumber a, IQ.RationalNumber b)
            {
                return a + (-b);
            }
            public static IR.RealNumber operator -(IQ.RationalNumber a, IR.RealNumber b)
            {
                return (-b) + a;
            }
            #endregion

            #region Integer Integrity
            public static RealNumber operator *(IR.RealNumber a, IZ.Integer b)
            {
                if (a.IsRational)
                    return New(a.ToRational() * b);

                return IR.New(a.Value.Value * (decimal)b.N);
            }
            public static RealNumber operator *(IZ.Integer a, IR.RealNumber b)
            {
                return b * a;
            }
            public static RealNumber operator /(IR.RealNumber a, IZ.Integer b)
            {
                if (a.IsRational)
                {
                    return New(a.ToRational() / b);
                }
                return IR.New(a.Value.Value / b.N);
            }
            public static RealNumber operator /(IZ.Integer a, IR.RealNumber b)
            {
                if (b.IsRational)
                {
                    return New(a / b.ToRational());
                }

                return IR.New((decimal)a.N / b.Value.Value);
            }
            public static RealNumber operator +(IR.RealNumber a, IZ.Integer b)
            {
                if (a.N == 0)
                    return New(b);
                else if (b.N == 0)
                    return a;

                if (a.IsRational)
                {
                    return New(a.ToRational() + b);
                }

                return IR.New(a.Value.Value + (decimal)b.N);
            }
            public static RealNumber operator +(IZ.Integer a, IR.RealNumber b)
            {
                return b + a;
            }
            public static RealNumber operator -(IR.RealNumber a, IZ.Integer b)
            {
                return a + (-b);
            }
            public static RealNumber operator -(IZ.Integer a, IR.RealNumber b)
            {
                return (-b) + a;
            }
            #endregion

            #region Conversion Operators

            public static implicit operator RealNumber(int a) => New(a);
            public static implicit operator RealNumber(decimal a) => New(a);
            public static implicit operator decimal(RealNumber a) => a.Value.Value;
            //public static implicit operator RealNumber(double a) => New((decimal)a);
            //public static explicit operator RealNumber(int a) => New(a);

            #endregion
        }

        public static RealNumber New(IZ.Integer n)
        {
            return new RealNumber(numerator: n.N, denominator: 1);
        }

        public static RealNumber New(int n)
        {
            return New(IZ.New(n));
        }

        public static RealNumber New(IQ.RationalNumber r)
        {
            return new RealNumber(numerator: r.N, denominator: r.D);
        }

        public static RealNumber New(decimal value)
        {
            if (value % 1 == 0)
                return New((int)value);

            return new RealNumber(value: value);
        }

        public static int DecimalPlacesCount(decimal num)
        {
            return BitConverter.GetBytes(decimal.GetBits(num)[3])[2];
        }

        public static bool TryConvertToRationalNumber(decimal num, out IQ.RationalNumber rational)
        {
            rational = IQ.New(0, 0);

            int wholePart = (int)decimal.Truncate(num);
            decimal decimalPlaces = num - (decimal)wholePart;

            var numString = decimalPlaces.ToString();
            var totalLength = numString.Length;
            var placesCount = DecimalPlacesCount(decimalPlaces);
            if (placesCount == 28)
            {
                numString = numString.Remove(numString.Length - 1) + (int.Parse(numString.Substring(numString.Length - 1, 1)) - 1);
                decimalPlaces = decimal.Parse(numString);
            }

            bool repeatingConfirmed = false;
            bool isNegative = decimalPlaces < 0;

            string first = "";
            int firstIndex = 0;
            string second = "";
            int secondIndex = 0;
            int repeatingPartLength = 0;
            int nonPlacesLength = numString.Length - placesCount;
            bool isEqualFirstTime = false;
            int repeatCount = 0;
            bool isRepeating = true;

            for (int i = 0; i < (decimal)placesCount / 2; i++)
            {
                repeatCount = 0;
                isRepeating = true;
                isEqualFirstTime = false;
                repeatingPartLength = i + 1;

                for (int j = 0; (isEqualFirstTime ? (j + 2) * repeatingPartLength : j + (2 * repeatingPartLength)) <= placesCount; j++)
                {
                    if (isEqualFirstTime)
                    {
                        secondIndex = secondIndex + repeatingPartLength;
                    }
                    else
                    {
                        firstIndex = nonPlacesLength + j;
                        secondIndex = firstIndex + repeatingPartLength;
                    }

                    first = numString.Substring(firstIndex, repeatingPartLength);
                    second = numString.Substring(secondIndex, repeatingPartLength);
                    if (string.Equals(first, second))
                    {
                        if (!isEqualFirstTime && firstIndex < placesCount)
                            isEqualFirstTime = true;
                        else
                        {
                            repeatCount++;
                            if (repeatCount > 2)
                                break;
                        }

                    }
                    else if (isEqualFirstTime)
                    {
                        isEqualFirstTime = false;
                        isRepeating = false;
                    }
                }

                if (isEqualFirstTime && isRepeating)
                {
                    repeatingConfirmed = true;
                    break;
                }
            }
            if (!repeatingConfirmed)
            {
                //Try to find repeating part don't fit in decimal places
                repeatCount = 0;
                isRepeating = true;
                isEqualFirstTime = false;
                int lastIndexOfOccurance = 0;
                for (int i = 0; i < (decimal)placesCount / 2; i++)
                {

                    firstIndex = totalLength;
                    repeatingPartLength = i + 1;

                    first = numString.Substring(firstIndex - repeatingPartLength, repeatingPartLength);
                    var indexOfOccurance = numString.IndexOf(first);

                    if (indexOfOccurance > 0 && indexOfOccurance != (firstIndex - repeatingPartLength))
                    {
                        lastIndexOfOccurance = indexOfOccurance;
                        if (!isEqualFirstTime)
                            isEqualFirstTime = true;
                        else
                        {
                            repeatCount++;
                        }

                    }
                    else if (i == 0)
                    {
                        isRepeating = false;
                        break;
                    }
                    else if (isEqualFirstTime)
                    {
                        var repeatingPart = numString.Substring(lastIndexOfOccurance, (firstIndex - repeatingPartLength + 1) - lastIndexOfOccurance);
                        firstIndex = lastIndexOfOccurance;
                        repeatingPartLength = repeatingPart.Length;
                        repeatingConfirmed = true;
                        break;
                    }
                }
            }


            if (repeatingConfirmed)
            {
                //first index = non repeating part
                //get int with Math.Truncate(decimal)
                int placesIndex = firstIndex - nonPlacesLength;
                //ulong firstMultiplier = (ulong)Math.Pow((double)10, (double)placesIndex + (double)repeatingPartLength);
                decimal firstNum = decimalPlaces;
                decimal firstMultiplier = 1;
                for (int i = 0; i < repeatingPartLength + placesIndex; i++)
                {
                    firstNum *= 10;
                    firstMultiplier *= 10;
                }
                //ulong secondMultiplier = (ulong)Math.Pow((double)10, (double)placesIndex);
                //decimal secondNum = secondMultiplier * decimalPlaces;

                decimal secondMultiplier = 1;
                for (int i = 0; i < repeatingPartLength; i++)
                {
                    secondMultiplier *= 10;
                }
                decimal remaininMultiplier = firstMultiplier - secondMultiplier;

                if (remaininMultiplier == 0)
                {
                    return false;
                }
                decimal remainingNum = (decimal.Truncate(firstNum));
                decimal gcd = GetGCD(remainingNum, remaininMultiplier);
                int numerator = remainingNum / gcd >= (decimal)int.MaxValue ? 0 : (int)(remainingNum / gcd);
                if (numerator == 0)
                    return false;

                if (isNegative)
                    numerator = -numerator;
                int denominator = remaininMultiplier / gcd >= (decimal)int.MaxValue ? 0 : (int)(remaininMultiplier / gcd);
                rational = IQ.New(numerator, denominator);
                return true;
            }
            else
            {
                decimal firstMultiplier = (ulong)Math.Pow((double)10, (double)placesCount);
                decimal firstNum = (ulong)((decimal)firstMultiplier * decimalPlaces);
                decimal gcd = GetGCD(firstNum, firstMultiplier);
                if (gcd > 0)
                {
                    var simplifiedFirstNum = (int)(firstNum / gcd);
                    var simplifiedFirstMultiplier = (int)(firstMultiplier / gcd);
                    rational = IQ.New((isNegative ? -simplifiedFirstNum : simplifiedFirstNum), simplifiedFirstMultiplier);
                    return true;
                }
                else
                    return false;
            }
        }

        public static List<int> GeneratePrimesNaive(int n)
        {
            List<int> primes = new List<int>();
            if (n > 2)
                primes.Add(2);

            int nextPrime = 3;
            while (primes.Count < n)
            {
                int sqrt = (int)Math.Sqrt(nextPrime);
                bool isPrime = true;
                for (int i = 0; (int)primes[i] <= sqrt; i++)
                {
                    if (nextPrime % primes[i] == 0)
                    {
                        isPrime = false;
                        break;
                    }
                }
                if (isPrime)
                {
                    primes.Add(nextPrime);
                }
                nextPrime += 2;
            }
            return primes;
        }

        public static List<int> FindDivisors(decimal a)
        {
            var divisors = new List<int>();
            var primes = GeneratePrimesNaive(1000);
            for (int i = 0; i <= a && i < primes.Count; i++)
            {
                if (a % primes[i] == 0)
                {
                    divisors.Add(primes[i]);
                    a = a / primes[i];
                    i = 0;
                }
            }
            return divisors;
        }

        public static decimal GetGCDTwo(decimal a, decimal b)
        {
            var divisorsOfA = FindDivisors(a);
            var divisorsOfB = FindDivisors(b);
            var commonDivisors = divisorsOfA.Intersect(divisorsOfB).ToList();

            decimal gcd = 1;
            if (commonDivisors != null & commonDivisors.Count() > 0)
            {
                for (int i = 0; i < commonDivisors.Count(); i++)
                {
                    gcd *= commonDivisors[i];
                }
            }

            return gcd;
        }

        public static decimal GetGCD(decimal a, decimal b)
        {
            decimal n1 = decimal.Truncate(a);
            decimal n2 = decimal.Truncate(b);
            if (n1 == 0) return n2;
            if (n2 == 0) return n1;
            if (n1 == n2) return n1;

            bool value1IsEven = (n1 % 2) == 0;
            bool value2IsEven = (n2 % 2) == 0;

            if (value1IsEven && value2IsEven)
                return GetGCD(n1 / 2, n2 / 2) * 2;
            else if (value1IsEven && !value2IsEven)
                return GetGCD(n1 / 2, n2);
            else if (value2IsEven)
                return GetGCD(n1, n2 / 2);
            else if (n1 > n2)
                return GetGCD((n1 - n2) / 2, n2);
            else
                return GetGCD(n1, (n2 - n1) / 2);
        }

        //public static bool TryConverDecimalToRationalNumber(double num)
        //{
        //    fraction(num);
        //    return true;
        //}


        //public static void fraction(double x)
        //{
        //    String a = x.ToString();
        //    var spilts = a.Split("."); // split using decimal
        //    int b = spilts[1].Length; // find the decimal length
        //    int denominator = (int)Math.Pow(10, b); // calculate the denominator
        //    int numerator = (int)(x * denominator); // calculate the nerumrator Ex
        //                                            // 1.2*10 = 12
        //    int gcd = getGCD(numerator, denominator); // Find the greatest common
        //                                              // divisor bw them
        //    String fraction = "" + numerator / gcd + "/" + denominator / gcd;
        //    int y = 5;
        //    //System.out.println(fraction);
        //}

        /// <summary>
        /// Checks for number of occurence of specified no in a number
        /// </summary>
        /// <param name="s">The no to check occurence times</param>
        /// <param name="check">The number where to check this</param>
        /// <returns>Index</returns>
        private static int Occurence(string s, string check)
        {
            int i = 0;
            int d = s.Length;
            string ds = check;
            for (int n = (ds.Length / d); n > 0; n--)
            {
                if (ds.Contains(s))
                {
                    i++;
                    ds = ds.Remove(ds.IndexOf(s), d);
                }
            }
            return i;
        }


        //#region Binary Operation Operators
        //public static RealNumber Multiply(this RealNumber a, RealNumber b)
        //{
        //    return New(a.Value * b.Value);
        //}
        //public static RealNumber Divide(this RealNumber a, RealNumber b)
        //{
        //    return New(a.Value / b.Value);
        //}
        //public static RealNumber Add(this RealNumber a, RealNumber b)
        //{
        //    return New(a.Value + b.Value);
        //}
        //public static RealNumber Substract(this RealNumber a, RealNumber b)
        //{
        //    return New(a.Value - b.Value);
        //}
        //public static RealNumber Negative(RealNumber a)
        //{
        //    return New(-a.Value);
        //}
        //#endregion

        //#region int Integrity
        //public static RealNumber Multiply(this int a, RealNumber b)
        //{
        //    return New((decimal)a * b.Value);
        //}
        //public static RealNumber Multiply(this RealNumber a, int b)
        //{
        //    return b * a;
        //}
        //public static RealNumber Divide(this RealNumber a, int b)
        //{
        //    return New(a.Value / (decimal)b);
        //}
        //public static RealNumber Divide(this int a, RealNumber b)
        //{
        //    return New((decimal)a / b.Value);
        //}
        //public static RealNumber Add(this int a, RealNumber b)
        //{
        //    return New((decimal)a + b.Value);
        //}
        //public static RealNumber Add(this RealNumber a, int b)
        //{
        //    return b + a;
        //}
        //public static RealNumber Substract(this int a, RealNumber b)
        //{
        //    return New((decimal)a - b.Value);
        //}
        //public static RealNumber Substract(this RealNumber a, int b)
        //{
        //    return New(a.Value - (decimal)b);
        //}
        //#endregion

        //#region Decimal Integrity
        //public static RealNumber Multiply(this decimal a, RealNumber b)
        //{
        //    return New((decimal)a * b.Value);
        //}
        //public static RealNumber Multiply(this RealNumber a, decimal b)
        //{
        //    return b * a;
        //}
        //public static RealNumber Divide(this RealNumber a, decimal b)
        //{
        //    return New(a.Value, (decimal)b);
        //}
        //public static RealNumber Divide(this decimal a, RealNumber b)
        //{
        //    return New((decimal)a, b.Value);
        //}
        //public static RealNumber Add(this decimal a, RealNumber b)
        //{
        //    return New((decimal)a + b.Value);
        //}
        //public static RealNumber Add(this RealNumber a, decimal b)
        //{
        //    return b + a;
        //}
        //public static RealNumber Substract(this decimal a, RealNumber b)
        //{
        //    return IR.New((decimal)a - b.Value);
        //}
        //public static RealNumber Substract(this RealNumber a, decimal b)
        //{
        //    return IR.New(a.Value - (decimal)b);
        //}
        //#endregion

        //#region Binary Operators

        //public static IZ.Integer Multiply(this IZ.Integer a, IZ.Integer b)
        //{
        //    return IZ.New((int)a.Value * (int)b.Value);
        //}

        //public static IQ.RationalNumber Divide(this IZ.Integer a, IZ.Integer b)
        //{
        //    return IQ.New((int)a.Value / (int)b.Value);
        //}

        //public static IZ.Integer Add(this IZ.Integer a, IZ.Integer b)
        //{
        //    return IZ.New((int)a.Value + (int)b.Value);
        //}

        //public static IZ.Integer Subtract(IZ.Integer a, IZ.Integer b)
        //{
        //    return IZ.New((int)a.Value - (int)b.Value);
        //}
        //#endregion

        //#region int Integrity

        //public static IZ.Integer Multiply(int a, IZ.Integer b)
        //{
        //    return IZ.New(a * b.N);
        //}

        //public static IZ.Integer Multiply(this IZ.Integer a, int b)
        //{
        //    return b * a;
        //}

        //public static IQ.RationalNumber Divide(this IZ.Integer a, int b)
        //{
        //    return IQ.New(a.N / b);
        //}

        //public static IQ.RationalNumber Divide(this int a, IZ.Integer b)
        //{
        //    return IQ.New(a / b.N);
        //}

        //public static IZ.Integer Add(this int a, IZ.Integer b)
        //{
        //    return IZ.New(a + b.N);
        //}

        //public static IZ.Integer Add(this IZ.Integer a, int b)
        //{
        //    return b + a;
        //}

        //public static IZ.Integer Substract(this int a, IZ.Integer b)
        //{
        //    return IZ.New(a - b.N);
        //}

        //public static IZ.Integer Substract(this IZ.Integer a, int b)
        //{
        //    return IZ.New(a.N - b);
        //}

        //#endregion

        //#region Decimal Integrity

        //public static IR.RealNumber Multiply(this decimal a, IZ.Integer b)
        //{
        //    return IR.New(a * b.N);
        //}

        //public static IR.RealNumber Multiply(this IZ.Integer a, decimal b)
        //{
        //    return b * a;
        //}

        //public static IR.RealNumber Divide(this IZ.Integer a, decimal b)
        //{
        //    return IR.New(a.N, b);
        //}

        //public static IR.RealNumber Divide(this decimal a, IZ.Integer b)
        //{
        //    return IR.New(a, b.N);
        //}

        //public static IR.RealNumber Add(this decimal a, IZ.Integer b)
        //{
        //    return IR.New(a + b.N);
        //}

        //public static IR.RealNumber Add(this IZ.Integer a, decimal b)
        //{
        //    return b + a;
        //}

        //public static IR.RealNumber Substract(this decimal a, IZ.Integer b)
        //{
        //    return a + (-b);
        //}

        //public static IR.RealNumber Substract(this IZ.Integer a, decimal b)
        //{
        //    return a + (-b);
        //}
        //#endregion

        //#region Rational Number Integrity

        //public static IQ.RationalNumber Multiply(this IQ.RationalNumber a, IZ.Integer b)
        //{
        //    return IQ.New(a.Numerator * b.N, a.Denominator);
        //}

        //public static IQ.RationalNumber Multiply(this IZ.Integer a, IQ.RationalNumber b)
        //{
        //    return IQ.New(a.N * b.Numerator, b.Denominator);
        //}

        //public static IQ.RationalNumber Divide(this IQ.RationalNumber a, IZ.Integer b)
        //{
        //    return IQ.New(a.Numerator, a.Denominator * b.N);
        //}

        //public static IQ.RationalNumber Divide(this IZ.Integer a, IQ.RationalNumber b)
        //{
        //    return IQ.New(a.N * b.Denominator, b.Numerator);
        //}

        //public static IQ.RationalNumber Add(this IQ.RationalNumber a, IZ.Integer b)
        //{
        //    return IQ.New((b.N * a.Denominator) + a.Numerator, a.Denominator);
        //}

        //public static IQ.RationalNumber Add(this IZ.Integer a, IQ.RationalNumber b)
        //{
        //    return b + a;
        //}

        //public static IQ.RationalNumber Substract(this IQ.RationalNumber a, IZ.Integer b)
        //{
        //    return a + (-b);
        //}

        //public static IQ.RationalNumber Substract(this IZ.Integer a, IQ.RationalNumber b)
        //{
        //    return (-b) + a;
        //}

        //#endregion

        //#region Real Number Integrity

        //public static IR.RealNumber Multiply(this IR.RealNumber a, IZ.Integer b)
        //{
        //    return IR.New(a.Value * b.N);
        //}

        //public static IR.RealNumber Multiply(this IZ.Integer a, IR.RealNumber b)
        //{
        //    return b * a;
        //}

        //public static IR.RealNumber Divide(this IR.RealNumber a, IZ.Integer b)
        //{
        //    return IR.New(a.Value / b.N);
        //}

        //public static IR.RealNumber Divide(this IZ.Integer a, IR.RealNumber b)
        //{
        //    return IR.New(a.N / b.Value);
        //}

        //public static IR.RealNumber Add(this IR.RealNumber a, IZ.Integer b)
        //{
        //    return IR.New(a.Value + b.N);
        //}

        //public static IR.RealNumber Add(this IZ.Integer a, IR.RealNumber b)
        //{
        //    return b + a;
        //}

        //public static IR.RealNumber Substract(this IR.RealNumber a, IZ.Integer b)
        //{
        //    return a + (-b);
        //}

        //public static IR.RealNumber Substract(this IZ.Integer a, IR.RealNumber b)
        //{
        //    return (-b) + a;
        //}

        //#region Real Number Integrity
        //public static IR.RealNumber Multiply(this IR.RealNumber a, IQ.RationalNumber b)
        //{
        //    if (a.Value == 0 || b.Numerator == 0)
        //        return IZ.New(0);

        //    return IR.New(a.Value * (decimal)b.Numerator, (decimal)b.Denominator);
        //}
        //public static IR.RealNumber Multiply(this IQ.RationalNumber a, IR.RealNumber b)
        //{
        //    return b * a;
        //}
        //public static IR.RealNumber Divide(this IR.RealNumber a, IQ.RationalNumber b)
        //{
        //    if (a.Value == 0)
        //        return IZ.New(0);

        //    return IR.New(a.Value * b.Denominator, b.Numerator);
        //}
        //public static IR.RealNumber Divide(this IQ.RationalNumber a, IR.RealNumber b)
        //{
        //    if (a.Numerator == 0)
        //        return IZ.New(0);

        //    return IR.New(a.Numerator, (decimal)a.Denominator * b.Value);
        //}
        //public static IR.RealNumber Add(this IR.RealNumber a, IQ.RationalNumber b)
        //{
        //    if (a.Value == 0)
        //        return b;
        //    else if (b.Numerator == 0)
        //        return a;

        //    return IR.New((a.Value * b.Denominator) + b.Numerator, b.Denominator);
        //}
        //public static IR.RealNumber Add(this IQ.RationalNumber a, IR.RealNumber b)
        //{
        //    return b + a;
        //}
        //public static IR.RealNumber Substract(this IR.RealNumber a, IQ.RationalNumber b)
        //{
        //    return a + (-b);
        //}
        //public static IR.RealNumber Substract(this IQ.RationalNumber a, IR.RealNumber b)
        //{
        //    return (-b) + a;
        //}
        //#endregion

        //#region decimal Integrity
        //public static IR.RealNumber Multiply(this decimal a, IQ.RationalNumber b)
        //{
        //    if (a == 0 || b.Numerator == 0)
        //        return IZ.New(0);

        //    return IR.New(a * b.Numerator, (decimal)b.Denominator);
        //}
        //public static IR.RealNumber Multiply(this IQ.RationalNumber a, decimal b)
        //{
        //    return b * a;
        //}
        //public static IR.RealNumber Divide(this decimal a, IQ.RationalNumber b)
        //{
        //    if (a == 0)
        //        return IZ.New(0);

        //    return IR.New(a * b.Denominator, b.Numerator);
        //}
        //public static IR.RealNumber Divide(this IQ.RationalNumber a, decimal b)
        //{

        //    if (a.Numerator == 0)
        //        return IZ.New(0);

        //    return IR.New(a.Numerator, (decimal)a.Denominator * b);
        //}
        //public static IR.RealNumber Add(this decimal a, IQ.RationalNumber b)
        //{
        //    if (b.Numerator == 0)
        //        return IR.New(a);

        //    return IR.New((a * b.Denominator) + b.Numerator, b.Denominator);
        //}
        //public static IR.RealNumber Add(this IQ.RationalNumber a, decimal b)
        //{
        //    return b + a;
        //}
        //public static IR.RealNumber Substract(this decimal a, IQ.RationalNumber b)
        //{
        //    return a + (-b);
        //}
        //public static IR.RealNumber Substract(this IQ.RationalNumber a, decimal b)
        //{
        //    return (-b) + a;
        //}
        //#endregion

        //#region int Integrity
        //public static IQ.RationalNumber Multiply(this int a, IQ.RationalNumber b)
        //{
        //    if (a == 0 || b.Numerator == 0)
        //        return IZ.New(0);

        //    return IQ.New(a * b.Numerator, b.Denominator);
        //}
        //public static IQ.RationalNumber Multiply(this IQ.RationalNumber a, int b)
        //{
        //    return b * a;
        //}
        //public static IQ.RationalNumber Divide(this int a, IQ.RationalNumber b)
        //{
        //    if (a == 0)
        //        return IZ.New(0);

        //    return a * IQ.New(b.Denominator, b.Numerator);
        //}
        //public static IQ.RationalNumber Divide(this IQ.RationalNumber a, int b)
        //{
        //    if (a.Numerator == 0)
        //        return IZ.New(0);

        //    return IQ.New(a.Numerator, a.Denominator * b);
        //}
        //public static IQ.RationalNumber Add(this int a, IQ.RationalNumber b)
        //{
        //    if (a == 0)
        //        return b;
        //    else if (b.Numerator == 0)
        //        return IZ.New(a);

        //    return IQ.New((a * b.Denominator) + b.Numerator, b.Denominator);
        //}
        //public static IQ.RationalNumber Add(this IQ.RationalNumber a, int b)
        //{
        //    return b + a;
        //}
        //public static IQ.RationalNumber Substract(this int a, IQ.RationalNumber b)
        //{
        //    return a + (-b);
        //}
        //public static IQ.RationalNumber Substract(this IQ.RationalNumber a, int b)
        //{
        //    return (-b) + a;
        //}
        //#endregion

        //#endregion
    }
}

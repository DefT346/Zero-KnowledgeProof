using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Zero_KnowledgeProof
{
    public static class MathUtils
    {
        public static BigInteger mod(BigInteger x, BigInteger m)
        {
            return (x % m + m) % m;
        }

        public static BigInteger GCD(BigInteger a, BigInteger b)
        {
            a = BigInteger.Abs(a);
            b = BigInteger.Abs(b);

            var t = BigInteger.Min(a, b);
            a = BigInteger.Max(a, b);
            b = t;

            while (a != 0 && b != 0)
            {
                if (a > b)
                    a %= b;
                else
                    b %= a;
            }

            return a | b;
        }

        public static BigInteger ExtGCD(BigInteger a, BigInteger b, out BigInteger x, out BigInteger y)
        {
            if (b == 0)
            {
                x = 1;
                y = 0;
                return a;
            }

            BigInteger g = ExtGCD(b, a % b, out y, out x); // x и y - переставляются
            y = y - (a / b) * x;
            return g;
        }

        public static BigInteger Inverse(BigInteger value, BigInteger modulus)
        {
            value = MathUtils.mod(value, modulus);
            if (GCD(value, modulus) == 1)
            {
                ExtGCD(value, modulus, out BigInteger x, out BigInteger y);
                return mod(x, modulus);
            }
            else
            {
                throw new Exception($"Обратного значения {value} по модулю {modulus} не сущетвует.");
            }
        }

        public static BigInteger ComparisonMinRoot(BigInteger a, BigInteger b, BigInteger m)
        {
            return Comparison(a, b, m, out _, getAllRoots: false)[0];
        }

        public static BigInteger[] Comparison(BigInteger a, BigInteger b, BigInteger m, out BigInteger d, bool getAllRoots = true)
        {
            d = GCD(a, m);
            var roots = new BigInteger[(int)d];

            if (b % d == 0)
            {
                a /= d;
                b /= d;
                m /= d;

                if (GCD(a, m) == 1)
                {
                    ExtGCD(a, m, out BigInteger x, out BigInteger y);
                    roots[0] = mod(x * b, m);
                    if (d > 1)
                        for (int n = 1; n < d; n++)
                            roots[n] = roots[0] + n * m;
                }
                return roots;
            }
            else
                return null;
        }

        public static BigInteger Sqrt(this BigInteger n)
        {
            if (n == 0) return 0;
            if (n > 0)
            {
                int bitLength = Convert.ToInt32(Math.Ceiling(BigInteger.Log(n, 2)));
                BigInteger root = BigInteger.One << (bitLength / 2);

                while (!isSqrt(n, root))
                {
                    root += n / root;
                    root /= 2;
                }

                return root;
            }

            throw new ArithmeticException("NaN");
        }

        private static Boolean isSqrt(BigInteger n, BigInteger root)
        {
            BigInteger lowerBound = root * root;
            BigInteger upperBound = (root + 1) * (root + 1);

            return (n >= lowerBound && n < upperBound);
        }

        public static BigInteger RandomValueLess(BigInteger less) // x
        {
            BigInteger result;
            do
            {
                result = new BigInteger(BigPrime.GetRandom(less.GetBitLength() - 1));
            } 
            while (result < 0 || result > less);

            return result;
        }

        static readonly BigInteger FastSqrtSmallNumber = 4503599761588224UL; // as static readonly = reduce compare overhead

        public static BigInteger SqrtFast(BigInteger value)
        {
            if (value <= FastSqrtSmallNumber) // small enough for Math.Sqrt() or negative?
            {
                if (value.Sign < 0) throw new ArgumentException("Negative argument.");
                return (ulong)Math.Sqrt((ulong)value);
            }

            BigInteger root; // now filled with an approximate value
            int byteLen = value.ToByteArray().Length;
            if (byteLen < 128) // small enough for direct double conversion?
            {
                root = (BigInteger)Math.Sqrt((double)value);
            }
            else // large: reduce with bitshifting, then convert to double (and back)
            {
                root = (BigInteger)Math.Sqrt((double)(value >> (byteLen - 127) * 8)) << (byteLen - 127) * 4;
            }

            for (; ; )
            {
                var root2 = value / root + root >> 1;
                if (root2 == root || root2 == root + 1) return root;
                root = value / root2 + root2 >> 1;
                if (root == root2 || root == root2 + 1) return root2;
            }
        }

        public static BigInteger Sqrt2(BigInteger n)
        {
            var q = BigInteger.One << ((int)BigInteger.Log(n, 2) >> 1);
            var m = BigInteger.Zero;
            while (BigInteger.Abs(q - m) >= 1)
            {
                m = q;
                q = (m + n / m) >> 1;
            }
            return q;
        }

        public static BigInteger QuadraticComparison(BigInteger a, BigInteger p, bool debug = false)
        {
            BigInteger x = -100;

            // Обработка случаев
            if (mod(p, 4) == 3)
            {

                var m = BigInteger.DivRem(p, 4, out BigInteger rem);
                x = BigInteger.ModPow(a, m + 1, p);
            }
            else
            if (mod(p, 8) == 5)
            {

                var m = BigInteger.DivRem(p, 8, out BigInteger rem);
                var l = BigInteger.ModPow(a, 2 * m + 1, p);

                if (l == p - 1) l = -1;

                if (l == 1)
                {
                    x = BigInteger.ModPow(a, m + 1, p);
                }
                else if (l == -1)
                {
                    var a1 = BigInteger.ModPow(a, m + 1, p);
                    var a2 = BigInteger.ModPow(2, 2 * m + 1, p);
                    x = a1 * a2 % p;
                }
                else
                    throw new Exception($"Ошибка: l была равна {l}");
            }

            return x;
        }

        public static (BigInteger x1, BigInteger x2) GetSquareRootByModulus(BigInteger value, BigInteger p, BigInteger q, BigInteger n)
        {
            var r = MathUtils.QuadraticComparison(value, p);
            var s = MathUtils.QuadraticComparison(value, q);

            MathUtils.ExtGCD(p, q, out BigInteger c, out BigInteger d);

            var sqrtiV1 = MathUtils.mod(r * d * q + s * c * p, n);
            var sqrtiV2 = MathUtils.mod(r * d * q - s * c * p, n);

            return (sqrtiV1, sqrtiV2);
        }
    }
}

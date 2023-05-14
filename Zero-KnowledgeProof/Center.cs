using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Zero_KnowledgeProof
{
    internal class Center // сторона B, проверяющая представляемое стороной А доказательство
    {

        public BigInteger n { get; private set; }

        public BigInteger S;
        private BigInteger V;
        private BigInteger quadx;

        private int tempRandomBit;

        public Center()
        {
            GenerateKeys(1024);
        }

        public int Ping()
        {
            // Сторона В посылает А случайный бит b
            var randomBit = (int)Math.Round(new Random().Next(0, 100) / 100f); /*Console.WriteLine($"\nrandomBit: {randomBit}");*/
            tempRandomBit = randomBit;
            return randomBit;
        }

        BigInteger x;
        public void SetX(BigInteger x)
        {
            this.x = x;
        }

        public bool Check(BigInteger ry)
        {
            if (tempRandomBit == 0)
            {
                var val = MathUtils.mod(ry * ry, n);
                Console.WriteLine($"------------------------------------");
                Console.WriteLine($"  x: {x}");
                Console.WriteLine($"val: {val}");
                Console.WriteLine($"------------------------------------");

                return x == val;
            }
            else
            {
                var val = MathUtils.mod(ry * ry * V, n);
                Console.WriteLine($"------------------------------------");
                Console.WriteLine($"  x: {x}");
                Console.WriteLine($"val: {val}");
                Console.WriteLine($"------------------------------------");

                return x == val;
            }
        }

        public void GenerateKeys(int size)
        {
            Console.WriteLine($"\nГенерация ключей...");


            BigInteger iV;

            while (true)
            {
                var p = new BigPrime(size / 2).ToBigInteger();
                var q = new BigPrime(size / 2).ToBigInteger();
                n = p * q;

                quadx = new BigInteger(BigPrime.GetRandom(size / 2));

                V = BigInteger.ModPow(quadx, 2, n);
                if (V % p == 0 || V % q == 0)
                {
                    Console.WriteLine("Условие нулей");
                    continue;
                }

                try
                {
                    iV = MathUtils.Inverse(V, n);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Подбор другого значения...");
                    continue;
                }

                var sqrtiV = MathUtils.GetSquareRootByModulus(iV, p, q, n);

                if (MathUtils.mod(sqrtiV.x1 * sqrtiV.x1, n) == MathUtils.mod(iV, n))
                {
                    S = MathUtils.ComparisonMinRoot(1, sqrtiV.x1, n);
                    break;
                }
                else if (MathUtils.mod(sqrtiV.x2 * sqrtiV.x2, n) == MathUtils.mod(iV, n))
                {
                    S = MathUtils.ComparisonMinRoot(1, sqrtiV.x2, n);
                    break;
                }
                //else
                //    Console.WriteLine($"Переподбор значений");
            }

            Console.WriteLine($"\nЗначение n: {n}");
            Console.WriteLine($"\nОткрытый ключ V: {V}");
            Console.WriteLine($"\nСекретный ключ S: {S}");
        }

    }
}

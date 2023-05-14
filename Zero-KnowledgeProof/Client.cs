using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Zero_KnowledgeProof
{
    internal class Client // сторона A, доказывающая свою подлинность
    {
        private BigInteger r;
        private BigInteger x;
        private BigInteger S;

        public Client(BigInteger S)
        {
            if (S == 0)
            {
                Console.Write("\n[Сторона А] Введите ключ S: ");
                this.S = BigInteger.Parse(Console.ReadLine());
            }
            else
            {
                this.S = S;
            }
        }

        public BigInteger GetX(BigInteger n)
        {
            // Сторона А выбирает некоторое случайное число r
            r = MathUtils.RandomValueLess(n); //Console.WriteLine($"\nr: {r}");

            // Затем она вычисляет x и отправляет его стороне В
            x = MathUtils.mod(r * r, n); //Console.WriteLine($"\nx: {x}");

            return x;
        }


        public BigInteger Pong(int randomBit, BigInteger n)
        {
            if (randomBit == 0)
            {
                Console.WriteLine($"[Отправка значения стороне B] r: {r}");
                return r;
            }
            else
            {
                var y = MathUtils.mod(r * S, n);
                Console.WriteLine($"[Отправка значения стороне B] y = r * S mod n = {y}");
                return y;
            }
        }
    }
}

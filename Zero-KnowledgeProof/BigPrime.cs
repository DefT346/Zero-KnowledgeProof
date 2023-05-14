using System.Numerics;


public class BigPrime
{
    private BigInteger value;

    public int bitsCount { get; private set; }

    public BigPrime(int size = 1024)
    {
        bitsCount = size;
        var randomNumber = GetRandom(size);
        value = new BigInteger(randomNumber);
        value = BigInteger.Abs(value);

        CorrectToNearestPrime();
    }

    public static BigInteger GetMinByBitCount(int bitsCount)
    {
        return BigInteger.Pow(2, bitsCount - 1);
    }

    public static BigInteger GetMinPrimeByBitCount(int bitsCount)
    {
        var min = GetMinByBitCount(bitsCount);

        while (MillerRabinTest(10, min, bitsCount) == false)
            min++;

        return min;
    }

    public static byte[] GetRandom(long bits)
    {
        var bytes = new List<byte>();

        int byteValue = 0;
        var rnd = new Random();
        for (int i = 0; i < bits; i++)
        {
            var bit = rnd.Next(0, 2) == 0 ? 1 : 0;

            if (i == bits - 1)
                byteValue.SetBit(i % 8, 1);
            else
                byteValue.SetBit(i % 8, bit);

            if (i % 8 == 7)
            {
                bytes.Add((byte)byteValue);
                byteValue = 0;
            }
            if (i == bits - 1)
            {
                bytes.Add((byte)byteValue);
            }
        }

        return bytes.ToArray();
    }

    private static bool MillerRabinTest(int k, BigInteger value, int bitsCount)
    {
        if (value == 2 || value == 3)
            return true;
        if (value < 2 || value % 2 == 0)
            return false;

        BigInteger d = value - 1;
        int s = 0;

        while (d % 2 == 0)
        {
            d /= 2;
            s += 1;

        }

        for (int i = 0; i < k; i++)
        {
            BigInteger a;

            do
                a = new BigInteger(GetRandom(bitsCount));
            while (a < 2 || a >= value - 2);

            BigInteger x = BigInteger.ModPow(a, d, value);

            if (x == 1 || x == value - 1)
                continue;

            for (int r = 1; r < s; r++)
            {
                x = BigInteger.ModPow(x, 2, value);

                if (x == 1)
                    return false;
                if (x == value - 1)
                    break;
            }

            if (x != value - 1)
                return false;
        }
        return true;
    }

    private BigInteger CorrectToNearestPrime()
    {
        while (MillerRabinTest(10, value, bitsCount) == false)
            value++;

        return value;
    }

    public static BigInteger operator *(BigPrime a, BigPrime b)
    {
        return a.value * b.value;
    }

    public static BigInteger operator -(BigPrime a, BigPrime b)
    {
        return a.value - b.value;
    }

    public static BigInteger operator -(BigPrime a, int b)
    {
        return a.value - b;
    }

    public static string ToBase64(BigInteger value)
    {
        return Convert.ToBase64String(value.ToByteArray());
    }

    public BigInteger ToBigInteger()
    {
        return value;
    }

    public override string ToString()
    {
        return value.ToString();
    }
}

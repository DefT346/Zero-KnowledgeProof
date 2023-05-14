using System.Collections;
using System.Numerics;

public static class BitArrayExtensions
{
    public static int ParseBits(string bitsString)
    {
        return Convert.ToInt32(bitsString, 2);
    }

    public static int MessageToBin(string messgae)
    {
        return Convert.ToInt32(messgae);
    }

    public static void SetBit(this ref int intValue, int bitPosition, int bit)
    {
        if (bit == 1)
            intValue |= (1 << bitPosition);
        else
            intValue &= ~(1 << bitPosition);
    }

    public static void SetBit(this ref BigInteger intValue, int bitPosition, int bit)
    {
        if (bit == 1)
            intValue |= (1 << bitPosition);
        else
            intValue &= ~(1 << bitPosition);
    }

    public static int GetBit(this int b, int bitNumber)
    {
        return (b >> bitNumber) & 1;
    }

    public static void Print(this int source, int seqSize)
    {
        Console.WriteLine(Convert.ToString(source, 2).PadLeft(seqSize, '0'));
    }

    public static BitArray Create(string bits)
    {
        var ba = new BitArray(bits.Length);
        for (int i = 0; i < bits.Length; i++)
            ba[i] = bits[i] == '1' ? true : false;
        return ba;
    }

    public static void Print(this BitArray bitArray)
    {
        foreach (bool bit in bitArray)
            Console.Write("{0}", bit == true ? 1 : 0);
        Console.WriteLine();
    }
}


using System;

public static class StringUtils
{
    //public static string 
}

public static class MathUtils
{
    // operands should be non-negative
    public static uint DivideRoundUp(uint num, uint divisor)
    {
        return (num + divisor - 1) / divisor;
    }

    //TODO: be careful with overflow handling
    public static ulong Power(this byte num, byte exponent)
    {
        uint r = 1;
        while (exponent != 0)
        {
            if ((exponent & 1) == 1)
                r *= num;
            num *= num;
            exponent >>= 1;
        }
        return r;
    }

    public static ulong PowerOf2(byte exponent)
    {
        return 1u << exponent;
    }

    public static bool IsPowerOf2(this byte v)
    {
        return (v & (v - 1)) == 0;
    }

    public static byte LogOf2(this ulong v)
    {
        // See https://graphics.stanford.edu/~seander/bithacks.html#IntegerLog
        ulong[] b = {0x2, 0xC, 0xF0, 0xFF00, 0xFFFF0000, 0xFFFFFFFF00000000};
        byte[] S = {1, 2, 4, 8, 16, 32};
        byte r = 0;
        for (uint i = 4; i >= 0; i--)
        {
            if ((v & b[i]) != 0u)
            {
                v >>= S[i];
                r |= S[i];
            }
        }
        return r;
    }

    public static byte IntegerLogOf2(this ulong v)
    // will return 0 if input is not a power of 2
    {
        if (v != 0 && (v & (v - 1)) == 0)
        {
            return LogOf2(v);
        }
        else return 0;
    }
}

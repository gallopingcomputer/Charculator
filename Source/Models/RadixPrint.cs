using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    //* General explanation of parameters: 
    //  (None of the parameters should be `null`)
    //      seq                 
    //      group_size          (also known as grouping_mode) specifies number of bytes to be printed as part of a group (see above). A group size of 0 should not be used with irreducible radices.
    //      word_separator      will be used to separate groups; this should not be empty or null unless the grouping mode is None (in which case it is ignored). However, also note that an empty or null value, or a None grouping mode, is not allowed under a non-group-reducible setting.
    //      byte_separator      separates bytes within groups. If empty, bytes will not be delimited within groups, and each group will instead be printed as a number (in particular, if the radix chosen is a root of 256, then the behavior is the same as if the empty string was used; for none-reducible radices, this is preferred over simply concatenating the numerical representations).
    //!NOTE: if byte_separator is empty, the length of the sequence shall not exceed ???

public static 
class RadixPrint
{
    static readonly char[] ALPHABET = 
    {
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
        'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
        'U', 'V', 'W', 'X', 'Y', 'Z'
    }; //TODO: upper-case or lower-case, or other options? (Maybe add one trascoding pass at the end... but that might be messy and somewhat "inefficient"??? Alternative: always use uppercase and convert to lowercase at the end if needed) //TODO: additoinally allow case-sensitive mappings???

    //* despite being of type `ulong`, `value` must not exceed the range of type `long` either, since we have that cast.
    public static 
    string PrintNumber(this Radix radix, ulong value)
    {
        if (radix == 2 || radix == 8 || radix == 10 || radix == 16)
            return Convert.ToString((long)value, radix);
        else if (radix <= 36)
        {
            byte digits = radix.GetDigitCountForNumber(value).Item1;
            char[] s = new char[digits];
            // The reason we pre-allocate the array is not because of "performance", but rather because it is more clear (in this case) to explicitly use an array as opposed to StringBuilder.

            while (digits-- > 0) // Nope, I'm not writing it as "digits --> 0".
            {
                byte d = (byte)(value % radix);
                s[digits] = ALPHABET[d];
                value /= radix;
            }
            return s.ToString();
        }
        else
            throw new NotSupportedException();
    }

    //public static string PrintPaddedNumber(this Radix radix, ulong value, )
    // Not used because it isn't absolutely necessary; the only advantage it provides is when we're using grouped formatting, as it removes the need to call GetDigitCount followed by PadLeft for each group when we know we are printing things in fixed-width groups.

    public static 
    string PrintByteSequenceHexadecimal(byte[] seq, string byte_separator = "")
    =>  String.Join(byte_separator, seq.Select(b => b.ToString("X2")));

    private static
    string PrintByteSequenceReducible(this Radix radix, byte[] seq, string byte_separator = "")
    {
        //* If used alone, this must satisfy the following conditions:
        //      byte a = MathUtils.IntegerLogOf2(radix.Value);
        //      System.Diagnostics.Debug.Assert((byte_separator != "") || ((a != 0) && (8 % a == 0))); 
        //* In practice, that second part is more like "radix == 2 || radix == 4 || radix == 16" (no support for base 256 yet)
        return String.Join(byte_separator, seq.Select(b => radix.PrintNumber(b)));
    }

    //! Input size must be restricted if using a byte-irreducible base with no separator (must be able to hold it in a UInt64); alternative is to use BigInteger for larger things. 
    //!!! - according to the docs here (https://docs.microsoft.com/en-us/dotnet/api/system.bitconverter.touint64), the byte array will be truncated
    public static 
    string PrintByteSequence(this Radix radix, byte[] seq, string byte_separator = "-")
    {
        if (byte_separator == "")
        {
            if (radix == 2 || radix == 4 || radix == 16)
                return radix.PrintByteSequenceReducible(seq, byte_separator);
            else
            {
                ulong value = BitConverter.ToUInt64(seq, 0);
                //!!! - TODO: ENDIANNESS IS WRONG; JUST USE BIGINT INSTEAD
                //!!! - in this case you should probably disable automatic update
                byte digits = radix.GetDigitCount((byte)seq.Length).Item1;
                return radix.PrintNumber(value).PadLeft(digits, '0');
            }
        }
        else
            return radix.PrintByteSequenceReducible(seq, byte_separator);
    }
}

public static 
class GroupedRadixPrint
{
    public static 
    string PrintFormattedHexadecimal(
        byte[] seq, string byte_separator = "", 
        byte group_size = 0, string word_separator = " ")
    {
        const string format = "X2";
        if (group_size == 0)
        {
            return RadixPrint.PrintByteSequenceHexadecimal(seq, byte_separator);
        }
        else
        {
            int offset = 0;
            return String.Join(word_separator,
                String.Join(byte_separator, new ArraySegment<byte>(seq, offset, group_size).Select(
                    b => { offset++; return b.ToString(format); }
                ))
            );
        }
    }

    public static 
    string PrintFormattedByteSequence(this Radix radix, 
        byte[] seq, string byte_separator = "", 
        byte group_size = 0, string word_separator = "")
    {
        if (group_size == 0) // None
        {
            return radix.PrintByteSequence(seq, byte_separator);
        }
        else
        {
            uint segcount = MathUtils.DivideRoundUp((uint)seq.Length, group_size);
            ArraySegment<byte>[] segments = new ArraySegment<byte>[segcount];
            for (uint i = 0; i < segcount; i++)
                segments[i] = new ArraySegment<byte>(seq, (int)(group_size * i), (int)group_size);

            return String.Join(word_separator, segments.Select(s =>
                radix.PrintByteSequence(s.ToArray(), byte_separator)
            ));
        }
    }

    public static 
    string PrintFormattedByteSequenceEx(this Radix radix,
        List<byte[]> encoded_text, 
        int grouping_mode, string error_string = "?-", 
        string word_separator = " ", string byte_separator = "")
    {
        if (grouping_mode <= 0) // Neutral or None
        {
            return String.Join(
                (grouping_mode == (int)GroupingModeType.Neutral) 
                    ? word_separator : "",
                encoded_text.Select(x => (x.Length == 0) 
                    ? error_string 
                    : radix.PrintFormattedByteSequence(x, byte_separator)
                ) //*| this assumes that all valid characters have non-zero length, which might not be true in some hypothetical wacky encoding that doesn't care about cross-compatibility
            );
        }
        else
        {
            StringBuilder str1 = new StringBuilder();

            int gl = 0;
            foreach (byte[] n in encoded_text)
            {
                if (n.Length == 0) throw new NotSupportedException(); // alternatively just return null
                // boneheaded exception

                foreach (byte b in n)
                {
                    str1.Append(b.ToString("X2") + byte_separator);
                    ++gl;

                    if (gl == grouping_mode)
                    {
                        str1.Append(word_separator);
                        gl = 0;
                    }
                }
            }

            return str1.ToString();
        }
    }

}

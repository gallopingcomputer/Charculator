using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StrEnc
{
    public static class RadixPrint
    {
        public static string GetHexadecimalForByteGroup(byte[] seq, string byte_separator = "")
            => String.Join(byte_separator, seq.Select(b => b.ToString("X2")));

        public static string GetDecimalForByteGroup(byte[] seq, string byte_separator = "-")
            => String.Join(byte_separator, seq.Select(b => b.ToString(/*TODO*/))); // TODO: also need some indication of how big the field is - pad with zeroes or use netmask notation?

        //*| the above method is very general (& also used for generating a textual representation of the hash value), but the stuff below are highly specific to the string encoding scenario (or at least so defined)

        public enum GroupingMode_Basic /* must be used in a type-safe manner (no out-of-range stuff) */
        { None = -1, Natural = 0, Forced }

        public enum GroupingMode { None = -1, Neutral = 0 }
        // otherwise it is "forced" - any positive value is allowed and is interpreted as the maximum number of bytes in a grouped sequence; any negative value is the same as None

        public static string GetFormattedHexadecimal(List<byte[]> encoded_text, int grouping_mode, string error_string = "?-", string word_separator = " ", string byte_separator = "")
        // word_separator is ignored when grouping_mode < 0
        {
            if (grouping_mode <= 0)
            {
                return String.Join(
                    (grouping_mode == 0) ? word_separator : "",
                    //*| hopefully branch prediction means this doesn't matter
                    encoded_text.Select(
                        x => (x.Length == 0) ? error_string : GetHexadecimalForByteGroup(x, byte_separator)
                    ) //*| this assumes that all valid characters have byte length > 1
                );
            }
            else
            {
                StringBuilder str1 = new StringBuilder();

                int gl = 0;
                foreach (byte[] n in encoded_text)
                {
                    if (n.Length == 0) throw new NotSupportedException();
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

                //?| Would it be much easier if you extend list with some sort of EnumerateAs method? - Oh wait there's SelectMany - Oh wait but it still wouldn't be alright since you wouldn't get a chance to catch the exception, so screw that
            }
        }
    }
}

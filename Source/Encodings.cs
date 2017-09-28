using System;
using System.Text;
using System.Collections.Generic;

namespace StrEnc.Info
{
    public static class Encodings
    {
        //TODO| OOP design: consider turning EncodingId into a class instead (and have a member field that stores the cached enc object) wait WHAT NO!

        public enum EncodingId { 
            Undefined = -1, 
            SystemDefault = 0, 
            ASCII = 20127,
            UTF7 = 65000,
            UTF8 = 65001,
            UTF16LE = 1200,
            UTF32LE = 12000
        }
        const EncodingId MIN_VALID_ENCODINGID = (EncodingId)0;
        const EncodingId MAX_VALID_ENCODINGID = (EncodingId)65535;

        public static string GetNumericalName(this EncodingId enc_id)
            => ((int)enc_id).ToString();

        public static Encoding GetEncoder(this EncodingId enc_id)
        {
            try {
                return Encoding.GetEncoding((int)enc_id,
                    EncoderExceptionFallback.ExceptionFallback, 
                    DecoderExceptionFallback.ExceptionFallback); 
            }
            catch (Exception e)
            {
                if (e is ArgumentException || e is NotSupportedException) /* unsupported codepage id */ return null;
                else { throw; }
            }
        }

        public static string GetEncodingFullName(Encoding enc) => enc.CodePage.ToString() + " | " + enc.EncodingName;
        public static string GetEncodingName(Encoding enc) => enc.EncodingName;
        public static string GetEncodingWebName(Encoding enc) => enc.WebName;

        public static string GetEncodingBodyName(Encoding enc) => enc.BodyName;
        public static string GetEncodingHeaderName(Encoding enc) => enc.HeaderName;


        //*| unnused, since it's kinda unwieldly
        public static int get_byte_count(Encoding enc, string source,
            ref List<int> enc_errors, ref List<int> enc_error_offset)
        {
            int count = 0;
            enc_errors.Clear(); enc_error_offset.Clear();

            for (int i = 0; i < source.Length; i++)
            {
                try {
                    count += enc.GetByteCount(source.Substring(i, 1));
                }
                catch (EncoderFallbackException) {
                    enc_errors.Add(i); 
                    enc_error_offset.Add(count);
                }
            }
            return count;
        }

        public static void get_encoded_text(Encoding enc, string source, ref List<byte[]> encoded_text, ref List<int> enc_errors)
        {
            encoded_text.Clear(); enc_errors.Clear();
            for (int i = 0; i < source.Length; i++)
            {
                try { encoded_text.Add(enc.GetBytes(source.Substring(i, 1))); }
                catch (EncoderFallbackException) { encoded_text.Add(new byte[0]); enc_errors.Add(i); }
            }
        }

        public static void get_encoded_text(Encoding enc, string source, ref List<byte> encoded_text, out byte[] enc_state, ref List<int> enc_errors)
        {
            encoded_text.Clear(); enc_errors.Clear();
            enc_state = new byte[source.Length];
            for (int i = 0; i < source.Length; i++)
            {
                try {
                    byte[] ch = enc.GetBytes(source.Substring(i, 1));
                    byte j = 0; for (; j < ch.Length; ++j)
                        encoded_text.Add(ch[j]);
                    enc_state[i] = j;
                }
                catch (EncoderFallbackException) {
                    enc_state[i] = 0; enc_errors.Add(i);
                }
            }
        }
    }

    
}
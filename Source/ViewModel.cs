using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace StrEnc.Info
{
    public class StringInfoView
    {
        public StringInfoView()
        {
            _text = "";
            _enc_id = 0;
            _grouping_mode = 0;
            _error_string = "?-";
            hashalg_id = Hashing.HashAlgId.SHA1;
            //TODO|

            _Materialize();
        }


        //* source text

        private string _text;
        public string Text
        {
            get => _text;
            set { _text = value; _Materialize(); }
        }


        //* processing parameters (group 1)

        private Encodings.EncodingId _enc_id;
        private Encoding _enc;
        public Encodings.EncodingId EncodingID { get => _enc_id; }
        public Encoding Encoder { get => _enc; }
        public bool IsEncodingSupported() => _enc != null;
        public bool SetEncoding(int value)
        // returns true on success, false on failure
        {
            bool valid = value < 65536 && value >= 0;

            /* if (new_enc != null)
            {
                _enc_id = value; _enc = new_enc;
                _Materialize(); 
                return true;
            }
            else
                return false; */
            //*| I've decided that the viewmodel *should* be able to accept unsupported encoding id's

            if (valid)
            {
                _enc_id = (Encodings.EncodingId)value;
                _enc = _enc_id.GetEncoder();
                _Materialize();
            }

            return valid;
        }


        //* cached results

        private byte[] _encoded_text_flattened;
        private List<byte[]> _encoded_text;
        private List<int> _enc_errors; /* list of character indices that failed to encode */

        private void _Materialize()
        {
            if (_enc == null)
            {
                _encoded_text = null;
                _enc_errors = null;
                _encoded_text_flattened = null;
            }
            else
            {
                _encoded_text = new List<byte[]>();
                _enc_errors = new List<int>();
                StrEnc.Info.Encodings.get_encoded_text(_enc, _text, ref _encoded_text, ref _enc_errors);
                _encoded_text_flattened = _encoded_text.SelectMany(a => a).ToArray();
            }
        }

        public bool GetEncodingErrorState() 
            => (_enc == null) 
            ? /*(bool?)null*/ throw new InvalidOperationException() 
            : (_enc_errors.Count > 0);

        public int GetEncodingErrorCount(int end = -1, int begin = 0)
        {
            if (_enc == null)
                //return null;
                throw new InvalidOperationException();
            else if (end < 0)
                return _enc_errors.Count;
            else
                //TODO| consider throwing if begin is invalid? (e.g. < 0 || greater than end)
                return _enc_errors.Where(a => a < end && a >= begin).Count();
        }

        public int GetEncodedLength(int end = -1, int begin = 0)
        {
            if (_enc == null)
                throw new InvalidOperationException();
            else if (end < 0)
                return _encoded_text_flattened.Length;
            else
            {
                //TODO| consider throwing if begin is invalid? (e.g. < 0 || greater than end)
                int count = 0;
                for (int i = begin; i < end; i++)
                    count += _encoded_text[i].Length;
                return count;
            }
        }


        //* group 2 / display parameters

        private int _grouping_mode;
        public int GroupingMode { get => _grouping_mode; }
        public bool SetGroupingMode(int gr_new)
		{
			bool valid = !(gr_new < -1 || gr_new > 8);
            //*| "(GetEncodingErrorCount() > 0 && gr_new > 0)" should no longer be part of this

            if (valid)
                _grouping_mode = gr_new;

            return valid;
		}

        private string _error_string;
        public string ErrorString { get => _error_string; }
        public bool SetErrorString(string value)
        {
            bool valid = true;
            //TODO| validate and prevent ambiguous strings from being used

            if (valid)
                _error_string = value;

            return valid;
        }

        public Hashing.HashAlgId hashalg_id;

        
        public string GetHexadecimalText()
        // returns null if there are encoding errors && gr != 0 (we currently assume that the error_string parameter does not generate conditional/dynamically-determined errors, which might change if, say, we add display modes other than hexadecimal)
            => (GetEncodingErrorCount() != 0 && GroupingMode != 0) ? null : Textual.GetFormattedHexadecimal(_encoded_text, GroupingMode, ErrorString);
            //!!!| returns null if grouping setings are bad - WAIT... what if it's the encoding that's bad??? => hierarchy of errors (or subcomponent states in general) - this is about how you define the error codes & who is responsible for which errors (subcomp. states) => check _enc != null (i.e. IsEncodingSupported()), i.e. this assumes that a valid encoding is chosen
            //*| GetHexText is responsible for encoding errors (and incorrect GR), but not for picking an unsupported encoder => who's responsible for that? RefreshAll? SetEncoding?
            //*| compare with GR (SetGR and RefreshHexText)... this must be handled in Refresh, since a valid GR can be invalidated later by encoding errors (similar to how hashing is disabled, which is also handled in RefreshHash & GetHashText???)

        public string GetHashText()
        // returns null if there are encoding errors
            => Textual.GetHexadecimal(Hashing.GetHashValue(_encoded_text_flattened, hashalg_id));
            //TODO| problem with using List<byte[]> is that an array still needs to be materialized
            //?| but I guess you could create a stream object when it gets bad enough? But it sounds horribly complicated; this only changes a constant factor (since hashing is already O(N))... to really make it work better you'd need better editing mechanics
    }

    
    //
    // ───  ───────────────────────────────────────────────────────────────────────────
    //

    public static class Textual
    {
        public static string GetHexadecimal(byte[] dig, string byte_separator = "")
            => String.Join(byte_separator, dig.Select(b => b.ToString("X2")));
        
        // this is preferred over BitConverter because it always generates hyphen separated strings
        
        //*| the above method is very general (& also used for generating a textual representation of the hash value), but the stuff below are highly specific to the string encoding scenario (or at least so defined)
        
        public enum GroupingMode_Basic /* must be used in a type-safe manner (no out-of-range stuff) */
        { None = -1, Natural = 0, Forced }

        public enum GroupingMode { None = -1, Neutral = 0 }
        // otherwise it is "forced" - any positive value is allowed. and any negative value is the same as None, but this isn't a great idea

        public static string GetFormattedHexadecimal(List<byte[]> encoded_text, int grouping_mode, string error_string = "?-", string word_separator = " ", string byte_separator = "")
            // word_separator is ignored when grouping_mode < 0
        {
            if (grouping_mode <= 0)
            {
                return String.Join(
                    (grouping_mode == 0) ? word_separator : "", 
                        //*| hopefully branch prediction means this doesn't matter
                    encoded_text.Select(
                        x => (x.Length == 0) ? error_string : GetHexadecimal(x, byte_separator)
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
                        str1.Append(b.ToString("X2"));
                        ++gl;

                        if (gl == grouping_mode) 
                        {
                            str1.Append(' '); 
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StrEnc
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

            _Materialize();
        }

        public StringInfoView(String text/*, */)
        {
            //TODO: finish this constructor

            //TODO: split into two (encoding converter and hash converter)

            _Materialize();
        }


        // this assumes that the setters do not perform deep copies
        private T _Xc_Set<T>(T value, Func<T, bool?> ns) {
            if (ns(value) == true)
                return value;
            else
                throw new ArgumentOutOfRangeException();
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
        /* returns true on success, false on failure;
         * even if an encoding is unsupported, it is still considered valid for this */
        {
            bool valid = value < 65536 && value >= 0;

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
        // throws on error (null encoder)
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
                StrEnc.Encodings.get_encoded_text(_enc, _text, ref _encoded_text, ref _enc_errors);
                _encoded_text_flattened = _encoded_text.SelectMany(a => a).ToArray();
            }
        }

        public bool GetEncodingErrorState() 
            => (_enc == null) 
            ? throw new InvalidOperationException() 
            : (_enc_errors.Count > 0);

        public int GetEncodingErrorCount(int end = -1, int begin = 0)
        {
            if (_enc == null)
                throw new InvalidOperationException();
            else if (end < 0) // get total errors
                return _enc_errors.Count;
            else
                return _enc_errors.Where(a => a < end && a >= begin).Count();
        }

        public int GetEncodedLength(int end = -1, int begin = 0)
        {
            if (_enc == null)
                throw new InvalidOperationException();
            else if (end < 0) // get total length
                return _encoded_text_flattened.Length;
            else
            {
                int count = 0;
                for (int i = begin; i < end; i++)
                    count += _encoded_text[i].Length;
                return count;
            }
        }


        //* group 2 / display parameters

        public int WordSeparator { get; private set; }

        private int _grouping_mode;
        public int GroupingMode { get => _grouping_mode; }
        public bool SetGroupingMode(int gr_new)
		{
			bool valid = !(gr_new < -1 || gr_new > 8);

            if (valid)
                _grouping_mode = gr_new;

            return valid;
		}

        private string _error_string;
        public string ErrorString { get => _error_string; }
        public bool SetErrorString(string value)
        {
            bool valid = true;

            if (value.Contains(" "))
            {
                valid = false;
            }

            //TODO| maybe not use " " as the magic string?

            //TODO| validate and prevent ambiguous strings from being used - if it contains spaces, or if it only contains the hexadecimal letters; this depends on the radix selected
            //TODO| similarly, the default value should also be different depending on radix ("?-" for hexadecimal becaue a byte is two nibbles, and "?" for decimal)

            //TODO| grouping mode also depends on radix (decimal can't have 0) => wait actually it can (but only with the right separator)

            if (valid)
                _error_string = value;

            return valid;
        }

        public Hashing.HashAlgId hashalg_id;

        
        public string GetHexadecimalText()
        // returns null if there are encoding errors && gr != 0 (we currently assume that the error_string parameter does not generate conditional/dynamically-determined errors, which might change if, say, we add display modes other than hexadecimal)
            => (GetEncodingErrorCount() != 0 && GroupingMode != 0) ? null : RadixPrint.GetFormattedHexadecimal(_encoded_text, GroupingMode, ErrorString);

        public string GetHashText()
        // returns null if there are encoding errors
            => RadixPrint.GetHexadecimalForByteGroup(Hashing.GetHashValue(_encoded_text_flattened, hashalg_id));

    }
}
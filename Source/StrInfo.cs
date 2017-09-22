using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;

namespace StrEnc
{
	public static class StrEncInfo
	{
		#region et_length
		public static int et_getlength(Encoding enc, string instring, ref List<int> enc_error_pos)
		{
            List<int> enc_error_offset = new List<int>();
            return et_getlength(enc, instring, ref enc_error_pos, ref enc_error_offset);
            // simply discard the error offset list
        }
		public static int et_getlength(Encoding enc, string instring, ref List<int> enc_error_pos, ref List<int> enc_error_offset)
		{	int et_length = 0;
			enc_error_pos.Clear(); enc_error_offset.Clear();
			for (int i = 0; i < instring.Length; i++)
			{	try { et_length += enc.GetByteCount(instring.Substring(i, 1)); }
				catch (EncoderFallbackException) { enc_error_pos.Add(i); enc_error_offset.Add(et_length); } }
			return et_length; }
		#endregion

		#region encode_text
		public static void get_et(Encoding enc, string instring, ref List<byte[]> et0) 
		{
			et0.Clear();
			for (int i = 0; i < instring.Length; i++)
			{	try { et0.Add(enc.GetBytes(instring.Substring(i, 1))); }
				catch (EncoderFallbackException) { et0.Add(new byte[0]); } } 
		}
		public static void get_et(Encoding enc, string instring, ref List<byte[]> et0, ref List<int> enc_error_pos) 
		{
			et0.Clear(); enc_error_pos.Clear();
			for (int i = 0; i < instring.Length; i++)
			{	try { et0.Add(enc.GetBytes(instring.Substring(i, 1))); }
				catch (EncoderFallbackException) { et0.Add(new byte[0]); enc_error_pos.Add(i); } } 
		}
		public static void get_et(Encoding enc, string instring, ref List<byte> et, out byte[] cl)
		{
			et.Clear();
			cl = new byte[instring.Length];
			for (int i = 0; i < instring.Length; i++)
			{
				try { 
					byte[] ch = enc.GetBytes(instring.Substring(i, 1));
					byte j = 0; for (; j < ch.Length; ++j)
						et.Add(ch[j]);
					cl[i] = j; }
				catch (EncoderFallbackException) { cl[i] = 0; }
			}
		}
		public static void get_et(Encoding enc, string instring, ref List<byte> et, out byte[] cl, ref List<int> enc_error_pos)
		{
			et.Clear(); enc_error_pos.Clear();
			cl = new byte[instring.Length];
			for (int i = 0; i < instring.Length; i++)
			{	try { 
					byte[] ch = enc.GetBytes(instring.Substring(i, 1));
					byte j = 0; for (; j < ch.Length; ++j)
						et.Add(ch[j]);
					cl[i] = j; }
				catch (EncoderFallbackException) { cl[i] = 0; enc_error_pos.Add(i); } }
		}
		#endregion

		#region formatted_string_conversions
		public static string strx2(byte[] dig)
		{
			StringBuilder str1 = new StringBuilder();
			foreach (byte b in dig)
				str1.Append(b.ToString("X2"));
			return str1.ToString();
		}

		// the code for (gr < 0) is the same for the three different functions (regular, _2 and _e).

		// strx2g_2 - unencodable characters are omitted and additional space is added
		public static void strx2g_2(ref List<byte[]> et0, int gr, out StringBuilder str1)
		{
            str1 = new StringBuilder();
			if (gr == 0)
            {
				for (int i = 0; i < et0.Count; i++) {
					for (int k = 0; k < et0[i].Length; ++k)
						str1.Append(et0[i][k].ToString("X2"));
					str1.Append(' ');
                }
			}
			else if (gr < 0)
            {
				for (int i = 0; i < et0.Count; i++)
					for (int k = 0; k < et0[i].Length; ++k)
						str1.Append(et0[i][k].ToString("X2"));
			}
			else
            {
				for (int i = 0, gl = 0; i < et0.Count; ++i) { 
					// gr > 0 is not allowed when there are encoding errors.
					if (et0[i].Length == 0) throw new NotSupportedException();
					for (int k = 0; k < et0[i].Length; ++k) { 
						str1.Append(et0[i][k].ToString("X2")); ++gl; 
						if (gl == gr) { str1.Append(' '); gl = 0; }
                    }
				}
			}
		}
		public static void strx2g_2(ref List<byte> et, ref byte[] cl, int gr, out StringBuilder str1)
		{
            str1 = new StringBuilder();
			if (gr == 0)
            {
				for (int k = 0, i = 0, gl = 0; k < et.Count; ) { // i is index of chars. k is the index of bytes.
					if (cl[i] != 0) {
						str1.Append(et[k].ToString("X2")); ++k; ++gl;
						if (gl == cl[i]) { ++i; str1.Append(' '); gl = 0; } }
					else { ++i; str1.Append(' '); }
				}
			}
			else if (gr < 0)
            {
				for (int k = 0; k < et.Count; ++k) {
					str1.Append(et[k].ToString("X2")); }
			}
			else
            {
				for (int k = 0, i = 0, gl = 0, gn = 0; k < et.Count; ) { // gn is the index of a byte in the char at i
					if (cl[i] != 0) {
						str1.Append(et[k].ToString("X2")); ++k; ++gl;
						if (gn == cl[i]) { ++i; gn = 0; }
						if (gl == gr) { str1.Append(' '); gl = 0; } }
					// gr > 0 is not allowed when there are encoding errors.
					else { throw new NotSupportedException(); } }
			}
		}

		// strx2g - unencodable characters are omitted
		public static void strx2g(ref List<byte[]> et0, int gr, out StringBuilder str1)
		{
            str1 = new StringBuilder();
			if (gr == 0)
            {
				for (int i = 0; i < et0.Count; i++) {
					for (int k = 0; k < et0[i].Length; ++k)
						str1.Append(et0[i][k].ToString("X2"));
					if (et0[i].Length != 0) str1.Append(' '); }
			}
			else if (gr < 0)
            {
				for (int i = 0; i < et0.Count; i++)
					for (int k = 0; k < et0[i].Length; ++k)
						str1.Append(et0[i][k].ToString("X2"));
			}
			else
            {
				for (int i = 0, gl = 0; i < et0.Count; ++i) { 
					// gr > 0 is not allowed when there are encoding errors.
					if (et0[i].Length == 0) throw new NotSupportedException();
					for (int k = 0; k < et0[i].Length; ++k)
						{ str1.Append(et0[i][k].ToString("X2")); ++gl; 
						if (gl == gr) { str1.Append(' '); gl = 0; } }
				}
			}
		}
		public static void strx2g(ref List<byte> et, ref byte[] cl, int gr, out StringBuilder str1)
		{
            str1 = new StringBuilder();
			if (gr == 0)
            {
				for (int k = 0, i = 0, gl = 0; k < et.Count; ) { // i is the index of chars. k is the index of bytes.
					if (cl[i] != 0) {
						str1.Append(et[k].ToString("X2")); ++k; ++gl;
						if (gl == cl[i]) { ++i; str1.Append(' '); gl = 0; } }
					else { ++i; }
				}
			}
			else if (gr < 0) {
				for (int k = 0; k < et.Count; ++k) {
					str1.Append(et[k].ToString("X2")); }
			}
			else {
				for (int k = 0, i = 0, gl = 0, gn = 0; k < et.Count; ) { // gn is the index of a byte in the char at i
					if (cl[i] != 0) {
						str1.Append(et[k].ToString("X2")); ++k; ++gl;
						if (gn == cl[i]) { ++i; gn = 0; }
						if (gl == gr) { str1.Append(' '); gl = 0; } }
					// gr > 0 is not allowed when there are encoding errors.
					else { throw new NotSupportedException(); } }
			}		
		}

		// strx2g_e - characters not supported by the encoding are displayed as "?-"s
		public static void strx2g_e(ref List<byte[]> et0, int gr, out StringBuilder str1, string errstring = "?-")
		{
			str1 = new StringBuilder();
			if (gr == 0) {
				for (int i = 0; i < et0.Count; ++i) {
					if (et0[i].Length == 0) { str1.Append(errstring); }
					else {
						for (int k = 0; k < et0[i].Length; ++k) 
						{ str1.Append(et0[i][k].ToString("X2")); } }
					str1.Append(" ");
				}
			}
			else if (gr < 0) {
				for (int i = 0; i < et0.Count; ++i) {
					if (et0[i].Length == 0) { str1.Append(errstring); }
					else {
						for (int k = 0; k < et0[i].Length; ++k) 
						{ str1.Append(et0[i][k].ToString("X2")); } }
				}
			}
			else {
				for (int i = 0, gl = 0; i < et0.Count; ++i) { 
					// gr > 0 is not allowed when there are encoding errors.
					if (et0[i].Length == 0) throw new NotSupportedException();
					for (int k = 0; k < et0[i].Length; ++k)
						{ str1.Append(et0[i][k].ToString("X2")); ++gl; 
						if (gl == gr) { str1.Append(' '); gl = 0; } }
				}
			}
		}
		public static void strx2g_e(ref List<byte> et, ref byte[] cl, int gr, out StringBuilder str1, string errstring = "?=")
		{
			str1 = new StringBuilder();
			if (gr == 0) {
				for (int i = 0, j = 0; i < cl.Length; ++i) {
					if (cl[i] == 0) str1.Append(errstring);
					else {
						for (int k = 0; k < cl[i]; ++k, ++j)
							str1.Append(et[j].ToString("X2")); }
					str1.Append(" ");
				}
			}
			else if (gr < 0) {
				for (int i = 0, j = 0; i < cl.Length; ++i) {
					if (cl[i] == 0) str1.Append(errstring);
					else {
						for (int k = 0; k < cl[i]; ++k)
						{ str1.Append(et[j].ToString("X2")); ++j; }
				} }
			}
			else {
				for (int k = 0, i = 0, gl = 0, gn = 0; k < et.Count; ) { // gn is the index of a byte in the char at i
					if (cl[i] != 0) {
						str1.Append(et[k].ToString("X2")); ++k; ++gl;
						if (gn == cl[i]) { ++i; gn = 0; }
						if (gl == gr) { str1.Append(' '); gl = 0; } }
					// gr > 0 is not allowed when there are encoding errors.
					else { throw new NotSupportedException(); } }
			}
		}
		#endregion

		#region Hash
		public enum HashAlgId { SHA1, SHA256, SHA384, SHA512, MD5 };
		public static byte[] hash(byte[] src, HashAlgId hashalg) // generate hashcode in byte[] form from byte[] source
		{
			HashAlgorithm hasher;
			switch (hashalg)
			{
				default:
					Exception e = new ArgumentOutOfRangeException(); throw e;
				case HashAlgId.SHA1:
					hasher = new SHA1CryptoServiceProvider(); return hasher.ComputeHash(src);
				case HashAlgId.SHA256:
					hasher = new SHA256CryptoServiceProvider(); return hasher.ComputeHash(src);
				case HashAlgId.SHA384:
					hasher = new SHA384CryptoServiceProvider(); return hasher.ComputeHash(src);
				case HashAlgId.SHA512:
					hasher = new SHA512CryptoServiceProvider(); return hasher.ComputeHash(src);
				case HashAlgId.MD5:
					hasher = new MD5CryptoServiceProvider(); return hasher.ComputeHash(src);
			}
		}
		#endregion
	}
}
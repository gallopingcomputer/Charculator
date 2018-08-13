using System;
using System.Security.Cryptography;
using System.Collections.Generic;

namespace StrEnc
{
    public static class Hashing
    {
        public enum HashAlgId { SHA1, SHA256, SHA384, SHA512, MD5 };

        public static string GetName(this HashAlgId h) => GetHashAlgName(h);

        public static string GetHashAlgName(HashAlgId h)
        {
            switch (h)
            {
                case HashAlgId.SHA1:	return "SHA1";
                case HashAlgId.SHA256:	return "SHA2-256";
                case HashAlgId.SHA384:	return "SHA2-384";
                case HashAlgId.SHA512:	return "SHA2-512";
                case HashAlgId.MD5:		return "MD5";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static HashAlgorithm GetHasher(HashAlgId h)
        {
            switch (h)
            {
                case HashAlgId.SHA1:    return new SHA1CryptoServiceProvider(); 
                case HashAlgId.SHA256:  return new SHA256CryptoServiceProvider(); 
                case HashAlgId.SHA384:  return new SHA384CryptoServiceProvider(); 
                case HashAlgId.SHA512:  return new SHA512CryptoServiceProvider(); 
                case HashAlgId.MD5:     return new MD5CryptoServiceProvider(); 
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static byte[] GetHashValue(byte[] source, HashAlgId h)
            => GetHasher(h).ComputeHash(source);
    }
}
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace vChat.Lib
{
    public class MD5Encrypt
    {
        /// <summary>
        /// Encrypt an input text into MD5 hash
        /// </summary>
        /// <param name="Text">Input text to encrypt</param>
        /// <returns>MD5 hash string</returns>
        /// <example>
        ///     String MD5Hash = MD5Encrypt.Hash("itexplore");
        /// </example>
        public static String Hash(String Text)
        {
            MD5 md5 = MD5.Create();
            byte[] hashData = md5.ComputeHash(Encoding.Default.GetBytes(Text));

            return BitConverter.ToString(hashData).Replace("-", "").ToLower();
        }
    }
}

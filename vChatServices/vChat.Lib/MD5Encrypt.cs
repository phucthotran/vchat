using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace vChat.Lib
{
    public class MD5Encrypt
    {
        /// <summary>
        /// Mã hóa một chuỗi thành mã MD5
        /// </summary>
        /// <param name="Text">Chuỗi cần mã hóa</param>
        /// <returns>Chuỗi MD5</returns>
        public static String Hash(String Text)
        {
            MD5 md5 = MD5.Create();
            byte[] hashData = md5.ComputeHash(Encoding.Default.GetBytes(Text));

            return BitConverter.ToString(hashData).Replace("-", "").ToLower();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QLTK_Nro_Pro.Presenter
{
    internal class CryptoManager
    {
        public static string Encryptor(string string_5, string string_6)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(string_5);
            byte[] key = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(string_6));
            byte[] array = new TripleDESCryptoServiceProvider
            {
                Key = key,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            }.CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length);
            return Convert.ToBase64String(array, 0, array.Length);
        }
        public static string Decryptor(string str, string key)
        {
            byte[] array = Convert.FromBase64String(str);
            byte[] key2 = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(key));
            byte[] bytes = new TripleDESCryptoServiceProvider
            {
                Key = key2,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            }.CreateDecryptor().TransformFinalBlock(array, 0, array.Length);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}

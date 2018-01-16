using System;
using System.Security.Cryptography;
using System.Text;
using log4net;

namespace Steven.Core.Utilities
{
    public class EncryptUtils
    {
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Decrypt(string txt, string key)
        {
            try
            {
                byte[] inputBuffer = Convert.FromBase64String(txt);
                Rijndael rijndael = new RijndaelManaged();
                MD5 md = new MD5CryptoServiceProvider();
                rijndael.BlockSize = 0x80;
                rijndael.KeySize = 0x80;
                rijndael.Mode = CipherMode.CBC;
                rijndael.Padding = PaddingMode.PKCS7;
                rijndael.IV = new byte[0x10];
                rijndael.Key = md.ComputeHash(Encoding.UTF8.GetBytes(key));
                ICryptoTransform transform = rijndael.CreateDecryptor();
                byte[] byteResult = transform.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
                return Encoding.UTF8.GetString(byteResult);
            }
            catch (Exception ex)
            {
                var log = LogManager.GetLogger("EncryptUtils.Decrypt");
                log.Error(ex);
                return string.Empty;
            }
        }

        public static string Encrypt(string txt, string key)
        {
            try
            {
                Rijndael rijndael = new RijndaelManaged();
                MD5 md = new MD5CryptoServiceProvider();
                rijndael.BlockSize = 0x80;
                rijndael.KeySize = 0x80;
                rijndael.Mode = CipherMode.CBC;
                rijndael.Padding = PaddingMode.PKCS7;
                rijndael.IV = new byte[0x10];
                rijndael.Key = md.ComputeHash(Encoding.UTF8.GetBytes(key));
                byte[] bytes = Encoding.UTF8.GetBytes(txt);
                ICryptoTransform transform = rijndael.CreateEncryptor();
                byte[] byteResult = transform.TransformFinalBlock(bytes, 0, bytes.Length);
                return Convert.ToBase64String(byteResult);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string HMACMD5Hash(string source, string secrect)
        {

            HMACMD5 hmacMD5 = new HMACMD5(Encoding.UTF8.GetBytes(secrect));

            // Convert the input string to a byte array and compute the hashamc.
            byte[] data = hmacMD5.ComputeHash(Encoding.UTF8.GetBytes(source));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("X2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }
}

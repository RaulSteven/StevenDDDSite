using System;
using System.Security.Cryptography;
using System.Text;

namespace Steven.Core.Utilities
{
    public class HashUtils
    {
        private static HashAlgorithm CreateAlgorithm(string hashFormat)
        {
            if (hashFormat.Equals("SHA1", StringComparison.OrdinalIgnoreCase))
            {
                return SHA1.Create();
            }
            if (hashFormat.Equals("MD5", StringComparison.OrdinalIgnoreCase))
            {
                return MD5.Create();
            }
            if (hashFormat.Equals("SHA256", StringComparison.OrdinalIgnoreCase))
            {
                return SHA256.Create();
            }
            throw new ArgumentException("无效的参数!");
        }

        private static string ByteArrayToHexString(byte[] bytes)
        {
            var result = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                result.Append(bytes[i].ToString("x").PadLeft(2, '0').ToUpper());
            }
            return result.ToString();
        }

        /// <summary>
        /// 计算给定字符串的哈希值
        /// </summary>
        /// <param name="str"></param>
        /// <param name="hashFormat"></param>
        /// <returns></returns>
        private static string HashString(string str, string hashFormat)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            HashAlgorithm algorithm = CreateAlgorithm(hashFormat);
            bytes = algorithm.ComputeHash(bytes);
            return ByteArrayToHexString(bytes);
        }

        /// <summary>
        /// 针对密码计算哈希值
        /// </summary>
        /// <param name="pass"></param>
        /// <param name="salt"></param>
        /// <param name="hashFormat"></param>
        /// <returns></returns>
        private static string HashPassword(string pass, string salt, string hashFormat)
        {
            if (String.IsNullOrEmpty(hashFormat)) return pass;
            HashAlgorithm algorithm = CreateAlgorithm(hashFormat);
            byte[] bytes = Encoding.UTF8.GetBytes(pass);
            byte[] src = Convert.FromBase64String(salt);
            byte[] dst = new byte[src.Length + bytes.Length];
            System.Buffer.BlockCopy(src, 0, dst, 0, src.Length);
            System.Buffer.BlockCopy(bytes, 0, dst, src.Length, bytes.Length);
            return ByteArrayToHexString(algorithm.ComputeHash(dst));
        }

        /// <summary>
        /// 针对密码计算哈希值
        /// </summary>
        /// <param name="pass"></param>
        /// <param name="hashFormat"></param>
        /// <returns></returns>
        private static string HashPassword(string pass, string hashFormat)
        {
            return HashString(pass, hashFormat);
        }

        /// <summary>
        /// 默认密码加密方式为SHA1
        /// </summary>
        /// <param name="pass"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string HashPasswordWithSalt(string pass, string salt)
        {
            return HashPassword(pass, salt, "SHA1");
        }

        /// <summary>
        /// 计算散列值
        /// </summary>
        /// <returns></returns>
        public static string GenerateSalt()
        {
            byte[] data = new byte[0x10];
            new RNGCryptoServiceProvider().GetBytes(data);
            return Convert.ToBase64String(data);
        }
    }
}

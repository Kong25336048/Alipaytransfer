using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CoreHelper
{
    /// <summary>
    /// 加密帮助类
    /// </summary>
    public static class EncryptHelper
    {


        #region 字符串加密

        private const string DefaultDESKey = "loogn789";
        /// <summary>   
        /// 利用DES加密算法加密字符串（可解密）   
        /// </summary>   
        /// <param name="plaintext">被加密的字符串</param>   
        /// <param name="key">密钥（只支持8个字节的密钥）</param>   
        /// <returns>加密后的字符串</returns>   
        public static string EncryptString(string plaintext, string key = DefaultDESKey)
        {
            DES des = new DESCryptoServiceProvider();
            des.Key = Encoding.UTF8.GetBytes(key);
            des.IV = Encoding.UTF8.GetBytes(key);
            byte[] bytes = Encoding.UTF8.GetBytes(plaintext);
            byte[] resultBytes = des.CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length);
            return Convert.ToBase64String(resultBytes);
        }


        /// <summary>   
        /// 利用DES解密算法解密密文（可解密）   
        /// </summary>   
        /// <param name="ciphertext">被解密的字符串</param>   
        /// <param name="key">密钥（只支持8个字节的密钥，同前面的加密密钥相同）</param>   
        /// <returns>返回被解密的字符串</returns>   
        public static string DecryptString(string ciphertext, string key = DefaultDESKey)
        {
            DES des = new DESCryptoServiceProvider();
            des.Key = Encoding.UTF8.GetBytes(key);
            des.IV = Encoding.UTF8.GetBytes(key);
            byte[] bytes = Convert.FromBase64String(ciphertext);
            byte[] resultBytes = des.CreateDecryptor().TransformFinalBlock(bytes, 0, bytes.Length);
            return Encoding.UTF8.GetString(resultBytes);
        }

        #endregion


    }
}

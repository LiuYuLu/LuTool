using System;
using System.Security.Cryptography;
using System.Text;

namespace LuTool
{
    /// <summary>
    /// AES加解密（C#版本）
    /// AES,高级加密标准（英语：Advanced Encryption Standard，缩写：AES），在密码学中又称Rijndael加密法，
    /// 是美国联邦政府采用的一种区块加密标准。这个标准用来替代原先的DES，已经被多方分析且广为全世界所使用。
    /// 严格地说，AES和Rijndael加密法并不完全一样（虽然在实际应用中二者可以互换），
    /// 因为Rijndael加密法可以支持更大范围的区块和密钥长度：AES的区块长度固定为128 比特，
    /// 密钥长度则可以是128，192或256比特；而Rijndael使用的密钥和区块长度可以是32位的整数倍，
    /// 以128位为下限，256比特为上限。包括AES-ECB,AES-CBC,AES-CTR,AES-OFB,AES-CFB
    /// </summary>
    public class AesHelper
    {
        #region 微信小程序 开放数据解密

        /// <summary>
        /// 微信小程序 开放数据解密
        /// AES解密（Base64）
        /// Add by 成长的小猪（Jason.Song） on 2018/10/26
        /// </summary>
        /// <param name="encryptedData">已加密的数据</param>
        /// <param name="sessionKey">解密密钥</param>
        /// <param name="iv">IV偏移量</param>
        /// <returns></returns>
        public static string DecryptForWeChatApplet(string encryptedData, string sessionKey, string iv)
        {
            var decryptBytes = Convert.FromBase64String(encryptedData);
            var keyBytes = Convert.FromBase64String(sessionKey);
            var ivBytes = Convert.FromBase64String(iv);
            var outputBytes = DecryptByAesBytes(decryptBytes, keyBytes, ivBytes);
            return Encoding.UTF8.GetString(outputBytes);
        }

        #endregion 微信小程序 开放数据解密

        #region AES加密

        /// <summary>
        /// AES加密
        /// Add by 成长的小猪（Jason.Song） on 2018/10/26
        /// </summary>
        /// <param name="encryptedBytes">待加密的字节数组</param>
        /// <param name="keyBytes">加密密钥字节数组</param>
        /// <param name="ivBytes">IV初始化向量字节数组</param>
        /// <param name="cipher">运算模式</param>
        /// <param name="padding">填充模式</param>
        /// <returns></returns>
        public static byte[] EncryptToAesBytes(byte[] encryptedBytes, byte[] keyBytes, byte[] ivBytes,
            CipherMode cipher = CipherMode.CBC, PaddingMode padding = PaddingMode.PKCS7)
        {
            if (encryptedBytes == null || encryptedBytes.Length <= 0)
                throw new ArgumentNullException(nameof(encryptedBytes));
            if (keyBytes == null || keyBytes.Length <= 0)
                throw new ArgumentNullException(nameof(keyBytes));
            if (ivBytes == null || ivBytes.Length <= 0)
                throw new ArgumentNullException(nameof(ivBytes));

            var des = new AesCryptoServiceProvider
            {
                Key = keyBytes,
                IV = ivBytes,
                Mode = cipher,
                Padding = padding
            };
            var outputBytes = des.CreateEncryptor().TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
            return outputBytes;
        }

        #endregion AES加密

        #region AES解密

        /// <summary>
        /// AES解密
        /// Add by 成长的小猪（Jason.Song） on 2018/10/26
        /// </summary>
        /// <param name="decryptedBytes">待解密的字节数组</param>
        /// <param name="keyBytes">解密密钥字节数组</param>
        /// <param name="ivBytes">IV初始化向量字节数组</param>
        /// <param name="cipher">运算模式</param>
        /// <param name="padding">填充模式</param>
        /// <returns></returns>
        public static byte[] DecryptByAesBytes(byte[] decryptedBytes, byte[] keyBytes, byte[] ivBytes,
            CipherMode cipher = CipherMode.CBC, PaddingMode padding = PaddingMode.PKCS7)
        {
            if (decryptedBytes == null || decryptedBytes.Length <= 0)
                throw new ArgumentNullException(nameof(decryptedBytes));
            if (keyBytes == null || keyBytes.Length <= 0)
                throw new ArgumentNullException(nameof(keyBytes));
            if (ivBytes == null || ivBytes.Length <= 0)
                throw new ArgumentNullException(nameof(ivBytes));

            var aes = new AesCryptoServiceProvider
            {
                Key = keyBytes,
                IV = ivBytes,
                Mode = cipher,
                Padding = padding
            };
            var outputBytes = aes.CreateDecryptor().TransformFinalBlock(decryptedBytes, 0, decryptedBytes.Length);
            return outputBytes;
        }

        #endregion AES解密
    }
}
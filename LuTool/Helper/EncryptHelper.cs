using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace LuTool
{
    /// <summary>
    /// 安全帮助类，提供SHA-1算法等
    /// </summary>
    public class EncryptHelper
    {
        #region SHA相关

        /// <summary>
        /// 采用SHA-1算法加密字符串（小写）
        /// </summary>
        /// <param name="encypStr">需要加密的字符串</param>
        /// <returns></returns>
        public static string GetSha1(string encypStr)
        {
            var sha1 = SHA1.Create();
            var sha1Arr = sha1.ComputeHash(Encoding.UTF8.GetBytes(encypStr));
            StringBuilder enText = new StringBuilder();
            foreach (var b in sha1Arr)
            {
                enText.AppendFormat("{0:x2}", b);
            }

            return enText.ToString();

            //byte[] strRes = Encoding.Default.GetBytes(encypStr);
            //HashAlgorithm iSHA = new SHA1CryptoServiceProvider();
            //strRes = iSHA.ComputeHash(strRes);
            //StringBuilder enText = new StringBuilder();
            //foreach (byte iByte in strRes)
            //{
            //    enText.AppendFormat("{0:x2}", iByte);
            //}
        }

        /// <summary>
        /// HMAC SHA256 加密
        /// </summary>
        /// <param name="message">加密消息原文。当为小程序SessionKey签名提供服务时，其中message为本次POST请求的数据包（通常为JSON）。特别地，对于GET请求，message等于长度为0的字符串。</param>
        /// <param name="secret">秘钥（如小程序的SessionKey）</param>
        /// <returns></returns>
        public static string GetHmacSha256(string message, string secret)
        {
            message = message ?? "";
            secret = secret ?? "";
            byte[] keyByte = Encoding.UTF8.GetBytes(secret);
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                StringBuilder enText = new StringBuilder();
                foreach (var b in hashmessage)
                {
                    enText.AppendFormat("{0:x2}", b);
                }

                return enText.ToString();
            }
        }

        #endregion SHA相关

        #region MD5

        /// <summary>
        /// 获取大写的MD5签名结果
        /// </summary>
        /// <param name="encypStr">需要加密的字符串</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static string GetMD5(string encypStr, Encoding encoding)
        {
            string retStr;
            MD5 m5 = MD5.Create();

            //创建md5对象
            byte[] inputBye;
            byte[] outputBye;

            //使用指定编码方式把字符串转化为字节数组．
            try
            {
                inputBye = encoding.GetBytes(encypStr);
            }
            catch
            {
                inputBye = Encoding.GetEncoding("utf-8").GetBytes(encypStr);
            }

            outputBye = m5.ComputeHash(inputBye);

            retStr = BitConverter.ToString(outputBye);
            retStr = retStr.Replace("-", "").ToUpper();
            return retStr;
        }

        /// <summary>
        /// 获取大写的MD5签名结果
        /// </summary>
        /// <param name="encypStr">需要加密的字符串</param>
        /// <param name="charset">编码</param>
        /// <returns></returns>
        public static string GetMD5(string encypStr, string charset = "utf-8")
        {
            charset = charset ?? "utf-8";
            try
            {
                //使用指定编码
                return GetMD5(encypStr, Encoding.GetEncoding(charset));
            }
            catch (Exception ex)
            {
                //使用UTF-8编码
                return GetMD5("utf-8", Encoding.GetEncoding(charset));

                //#if NET35 || NET40 || NET45
                //                inputBye = Encoding.GetEncoding("GB2312").GetBytes(encypStr);
                //#else
                //                inputBye = Encoding.GetEncoding(936).GetBytes(encypStr);
                //#endif
            }
        }

        /// <summary>
        /// 获取小写的MD5签名结果
        /// </summary>
        /// <param name="encypStr">需要加密的字符串</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static string GetLowerMD5(string encypStr, Encoding encoding)
        {
            return GetMD5(encypStr, encoding).ToLower();
        }

        #endregion MD5

        #region AES

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="inputdata">输入的数据</param>
        /// <param name="iv">向量</param>
        /// <param name="strKey">加密密钥</param>
        /// <returns></returns>
        public static byte[] AESEncrypt(byte[] inputdata, byte[] iv, string strKey)
        {
            //分组加密算法
            SymmetricAlgorithm des = Aes.Create();

            byte[] inputByteArray = inputdata; //得到需要加密的字节数组
            des.Key = Encoding.UTF8.GetBytes(strKey.Substring(0, 32)); //设置密钥及密钥向量
            des.IV = iv;
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    byte[] cipherBytes = ms.ToArray(); //得到加密后的字节数组
                    //cs.Close();
                    //ms.Close();
                    return cipherBytes;
                }
            }
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="inputdata">输入的数据</param>
        /// <param name="iv">向量</param>
        /// <param name="strKey">key</param>
        /// <returns></returns>
        public static byte[] AESDecrypt(byte[] inputdata, byte[] iv, byte[] strKey)
        {
            SymmetricAlgorithm des = Aes.Create();

            des.Key = strKey; //Encoding.UTF8.GetBytes(strKey);//.Substring(0, 7)
            des.IV = iv;
            byte[] decryptBytes = new byte[inputdata.Length];
            using (MemoryStream ms = new MemoryStream(inputdata))
            {
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    cs.Read(decryptBytes, 0, decryptBytes.Length);
                    //cs.Close();
                    //ms.Close();
                }
            }

            return decryptBytes;
        }

        /// <summary>
        /// AES解密(无向量)
        /// </summary>
        /// <param name="data">被加密的明文</param>
        /// <param name="key">密钥</param>
        /// <returns>明文</returns>
        public static string AESDecrypt(String data, String key)
        {
            Byte[] encryptedBytes = Convert.FromBase64String(data);
            Byte[] bKey = new Byte[32];
            Array.Copy(Encoding.UTF8.GetBytes(key.PadRight(bKey.Length)), bKey, bKey.Length);

            MemoryStream mStream = new MemoryStream(encryptedBytes);
            //mStream.Write( encryptedBytes, 0, encryptedBytes.Length );
            //mStream.Seek( 0, SeekOrigin.Begin );

            //RijndaelManaged aes = new RijndaelManaged();
            SymmetricAlgorithm aes = Aes.Create();

            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            aes.KeySize = 128;
            aes.Key = bKey;
            //aes.IV = _iV;
            CryptoStream cryptoStream = new CryptoStream(mStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
            try
            {
                byte[] tmp = new byte[encryptedBytes.Length + 32];
                int len = cryptoStream.Read(tmp, 0, encryptedBytes.Length + 32);
                byte[] ret = new byte[len];
                Array.Copy(tmp, 0, ret, 0, len);
                return Encoding.UTF8.GetString(ret);
            }
            finally
            {
#if NET35 || NET40 || NET45
                cryptoStream.Close();
                mStream.Close();
                aes.Clear();
#else
                //cryptoStream.();
                //mStream.Close();
                //aes.Clear();
#endif
            }
        }

        #endregion AES

        #region Base64

        /// <summary>
        /// Base64加密，采用utf8编码方式加密
        /// </summary>
        /// <param name="source">待加密的明文</param>
        /// <param name="codeType">加密采用的编码方式</param>
        /// <returns>加密后的字符串</returns>
        public static string Base64Encode(string source, string codeType = "utf-8")
        {
            return Base64Encode(source, Encoding.GetEncoding(codeType));
        }

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="source">待加密的明文</param>
        /// <param name="encodeType">加密采用的编码方式</param>
        /// <returns></returns>
        public static string Base64Encode(string source, Encoding encodeType)
        {
            string encode = string.Empty;
            byte[] bytes = encodeType.GetBytes(source);
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = source;
            }

            return encode;
        }

        /// <summary>
        /// Base64解密，采用utf8编码方式解密
        /// </summary>
        /// <param name="result">待解密的密文</param>
        /// <param name="codeType">解密采用的编码方式</param>
        /// <returns>解密后的字符串</returns>
        public static string Base64Decode(string result, string codeType = "utf-8")
        {
            return Base64Decode(result, Encoding.GetEncoding(codeType));
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="result">待解密的密文</param>
        /// <param name="encodeType">解密采用的编码方式，注意和加密时采用的方式一致</param>
        /// <returns>解密后的字符串</returns>
        public static string Base64Decode(string result, Encoding encodeType)
        {
            string decode = string.Empty;
            byte[] bytes = Convert.FromBase64String(result);
            try
            {
                decode = encodeType.GetString(bytes);
            }
            catch
            {
                decode = result;
            }

            return decode;
        }

        #endregion

        #region Gzip

        /// <summary>
        /// 字符串采用Gzip压缩，并使用base64编码
        /// </summary>
        /// <param name="str"></param>
        /// <param name="strType"></param>
        /// <returns></returns>
        public static string GzipCompress(string str, string strType = "UTF-8")
        {
            var compressBeforeByte = Encoding.GetEncoding(strType).GetBytes(str);
            var compressAfterByte = GzipCompress(compressBeforeByte);
            return Convert.ToBase64String(compressAfterByte);
        }

        /// <summary>
        /// 字节数组使用gzip压缩
        /// </summary>
        /// <param name="data">字节数组</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static byte[] GzipCompress(byte[] data)
        {
            try
            {
                var ms = new MemoryStream();
                var zip = new GZipStream(ms, CompressionMode.Compress, true);
                zip.Write(data, 0, data.Length);
                zip.Close();
                var buffer = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(buffer, 0, buffer.Length);
                ms.Close();
                return buffer;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Base64字符串使用Gzip解压缩
        /// </summary>
        /// <param name="base64Str"></param>
        /// <param name="strType"></param>
        /// <returns></returns>
        public static string GzipDecompress(string base64Str, string strType = "UTF-8")
        {
            var compressBeforeByte = Convert.FromBase64String(base64Str);
            var compressAfterByte = GzipDecompress(compressBeforeByte);
            return Encoding.GetEncoding(strType).GetString(compressAfterByte);
        }

        /// <summary>
        /// 字节数组使用gzip解压缩
        /// </summary>
        /// <param name="data">字节数组</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static byte[] GzipDecompress(byte[] data)
        {
            try
            {
                var ms = new MemoryStream(data);
                var zip = new GZipStream(ms, CompressionMode.Decompress, true);
                var msReader = new MemoryStream();
                var buffer = new byte[0x1000];
                while (true)
                {
                    var reader = zip.Read(buffer, 0, buffer.Length);
                    if (reader <= 0)
                    {
                        break;
                    }

                    msReader.Write(buffer, 0, reader);
                }

                zip.Close();
                ms.Close();
                msReader.Position = 0;
                buffer = msReader.ToArray();
                msReader.Close();
                return buffer;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        #endregion
    }
}
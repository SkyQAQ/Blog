using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Linq;
using System.Web;
using System.IO;
using System.Net;

namespace Blog.Core.Common
{
    public static class WuYao
    {
        private const string _charset = "utf-8";

        #region 时间戳
        /// <summary>
        ///转时间戳
        /// </summary>
        public static string ConvertTimeStamp(DateTime time)
        {
            DateTime dtStart = new DateTime(1970, 1, 1).AddHours(8);
            TimeSpan toNow = time.Subtract(dtStart);
            string timeStamp = toNow.Ticks.ToString();
            timeStamp = timeStamp.Substring(0, timeStamp.Length - 7);
            return timeStamp;
        }

        /// <summary>
        /// 转时间戳（精确到毫秒）
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ConvertMilliTimeStamp(DateTime time)
        {
            DateTime dtStart = new DateTime(1970, 1, 1).AddHours(8);
            TimeSpan toNow = time.Subtract(dtStart);
            string timeStamp = toNow.Ticks.ToString();
            timeStamp = timeStamp.Substring(0, timeStamp.Length - 4);
            return timeStamp;
        }

        /// <summary>
        /// 将时间戳转换为一般时间格式
        /// </summary>
        /// <param name="now">时间戳(到日期)</param>
        /// <returns></returns>
        public static DateTime GetNoralTime(string now)
        {
            string timeStamp = now;
            DateTime dtStart = new DateTime(1970, 1, 1).AddHours(8);
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            DateTime dtResult = dtStart.Add(toNow);
            return dtResult;
        }
        #endregion

        #region HTTP请求
        /// <summary>
        /// 获取请求参数
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="charSet"></param>
        /// <returns></returns>
        public static string BuildEncodeUrl(IDictionary<string, string> dict, string charSet = _charset)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var param in dict.OrderBy(t => t.Key))
            {
                string encode = HttpUtility.UrlEncode(param.Value, Encoding.GetEncoding(charSet));
                sb.Append(param.Key).Append("=").Append(encode).Append("&");
            }
            return sb.ToString().Substring(0, sb.ToString().LastIndexOf("&"));
        }

        /// <summary>
        /// 获取参数签名
        /// </summary>
        /// <param name="parameters">参数列表</param>
        /// <param name="url">请求地址</param>
        /// <param name="secret">密钥</param>
        /// <returns></returns>
        public static string GetSign(IDictionary<string, string> parameters, string url, string secret)
        {
            var builder = new StringBuilder();
            builder.Append(url);
            builder.Append("\n");
            foreach (var param in parameters.OrderBy(t => t.Key))
            {
                builder.Append(string.Format("{0}={1}\n", param.Key, param.Value));
            }
            builder.Append(secret);
            return GetMd5(builder.ToString());
        }

        /// <summary>
        /// 获取IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetIpAddress()
        {
            string AddressIP = string.Empty;
            foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    AddressIP = _IPAddress.ToString();
                }
            }
            return AddressIP;
        }
        #endregion

        #region 数据加解密
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="plainText">明文</param>
        /// <returns>string</returns>
        public static string GetMd5(string plainText)
        {
            var buffer = new StringBuilder();
            using (var md5 = MD5.Create())
            {
                var md5Bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(plainText));
                for (var i = 0; i < md5Bytes.Length; i++)
                {
                    var val = Convert.ToInt32(md5Bytes[i] & 0xff);
                    if (val < 16)
                    {
                        buffer.Append("0");
                    }
                    buffer.Append(string.Format("{0:X}", val));
                }
            }
            return buffer.ToString();
        }

        /// <summary>
        /// AES-256加密
        /// </summary>
        /// <param name="plainText">明文</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">向量</param>
        /// <returns>string</returns>
        public static string AesEncrypt(string plainText, string key = "", string iv = "", string charSet = _charset)
        {
            if (string.IsNullOrWhiteSpace(plainText))
            {
                return string.Empty;
            }
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(iv))
            {
                ConfigHelper config = new ConfigHelper();
                key = config.AES_Key;
                iv = config.AES_Iv;
            }
            byte[] bText = Encoding.GetEncoding(charSet).GetBytes(plainText);
            byte[] bKey = Encoding.GetEncoding(charSet).GetBytes(key);
            byte[] bIv = Encoding.GetEncoding(charSet).GetBytes(iv);
            using (Rijndael rijAlg = Rijndael.Create())
            {
                rijAlg.Key = bKey;
                rijAlg.IV = bIv;
                // 获取或设置对称算法所用密钥的大小（以位为单位）
                rijAlg.KeySize = 256;
                // 获取或设置加密操作的块大小（以位为单位）
                rijAlg.BlockSize = 128;
                // 获取或设置对称算法的运算模式
                rijAlg.Mode = CipherMode.CBC;
                // 获取或设置对称算法中使用的填充模式
                rijAlg.Padding = PaddingMode.PKCS7;
                // 创建一个加密器来执行流变换
                using (ICryptoTransform transform = rijAlg.CreateEncryptor(bKey, bIv))
                {
                    return Convert.ToBase64String(transform.TransformFinalBlock(bText, 0, bText.Length));
                }
            }
        }

        /// <summary>
        /// AES-256解密
        /// </summary>
        /// <param name="plainText">密文</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">向量</param>
        /// <returns>string</returns>
        public static string AesDecrypt(string cipherText, string key = "", string iv = "", string charSet = _charset)
        {
            if (string.IsNullOrWhiteSpace(cipherText))
            {
                return string.Empty;
            }
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(iv))
            {
                ConfigHelper config = new ConfigHelper();
                key = config.AES_Key;
                iv = config.AES_Iv;
            }
            byte[] bText = Convert.FromBase64String (cipherText);
            byte[] bKey = Encoding.GetEncoding(charSet).GetBytes(key);
            byte[] bIv = Encoding.GetEncoding(charSet).GetBytes(iv);
            using (Rijndael rijAlg = Rijndael.Create())
            {
                rijAlg.Key = bKey;
                rijAlg.IV = bIv;
                // 获取或设置对称算法所用密钥的大小（以位为单位）
                rijAlg.KeySize = 256;
                // 获取或设置加密操作的块大小（以位为单位）
                rijAlg.BlockSize = 128;
                // 获取或设置对称算法的运算模式
                rijAlg.Mode = CipherMode.CBC;
                // 获取或设置对称算法中使用的填充模式
                rijAlg.Padding = PaddingMode.PKCS7;
                // 创建一个加密器来执行流变换
                using (ICryptoTransform transform = rijAlg.CreateDecryptor(bKey, bIv))
                {
                    return Encoding.GetEncoding(charSet).GetString(transform.TransformFinalBlock(bText, 0, bText.Length));
                }
            }
        }

        /// <summary>
        /// Rsa加密
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="publicKey"></param>
        /// <param name="charSet"></param>
        /// <returns></returns>
        public static string RsaEncrypt(string plainText, string publicKey = "", string charSet = _charset)
        {
            ConfigHelper config = new ConfigHelper();
            if (string.IsNullOrWhiteSpace(plainText))
            {
                return string.Empty;
            }
            if (string.IsNullOrEmpty(publicKey))
            {
                publicKey = config.Public_Key;
            }
            using (var rsaProvider = CreateRsaFromPublicKey(config.Public_Key))
            {
                var inputBytes = Encoding.GetEncoding(charSet).GetBytes(plainText);
                var encryptedBytes = rsaProvider.Encrypt(inputBytes, RSAEncryptionPadding.Pkcs1);
                return Convert.ToBase64String(encryptedBytes.ToArray());
            }
        }

        /// <summary>
        /// Rsa解密
        /// </summary>
        /// <param name="cipherText"></param>
        /// <param name="praviteKey"></param>
        /// <param name="charSet"></param>
        /// <returns></returns>
        public static string RsaDecrypt(string cipherText, string praviteKey = "", string charSet = _charset)
        {
            ConfigHelper config = new ConfigHelper();
            if (string.IsNullOrWhiteSpace(cipherText))
            {
                return string.Empty;
            }
            if (string.IsNullOrEmpty(praviteKey))
            {
                praviteKey = config.Private_Key;
            }
            using (var rsaProvider = CreateRsaFromPrivateKey(config.Private_Key))
            {
                var inputBytes = Convert.FromBase64String(cipherText);
                var decryptedBytes = rsaProvider.Decrypt(inputBytes, RSAEncryptionPadding.Pkcs1);
                return Encoding.GetEncoding(charSet).GetString(decryptedBytes);
            }
        }
        
        /// <summary>
        /// 获取MD5加密密码（前台Rsa加密密码）
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static string GetPasswordCipher(string pwd)
        {
            return GetMd5(RsaDecrypt(pwd) + Constants.PasswordSalt);
        }
        #endregion

        #region 帮助方法
        private static RSA CreateRsaFromPrivateKey(string privateKey)
        {
            var privateKeyBits = System.Convert.FromBase64String(privateKey);
            var rsa = RSA.Create();
            var RSAparams = new RSAParameters();

            using (var binr = new BinaryReader(new MemoryStream(privateKeyBits)))
            {
                byte bt = 0;
                ushort twobytes = 0;
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130)
                    binr.ReadByte();
                else if (twobytes == 0x8230)
                    binr.ReadInt16();
                else
                    throw new Exception("Unexpected value read binr.ReadUInt16()");

                twobytes = binr.ReadUInt16();
                if (twobytes != 0x0102)
                    throw new Exception("Unexpected version");

                bt = binr.ReadByte();
                if (bt != 0x00)
                    throw new Exception("Unexpected value read binr.ReadByte()");

                RSAparams.Modulus = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.Exponent = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.D = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.P = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.Q = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.DP = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.DQ = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.InverseQ = binr.ReadBytes(GetIntegerSize(binr));
            }

            rsa.ImportParameters(RSAparams);
            return rsa;
        }

        private static int GetIntegerSize(BinaryReader binr)
        {
            byte bt = 0;
            byte lowbyte = 0x00;
            byte highbyte = 0x00;
            int count = 0;
            bt = binr.ReadByte();
            if (bt != 0x02)
                return 0;
            bt = binr.ReadByte();

            if (bt == 0x81)
                count = binr.ReadByte();
            else
                if (bt == 0x82)
            {
                highbyte = binr.ReadByte();
                lowbyte = binr.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt;
            }

            while (binr.ReadByte() == 0x00)
            {
                count -= 1;
            }
            binr.BaseStream.Seek(-1, SeekOrigin.Current);
            return count;
        }

        private static RSA CreateRsaFromPublicKey(string publicKeyString)
        {
            byte[] SeqOID = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
            byte[] x509key;
            byte[] seq = new byte[15];
            int x509size;

            x509key = Convert.FromBase64String(publicKeyString);
            x509size = x509key.Length;

            using (var mem = new MemoryStream(x509key))
            {
                using (var binr = new BinaryReader(mem))
                {
                    byte bt = 0;
                    ushort twobytes = 0;

                    twobytes = binr.ReadUInt16();
                    if (twobytes == 0x8130)
                        binr.ReadByte();
                    else if (twobytes == 0x8230)
                        binr.ReadInt16();
                    else
                        return null;

                    seq = binr.ReadBytes(15);
                    if (!CompareBytearrays(seq, SeqOID))
                        return null;

                    twobytes = binr.ReadUInt16();
                    if (twobytes == 0x8103)
                        binr.ReadByte();
                    else if (twobytes == 0x8203)
                        binr.ReadInt16();
                    else
                        return null;

                    bt = binr.ReadByte();
                    if (bt != 0x00)
                        return null;

                    twobytes = binr.ReadUInt16();
                    if (twobytes == 0x8130)
                        binr.ReadByte();
                    else if (twobytes == 0x8230)
                        binr.ReadInt16();
                    else
                        return null;

                    twobytes = binr.ReadUInt16();
                    byte lowbyte = 0x00;
                    byte highbyte = 0x00;

                    if (twobytes == 0x8102)
                        lowbyte = binr.ReadByte();
                    else if (twobytes == 0x8202)
                    {
                        highbyte = binr.ReadByte();
                        lowbyte = binr.ReadByte();
                    }
                    else
                        return null;
                    byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                    int modsize = BitConverter.ToInt32(modint, 0);

                    int firstbyte = binr.PeekChar();
                    if (firstbyte == 0x00)
                    {
                        binr.ReadByte();
                        modsize -= 1;
                    }

                    byte[] modulus = binr.ReadBytes(modsize);

                    if (binr.ReadByte() != 0x02)
                        return null;
                    int expbytes = (int)binr.ReadByte();
                    byte[] exponent = binr.ReadBytes(expbytes);

                    var rsa = RSA.Create();
                    var rsaKeyInfo = new RSAParameters
                    {
                        Modulus = modulus,
                        Exponent = exponent
                    };
                    rsa.ImportParameters(rsaKeyInfo);
                    return rsa;
                }

            }
        }

        private static bool CompareBytearrays(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;
            int i = 0;
            foreach (byte c in a)
            {
                if (c != b[i])
                    return false;
                i++;
            }
            return true;
        }
        #endregion

        #region 判断接口、类是否被继承,如基础则实例化第一个子类
        /// <summary>
        /// 判断接口、类是否被继承,如基础则实例化第一个子类
        /// </summary>
        /// <param name="main">父类</param>
        /// <returns></returns>
        public static object GetSubClass(Type main)
        {
            Type sub = null;
            bool isInherited = false;
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (main.IsAssignableFrom(type))
                    {
                        if (type.IsClass && !type.IsAbstract)
                        {
                            sub = type;
                            isInherited = true;
                            break;
                        }
                    }
                }
                if (isInherited)
                {
                    break;
                }
            }
            if (sub == null) { return null; }
            return Activator.CreateInstance(sub);
        }
        #endregion
    }
}

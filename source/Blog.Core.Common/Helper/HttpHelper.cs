using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Net.Security;
using System.Net.Cache;
using System.Security.Cryptography;
using System.Linq;
using System.Web;
using System.IO;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;

namespace Blog.Core.Common
{
    /// <summary>
    /// HTTP帮助类
    /// </summary>
    public static class HttpHelper
    {
        #region HTTP请求

        /// <summary>
        /// 日志帮助类
        /// </summary>
        private static readonly LogHelper _log = new LogHelper();

        /// <summary>
        /// HttpGet方法
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="requestString">请求数据</param>
        /// <param name="encoding">编码方式</param>
        /// <param name="seconds">请求超时时间：秒</param>
        /// <param name="headers">头部数据</param>
        /// <param name="isHttps">是否Https请求</param>
        /// <returns>string</returns>
        public static string Get(string url, IDictionary<string, string> param = null, Encoding encoding = null, int seconds = 30, IDictionary<string, string> headers = null)
            => Request("Get", url + "?" + BuildParams(param), "", "", encoding, seconds, headers);

        /// <summary>
        /// HttpPost方法
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="requestString">请求数据</param>
        /// <param name="encoding">编码方式</param>
        /// <param name="seconds">请求超时时间：秒</param>
        /// <param name="headers">头部数据</param>
        /// <param name="isHttps">是否Https请求</param>
        /// <returns>string</returns>
        public static string Post(string url, IDictionary<string, string> param, string contentType = Constants.ContentType1, Encoding encoding = null, int seconds = 30, IDictionary<string, string> headers = null)
            => Request("Post", url, BuildParams(param), contentType, encoding, seconds, headers);

        /// <summary>
        /// 请求方法
        /// </summary>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="requestString"></param>
        /// <param name="contentType"></param>
        /// <param name="encoding"></param>
        /// <param name="seconds"></param>
        /// <param name="headers"></param>
        /// <param name="isHttps"></param>
        /// <returns></returns>
        private static string Request(string method, string url, string requestString, string contentType, Encoding encoding, int seconds = 30, IDictionary<string,string> headers = null)
        {
            try
            {
                if (string.IsNullOrEmpty(method))
                    throw new Exception("请求方式不能为空！");
                if (string.IsNullOrEmpty(url))
                    throw new Exception("请求地址不能为空！");
                string result = string.Empty;
                if (url.ToLower().StartsWith("https"))
                {
                    ServicePointManager.ServerCertificateValidationCallback += (s, cert, chain, sslPolicyErrors) => true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                }
                if (encoding == null)
                    encoding = Encoding.UTF8;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = method;
                #region 一些属性
                // HTTP 标头的值。 默认值为 null
                // request.Headers.Add("Accept", "image/*");

                // 根据原地址、重定向地址盘点请求过程中URL是否更改
                // bool isUrlChanged = request.RequestUri != request.Address;

                // Connection设置用来保持连接或关闭
                // request.Connection = "Close";

                // ConnectionGroupName属性使您能够将请求与连接组相关联。 当你的应用程序发出请求到一台服务器对于不同的用户，例如从数据库服务器中检索客户信息的网站时，这很有用。
                // Create a secure group name.
                // SHA1Managed Sha1 = new SHA1Managed();
                // Byte[] updHash = Sha1.ComputeHash(Encoding.UTF8.GetBytes("username" + "password" + "domain"));
                // string secureGroupName = Encoding.Default.GetString(updHash);
                // Set the authentication credentials for the request.      
                // request.Credentials = new NetworkCredential("username", "password", "domain");
                // request.ConnectionGroupName = secureGroupName;
                #endregion
                request.Credentials = CredentialCache.DefaultCredentials;
                request.ReadWriteTimeout = seconds * 1000;
                request.Timeout = seconds * 1000;
                request.UserAgent = "WUYAO";
                // 不需要根据http协议版本匹配，不需要进行协议握手就是说
                // 100-Continue的作用是,设定Client 和 Server在Post数据前需要进行“请求头域”的数据匹配,相当于是握手。如果匹配则开始进行body 的内容，Post数据。否则，报错（417） Unkown
                if (method.ToLower() == "post")
                    request.ServicePoint.Expect100Continue = false;
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        if (header.Key.Replace("-", "").ToLower() == "contenttype")
                            request.ContentType = header.Value;
                        else
                            request.Headers.Add(header.Key, header.Value);
                    }
                }
                if (!string.IsNullOrEmpty(requestString))
                {
                    byte[] data = encoding.GetBytes(requestString);
                    request.ContentLength = data.Length;
                    request.ContentType = contentType;
                    using (Stream stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                        stream.Close();
                    }
                }
                using(HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), encoding);
                    result = reader.ReadToEnd();
                }
                return result;
            }
            catch (WebException ex)
            {
                _log.Error(string.Format("[{0}]<{1}>{2}", url, requestString, ex.Message, ex));
                throw ex;
            }
        }

        #endregion

        #region 获取云收订签名

        /// <summary>
        /// 签名-云收订
        /// </summary>
        /// <param name="method">GET | POST</param>
        /// <param name="path">/path/to/method</param>
        /// <param name="header">EncodeRFC3986(HeaderKey1 + HeaderValue1 + HeaderKey2 + HeaderValue2 ...)</param>
        /// <param name="getParam">EncodeRFC3986(GetKey1=GetValue1&GetKey2=GetValue2 ...)</param>
        /// <param name="postParam">EncodeRFC3986(GetKey1=GetValue1&GetKey2=GetValue2 ...)</param>
        /// <param name="secret">秘钥</param>
        /// <returns></returns>
        public static string SignCloudSales(string method, string path, IDictionary<string, string> header, IDictionary<string, string> getParam, IDictionary<string, string> postParam, string secret)
        {
            try
            {
                List<string> d = new List<string>();
                d.Add(secret);
                d.Add(method);
                d.Add(WebUtility.UrlEncode(path));
                d.Add(EncodeRFC3986(BuildHeaders(header)));
                d.Add(EncodeRFC3986(BuildParams(getParam)));
                d.Add(EncodeRFC3986(BuildParams(postParam)));
                d.Add(secret);
                string plainText = string.Join("&", d);
                return GetMd5(plainText).ToUpper();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="plainText">明文</param>
        /// <returns>string</returns>
        public static string GetMd5(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return string.Empty;
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
        /// 构建请求参数
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static string BuildParams(IDictionary<string, string> dict)
        {
            if (dict == null)
                return string.Empty;
            StringBuilder sb = new StringBuilder();
            foreach (var param in dict.OrderBy(t => t.Key))
            {
                sb.Append(param.Key).Append("=").Append(param.Value).Append("&");
            }
            return sb.ToString().Substring(0, sb.ToString().LastIndexOf("&"));
        }

        /// <summary>
        /// 构建请求头
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        private static string BuildHeaders(IDictionary<string, string> dict)
        {
            if (dict == null)
                return string.Empty;
            StringBuilder sb = new StringBuilder();
            foreach (var param in dict.OrderBy(t => t.Key))
            {
                sb.Append(param.Key).Append("+").Append(param.Value).Append("+");
            }
            return sb.ToString().Substring(0, sb.ToString().LastIndexOf("+"));
        }

        #endregion

        #region RFC3986编码

        /// <summary>
        /// RFC3986编码
        /// 有效字符 = 字母 / 数字 / "-" / "." / "_" / "~"
        /// "!" -> %21
        /// "*" -> %2A
        /// "(" -> %28
        /// ")" -> %29
        /// " " -> %20
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static string EncodeRFC3986(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;
            StringBuilder newBytes = new StringBuilder();
            var urf8Bytes = Encoding.UTF8.GetBytes(input);
            foreach (var item in urf8Bytes)
            {
                if (IsReverseChar((char)item))
                {
                    newBytes.Append('%');
                    newBytes.Append(item.ToString("X2"));
                }
                else
                {
                    newBytes.Append((char)item);
                }
            }
            return newBytes.ToString();
        }

        /// <summary>
        /// 是否需要转换字符    
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private static bool IsReverseChar(char c)
        {
            return !((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9')
                    || c == '-' || c == '_' || c == '.' || c == '~');
        }

        /// 读取队列订单获取SIGN例子
        /// Dictionary<string, string> param = new Dictionary<string, string>
        /// {
        ///     { "method", _config.CLOUDSALES_METHOD_READQUEUE
        ///
        ///     { "app_key", _config.CLOUDSALES_APP_KEY
        ///
        ///     { "sign_time", HttpHelper.ConvertTimeStamp(DateTime.UtcNow.AddHours(8)) },
        ///     { "sign_method", "md5" },
        ///     { "topic", _config.CLOUDSALES_QUEUE_ORDERS },
        ///     { "drop", _config.CLOUDSALES_IS_DROP },
        ///     { "num", _config.CLOUDSALES_ORDERS_NUM },
        /// };
        /// string sign = HttpHelper.SignCloudSales("POST", "/router", null, null, param, _config.CLOUDSALES_CLIENT_SECRET);

        #endregion
    }
}

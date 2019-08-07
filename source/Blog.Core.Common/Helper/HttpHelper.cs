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
        /// <summary>
        /// 日志帮助类
        /// </summary>
        private static readonly LogHelper _log = new LogHelper();

        #region HTTP请求
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
        public static string Get(string url, string requestString, Encoding encoding, int seconds = 30, IDictionary<string, string> headers = null, bool isHttps = false)
            => Request("Get", url, requestString, encoding, seconds, headers, isHttps);

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
        public static string Post(string url, string requestString, Encoding encoding, int seconds = 30, IDictionary<string, string> headers = null, bool isHttps = false)
            => Request("Post", url, requestString, encoding, seconds, headers, isHttps);

        /// <summary>
        /// 请求方法
        /// </summary>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="requestString"></param>
        /// <param name="encoding"></param>
        /// <param name="seconds"></param>
        /// <param name="headers"></param>
        /// <param name="isHttps"></param>
        /// <returns></returns>
        private static string Request(string method, string url, string requestString, Encoding encoding, int seconds = 30, IDictionary<string,string> headers = null, bool isHttps = false)
        {
            try
            {
                string result = string.Empty;
                if (isHttps)
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
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
                request.Headers.Add("ContentType ", Constants.ContentType1);
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
    }
}

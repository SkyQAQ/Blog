using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Helper.Tool
{
    /// <summary>
    /// 加解密工具类
    /// </summary>
    public static class CipherUtil
    {
        /// <summary>
        /// MD5密码
        /// </summary>
        /// <param name="plaintext"></param>
        /// <returns></returns>
        public static string EncryptMD5(string plaintext)
        {
            try
            {
                if (plaintext.IsNullOrWhiteSpace())
                    return string.Empty;

            }
            catch (Exception ex)
            {
                throw new Exception("MD5加密失败：" + plaintext);
            }
        }
    }
}

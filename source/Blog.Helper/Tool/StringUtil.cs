using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Blog.Helper.Tool
{
    /// <summary>
    /// 字符串帮助类
    /// </summary>
    public static class StringUtil
    {

    }

    /// <summary>
    /// 字符串扩展类
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// 转字符数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static char[] ToChar(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return null;
            char[] result = new char [str.Length];
            for (int i = 0; i < str.Length; i++)
            {
                result[i] = str[i];
            }
            return result;
        }

        /// <summary>
        /// 是否为空、空字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// 是否为空、空字符串、空格
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// 是否数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumber(this string str)
        {
            Regex regex = new Regex("[0-9]");
            return regex.IsMatch(str);
        }

        /// <summary>
        /// 正则表达式验证
        /// </summary>
        /// <param name="str"></param>
        /// <param name="rule">验证规则</param>
        /// <returns></returns>
        public static bool Regex(this string str, string rule)
        {
            if (str.IsNullOrWhiteSpace())
                return false;
            Regex regex = new Regex(rule);
            return regex.IsMatch(str);
        }
    }
}
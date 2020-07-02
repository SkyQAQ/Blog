using System;

namespace Blog.Helper.Tool
{
    /// <summary>
    /// 转换类型工具
    /// </summary>
    public static class CastUtil
    {
        /// <summary>
        /// 转换Bool
        /// 1或true是真
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ToBoolean(object value)
        {
            if (((value == null) || (value is DBNull)) || (value.ToString().Trim() == ""))
            {
                return false;
            }
            try
            {
                if (value.ToString() == "1")
                    return true;
                return Convert.ToBoolean(value);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 转换时间
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(object date)
        {
            if ((date == null) || (date is DBNull))
            {
                return new DateTime();
            }
            if (date is DateTime)
            {
                return (DateTime)date;
            }
            try
            {
                return Convert.ToDateTime(date.ToString());
            }
            catch
            {
                return new DateTime();
            }
        }

        /// <summary>
        /// 空时间
        /// 0001/1/1 0:00:00
        /// </summary>
        public static DateTime NullDateTime
        {
            get
            {
                return DateTime.Parse("0001/1/1 0:00:00");
            }
        }

        /// <summary>
        /// 转换时间?
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime? ToDateTimeNull(object date)
        {
            if ((date == null) || (date is DBNull))
            {
                return null;
            }
            if (date is DateTime)
            {
                return new DateTime?((DateTime)date);
            }
            try
            {
                return new DateTime?(Convert.ToDateTime(date.ToString()));
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 转换十进制数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal ToDecimal(object value)
        {
            if (((value == null) || (value is DBNull)) || (value.ToString().Trim() == ""))
            {
                return decimal.Zero;
            }
            try
            {
                return Convert.ToDecimal(value);
            }
            catch (Exception)
            {
                throw new Exception("转换十进制数失败：" + value.ToString());
            }
        }

        /// <summary>
        /// 转换浮点数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double ToDouble(object value)
        {
            if (((value == null) || (value is DBNull)) || (value.ToString().Trim() == ""))
            {
                return 0.0;
            }
            try
            {
                return Convert.ToDouble(value);
            }
            catch (Exception)
            {
                throw new Exception("转换浮点数失败：" + value.ToString());
            }
        }

        /// <summary>
        /// 转换长整型数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long ToLong(object value)
        {
            if (((value == null) || (value is DBNull)) || (value.ToString().Trim() == ""))
            {
                return 0;
            }
            try
            {
                return Convert.ToInt64(value);
            }
            catch (Exception)
            {
                throw new Exception("转换长整型数失败：" + value.ToString());
            }
        }

        /// <summary>
        /// 转换整数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt(object value)
        {
            if (((value == null) || (value is DBNull)) || (value.ToString().Trim() == ""))
            {
                return 0;
            }
            if (value is int)
            {
                return (int)value;
            }
            try
            {
                return Convert.ToInt32(value);
            }
            catch (Exception)
            {
                throw new Exception("转换整数失败：" + value.ToString());
            }
        }

        /// <summary>
        /// 转换字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToString(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return "";
            }
            return value.ToString();
        }

        /// <summary>
        /// 转换Base64
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToBase64String(byte[] value)
        {
            if (value == null || value.Length == 0)
            {
                return "";
            }
            return Convert.ToBase64String(value);
        }

        /// <summary>
        /// 转换时间字符串
        /// yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDateTimeString(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return "";
            }
            DateTime dateTime = ToDateTime(value);
            if (dateTime == new DateTime())
            {
                return "";
            }
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 转换日期字符串
        /// yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDateString(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return "";
            }
            DateTime dateTime = ToDateTime(value);
            if (dateTime == new DateTime())
            {
                return "";
            }
            return dateTime.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 转换SAP时间
        /// 20200331 --> 2020-03-31
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static DateTime ToSAPDateTime(object val)
        {
            if ((val == null) || (val is DBNull))
            {
                return new DateTime();
            }
            string str = val.ToString();
            if (str.Length == 8)
            {
                string[] textArray1 = new string[] { str.Substring(0, 4), "-", str.Substring(4, 2), "-", str.Substring(6, 2) };
                return Convert.ToDateTime(string.Concat(textArray1));
            }
            return new DateTime();
        }
        
        /// <summary>
        /// 转换日期
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime ToDate(object date)
        {
            if ((date == null) || (date is DBNull))
            {
                return new DateTime();
            }
            if (date is DateTime)
            {
                return (DateTime)date;
            }
            return ToDate(date.ToString());
        }

        /// <summary>
        /// 转换日期
        /// 2020-03-31
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime ToDate(string date) =>
            ToDate(date, '-', "ymd");

        /// <summary>
        /// 转换日期
        /// 2020-03-31 token:- formats:ymd
        /// </summary>
        /// <param name="date"></param>
        /// <param name="token"></param>
        /// <param name="formats"></param>
        /// <returns></returns>
        public static DateTime ToDate(string date, char token, string formats)
        {
            if (string.IsNullOrEmpty(date))
            {
                return new DateTime();
            }
            if (((string.IsNullOrEmpty(formats) || (formats.Length != 3)) || ((formats.ToLower().IndexOf('y') < 0) || (formats.ToLower().IndexOf('m') < 0))) || (formats.ToLower().IndexOf('d') < 0))
            {
                formats = "ymd";
            }
            if (date.Length > 10)
            {
                date = date.Substring(0, 10);
            }
            char[] separator = new char[] { token };
            string[] strArray = date.Split(separator);
            int year = Convert.ToInt32(strArray[formats.IndexOf('y')]);
            int month = Convert.ToInt32(strArray[formats.IndexOf('m')]);
            return new DateTime(year, month, Convert.ToInt32(strArray[formats.IndexOf('d')]));
        }

        /// <summary>
        /// Base64字符串转字节数组
        /// </summary>
        /// <param name="base64str"></param>
        /// <returns></returns>
        public static byte[] ToByteArray(string base64str)
        {
            if (base64str.IsNullOrWhiteSpace())
                return null;
            return Convert.FromBase64String(base64str);
        }
    }
}

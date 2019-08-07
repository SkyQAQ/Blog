using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Core.Common
{
    /// <summary>
    /// 数据类型转换类
    /// </summary>
    public static class Cast
    {
        public static bool ConToBoolean(object value)
        {
            if (((value == null) || (value is DBNull)) || (value.ToString().Trim() == ""))
            {
                return false;
            }
            return Convert.ToBoolean(value);
        }

        public static DateTime ConToDateTime(object date)
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

        public static DateTime NullDateTime
        {
            get
            {
                return DateTime.Parse("1753-1-1 12:00:00");
            }
        }

        public static DateTime? ConToDateTimeNull(object date)
        {
            if ((date == null) || (date is DBNull))
            {
                return null;
            }
            if (date is DateTime)
            {
                return new DateTime?((DateTime)date);
            }
            return new DateTime?(Convert.ToDateTime(date.ToString()));
        }

        public static decimal ConToDecimal(object value)
        {
            if (((value == null) || (value is DBNull)) || (value.ToString().Trim() == ""))
            {
                return decimal.Zero;
            }
            return Convert.ToDecimal(value);
        }

        public static double ConToDouble(object value)
        {
            if (((value == null) || (value is DBNull)) || (value.ToString().Trim() == ""))
            {
                return 0.0;
            }
            return Convert.ToDouble(value);
        }

        public static long ConToLong(object value)
        {
            if (((value == null) || (value is DBNull)) || (value.ToString().Trim() == ""))
            {
                return 0;
            }
            return Convert.ToInt64(value);
        }

        public static int ConToInt(object value)
        {
            if (((value == null) || (value is DBNull)) || (value.ToString().Trim() == ""))
            {
                return 0;
            }
            if (value is int)
            {
                return (int)value;
            }
            return Convert.ToInt32(value);
        }

        public static string ConToString(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return "";
            }
            return value.ToString();
        }

        public static string ToBase64String(byte[] value)
        {
            if(value == null || value.Length == 0)
            {
                return "";
            }
            return Convert.ToBase64String(value);
        }

        public static string ConToDateTimeString(object value)
        {
            DateTime dateTime = ConToDateTime(value);
            if (dateTime == null)
            {
                return "";
            }
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static DateTime GetSAPDataTime(object val)
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

        public static DateTime ToDate(string date) =>
            ToDate(date, '-', "ymd");

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
    }
}

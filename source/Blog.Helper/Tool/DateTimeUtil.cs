using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Helper.Tool
{
    /// <summary>
    /// 时间帮助类
    /// </summary>
    public static class DateTimeUtil
    {
        /// <summary>
        /// 计算两个时间的间隔
        /// </summary>
        /// <param name="time1">时间1</param>
        /// <param name="time2">时间2</param>
        /// <param name="type">间隔类型</param>
        /// <returns>doubble</returns>
        public static double Diff(DateTime time1, DateTime time2, DiffType type)
        {
            double result = 0;
            switch (type)
            {
                case DiffType.Year:
                    result = time1.Year - time2.Year;
                    break;
                case DiffType.Month:
                    if (time1.Year == time2.Year)
                        result = time1.Month - time2.Month;
                    else
                        result = (time1.Year - time2.Year) * 12 + (time1.Month - time2.Month);
                    break;
                case DiffType.Day:
                    result = (time1 - time2).TotalDays;
                    break;
                case DiffType.Hour:
                    result = (time1 - time2).TotalHours;
                    break;
                case DiffType.Minute:
                    result = (time1 - time2).TotalMinutes;
                    break;
                case DiffType.Second:
                    result = (time1 - time2).TotalSeconds;
                    break;
                case DiffType.Millisecond:
                    result = (time1 - time2).TotalMilliseconds;
                    break;
                default:
                    break;
            }
            return Math.Abs(result);
        }

        /// <summary>
        /// 获取两个日期间的所有日期列表
        /// </summary>
        /// <param name="time1">时间1</param>
        /// <param name="time2">时间2</param>
        /// <param name="day">日期间隔，默认1天</param>
        /// <param name="format">日期格式</param>
        /// <returns>List<string></returns>
        public static List<string> TwoDatetimeInterval(DateTime time1, DateTime time2, int day = 1, string format = "yyyy/MM/dd")
        {
            if (time1.ToShortDateString() == time2.ToShortDateString())
                return new List<string> { time1.ToString(format) };
            DateTime temp = time1;
            if (time1 > time2)
            {
                time1 = time2;
                time2 = temp;
            }
            List<string> result = new List<string>();
            for(DateTime dt = time1; dt <= time2; dt.AddDays(day))
            {
                result.Add(dt.ToString(format));
            }
            return result;
        }

        /// <summary>
        /// 获取最近一周的日期列表
        /// </summary>
        /// <param name="format">日期格式</param>
        /// <returns>List<string></returns>
        public static List<string> LatestWeek(string format = "yyyy/MM/dd")
        {
            return TwoDatetimeInterval(DateTime.Now.AddDays(-7), DateTime.Now);
        }

        /// <summary>
        /// 获取最近30天的日期列表
        /// </summary>
        /// <param name="format">日期格式</param>
        /// <returns>List<string></returns>
        public static List<string> LatestMonth(string format = "yyyy/MM/dd")
        {
            return TwoDatetimeInterval(DateTime.Now.AddDays(-30), DateTime.Now);
        }
    }

    /// <summary>
    /// 日期时间扩展类
    /// </summary>
    public static class DateTimeExtension
    {
        /// <summary>
        /// 当前日期季度
        /// </summary>
        /// <param name="dt">当前日期</param>
        /// <returns></returns>
        public static int Season(this DateTime dt)
        {
            int month = dt.Month;
            switch (month)
            {
                case 1 | 2 | 3:
                    return 1;
                case 4 | 5 | 6:
                    return 2;
                case 7 | 8 | 9:
                    return 3;
                case 10 | 11:
                    return 4;
                case 12:
                    return 4;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 当前日期年龄
        /// </summary>
        /// <param name="dt">当前日期</param>
        /// <returns>int</returns>
        public static int Age(this DateTime dt)
        {
            DateTime now = DateTime.Now;
            int age = now.Year - dt.Year;
            if (dt.Month > now.Month || (dt.Month == now.Month && dt.Day > now.Day))
                age = age--;
            return age;
        }

        /// <summary>
        /// 获取时间戳，默认13位
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string Timestamp(this DateTime dt, TimestampType type = TimestampType.Millisecond)
        {
            int div = type == TimestampType.Second ? 10000000 : 10000;
            return ((dt.ToUniversalTime().Ticks - 621355968000000000) / div).ToString();
        }
    }

    /// <summary>
    /// 间隔类型
    /// </summary>
    public enum DiffType
    {
        /// <summary>
        /// 年
        /// </summary>
        Year,
        /// <summary>
        /// 月
        /// </summary>
        Month,
        /// <summary>
        /// 日
        /// </summary>
        Day,
        /// <summary>
        /// 时
        /// </summary>
        Hour,
        /// <summary>
        /// 分
        /// </summary>
        Minute,
        /// <summary>
        /// 秒
        /// </summary>
        Second,
        /// <summary>
        /// 毫秒
        /// </summary>
        Millisecond,
    }
    /// <summary>
    /// 时间戳精确度
    /// </summary>
    public enum TimestampType
    {
        /// <summary>
        /// 秒-10位
        /// </summary>
        Second,
        /// <summary>
        /// 毫秒-13位
        /// </summary>
        Millisecond
    }
}

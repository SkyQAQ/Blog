using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Core.Common.Common
{

    public static class DateTimeExtensions
    {
        /// <summary>
        /// 获取该时间所在季度的第一天
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static DateTime ToFirstDayOfSeason(this DateTime target)
        {
            int ThisMonth = DateTime.Now.Month;
            int FirstMonthOfSeason = ThisMonth - (ThisMonth % 3 == 0 ? 3 : (ThisMonth % 3)) + 1;
            target = target.AddMonths(FirstMonthOfSeason - ThisMonth);
            return Convert.ToDateTime(target.ToString("yyyy-MM-01 HH:mm:ss"));
        }

        /// <summary>
        /// 获取该时间所在季度的最后一天
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static DateTime ToLastDayOfSeason(this DateTime target)
        {
            int ThisMonth = DateTime.Now.Month;
            int FirstMonthOfSeason = ThisMonth - (ThisMonth % 3 == 0 ? 3 : (ThisMonth % 3)) + 3;
            target = target.AddMonths(FirstMonthOfSeason - ThisMonth);
            return Convert.ToDateTime(target.AddMonths(1).ToString("yyyy-MM-01 HH:mm:ss")).AddDays(-1);
        }
    }
}

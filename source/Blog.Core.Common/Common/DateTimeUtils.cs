using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Core.Common
{
    public static class DateTimeUtils
    {
        /// <summary>
        /// 北京时间
        /// </summary>
        /// <returns></returns>
        public static DateTime NowBeijing()
        {
            return DateTime.UtcNow.AddHours(8);
        }
    }
}

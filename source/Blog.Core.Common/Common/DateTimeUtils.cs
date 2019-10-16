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

        /// <summary>
        /// 根据日期条件获取月份
        /// </summary>
        /// <param name="type">类型：0-根据日期；1-本季度；2-本年度；</param>
        /// <param name="startdate">开始日期</param>
        /// <param name="enddate">结束日期</param>
        /// <returns></returns>
        public static List<string> GetMonthList(int type, object startdate = null, object enddate = null)
        {
            List<string> monthList = new List<string>();
            string year = DateTime.Now.Year.ToString();
            int month = DateTime.Now.Month;
            switch (type)
            {
                case 0:
                    // 按日期
                    if (startdate == null)
                        throw new Exception("请选择开始日期！");
                    if (enddate == null)
                        throw new Exception("请选择结束日期！");
                    DateTime start;
                    DateTime end;
                    if(startdate.GetType() == new DateTime().GetType())
                    {
                        start = (DateTime)startdate;
                    }
                    else
                    {
                        if (!DateTime.TryParse(startdate.ToString(), out start))
                            throw new Exception("开始日期格式不正确！");
                    }
                    if (enddate.GetType() == new DateTime().GetType())
                    {
                        end = (DateTime)enddate;
                    }
                    else
                    {
                        if (!DateTime.TryParse(enddate.ToString(), out end))
                            throw new Exception("结束日期格式不正确！");
                    }
                    if (start > end)
                        throw new Exception("开始日期不能大于结束日期！");
                    int startYear = start.Year;
                    int startMonth = start.Month;
                    int endYear = end.Year;
                    int endMonth = end.Month;
                    int n = endYear - startYear;
                    if (n == 0)
                    {// 开始日期与结束日期同一年
                        for (int i = startMonth; i <= endMonth; i++)
                        {
                            monthList.Add(string.Concat(startYear.ToString(), i > 9 ? i.ToString() : "0" + i.ToString()));
                        }
                    }
                    else
                    {// 开始日期与结束日期不同年
                        for (int i = startMonth; i <= 12; i++)
                        {
                            monthList.Add(string.Concat(startYear.ToString(), i > 9 ? i.ToString() : "0" + i.ToString()));
                        }
                        if (n > 1)
                        {
                            for (int y = 1; y < n; y++)
                            {
                                for (int m = 1; m <= 12; m++)
                                {
                                    monthList.Add(string.Concat(startYear + y, m > 9 ? m.ToString() : "0" + m.ToString()));
                                }
                            }
                        }
                        for (int i = 1; i <= end.Month; i++)
                        {
                            monthList.Add(string.Concat(endYear.ToString(), i > 9 ? i.ToString() : "0" + i.ToString()));
                        }
                    }
                    break;
                case 1:
                    // 本季度
                    int season = (month - 1) / 3 + 1;
                    switch (season)
                    {
                        case 1:
                            monthList = new List<string> { year + "01", year + "02", year + "03" };
                            break;
                        case 2:
                            monthList = new List<string> { year + "04", year + "05", year + "06" };
                            break;
                        case 3:
                            monthList = new List<string> { year + "07", year + "08", year + "09" };
                            break;
                        case 4:
                            monthList = new List<string> { year + "10", year + "11", year + "12" };
                            break;
                    }

                    break;
                case 2:
                    // 本年度
                    monthList = new List<string> { year + "01", year + "02", year + "03", year + "04", year + "05", year + "06", year + "07", year + "08", year + "09", year + "10", year + "11", year + "12" };
                    break;
            }
            return monthList;
        }
    }
}

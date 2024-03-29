﻿using System;
using System.Data;
using Blog.Core.Common;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Blog.Core.Model;

namespace Blog.Core.Job
{
    public class TestJob1 : BaseJob
    {
        #region 老版，体彩接口更新

        protected void JobRun1()
        {
            try
            {
                string resp = HttpHelper.Get("http://www.lottery.gov.cn/api/lottery_kj_detail_new.jspx?_ltype=4");
                if (string.IsNullOrEmpty(resp))
                    throw new Exception("读取彩票接口失败！");
                dynamic result = JsonConvert.DeserializeObject<dynamic>(resp);

                if (result[0].codeNumber == null)
                    throw new Exception("获取中奖号码失败！");
                string[] nicecode = ((JArray)result[0].codeNumber).Select(item => item.ToString()).ToArray();
                string[] nicecode_pre = new string[5];
                nicecode_pre[0] = nicecode[0];
                nicecode_pre[1] = nicecode[1];
                nicecode_pre[2] = nicecode[2];
                nicecode_pre[3] = nicecode[3];
                nicecode_pre[4] = nicecode[4];
                string[] nicecode_suf = new string[2];
                nicecode_suf[0] = nicecode[5];
                nicecode_suf[1] = nicecode[6];

                if (result[0].eventName == null)
                    throw new Exception("获取中奖期数失败！");
                string currentStage = ((JArray)result[0].eventName).Select(item => item.ToString()).ToArray()[0];

                string sqlString = @"SELECT ui.Email,
                                            di.Type,
                                            di.DreamCode,
                                            di.StartStage,
                                            di.EndStage,
                                            di.UserInfoId
                                     FROM   DreamInfo di WITH(nolock)
                                            INNER JOIN UserInfo ui WITH(nolock)
                                              ON di.UserInfoId = ui.UserInfoId
                                     WHERE  di.IsDeleted = 0
                                            AND di.Type = @type--大乐透
                                            AND di.StartStage <= @stage
                                            AND di.EndStage >= @stage 
                                     ";
                DataTable dt = sql.Query(sqlString, new Dictionary<string, object> { { "@type", DreamInfoEnum.Type.DLT.GetHashCode() }, { "@stage", currentStage } });
                if (dt == null || dt.Rows.Count == 0)
                {
                    JobResult.Append("暂未查询到待开奖大乐透！");
                    return;
                }
                sql.OpenDb();
                sql.Execute("INSERT INTO tbl_wincode(Type, Period, Code) VALUES('大乐透', @p, @code)", new Dictionary<string, object> { { "@p", currentStage }, { "@code", string.Join(' ', nicecode)} });
                foreach (DataRow row in dt.Rows)
                {
                    string email = Cast.ConToString(row["Email"]);
                    if (string.IsNullOrEmpty(email))
                        continue;
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("【{0}】期大乐透中奖号码：{1}", currentStage, string.Join(' ', nicecode));
                    sb.AppendLine();
                    sb.AppendFormat("【{1}】--【{2}】期 你的圆梦号码：{0}", Cast.ConToString(row["DreamCode"]), Cast.ConToString(row["StartStage"]), Cast.ConToString(row["EndStage"]));
                    sb.AppendLine();
                    string[] dreamcode = Cast.ConToString(row["DreamCode"]).Split(' ');
                    string[] dreamcode_pre = new string[5];
                    dreamcode_pre[0] = dreamcode[0];
                    dreamcode_pre[1] = dreamcode[1];
                    dreamcode_pre[2] = dreamcode[2];
                    dreamcode_pre[3] = dreamcode[3];
                    dreamcode_pre[4] = dreamcode[4];
                    string[] dreamcode_suf = new string[2];
                    dreamcode_suf[0] = dreamcode[5];
                    dreamcode_suf[1] = dreamcode[6];
                    string[] same_pre = nicecode_pre.Intersect(dreamcode_pre).OrderBy(item => item).ToArray();
                    string[] same_suf = nicecode_suf.Intersect(dreamcode_suf).OrderBy(item => item).ToArray();
                    int count_pre = same_pre.Length;
                    int count_suf = same_suf.Length;
                    if (count_pre > 0)
                        sb.AppendFormat("前区相同号码：{0}", string.Join(' ', same_pre));
                    else
                        sb.AppendFormat("前区相同号码：nonono");
                    sb.AppendLine();
                    if (count_suf > 0)
                        sb.AppendFormat("后区相同号码：{0}", string.Join(' ', same_suf));
                    else
                        sb.AppendFormat("后区相同号码：nonono");
                    sb.AppendLine();
                    /// 一等奖：投注号码与当期开奖号码全部相同(顺序不限，下同)，即中奖；
                    if (count_pre == 5 && count_suf == 2)
                    {
                        DreamRecord record = new DreamRecord();
                        record.UserInfoId = Guid.Parse(Cast.ConToString(row["UserInfoId"]));
                        record.DreamType = Cast.ConToString(row["Type"]);
                        record.DreamStage = currentStage;
                        record.DreamNumber = Cast.ConToString(row["DreamCode"]);
                        record.DreamAmount = "10000000";
                        sql.Create(record);
                        sb.AppendFormat("恭喜你中得一等奖，从此走上人生巅峰，请登录中国体彩官网 http://www.lottery.gov.cn 获取奖金信息！");
                    }
                    /// 二等奖：投注号码与当期开奖号码中的五个前区号码及任意一个后区号码相同，即中奖；
                    else if (count_pre == 5 && count_suf == 1)
                    {
                        DreamRecord record = new DreamRecord();
                        record.UserInfoId = Guid.Parse(Cast.ConToString(row["UserInfoId"]));
                        record.DreamType = Cast.ConToString(row["Type"]);
                        record.DreamStage = currentStage;
                        record.DreamNumber = Cast.ConToString(row["DreamCode"]);
                        record.DreamAmount = "5000000";
                        sql.Create(record);
                        sb.AppendFormat("恭喜你中得二等奖，西藏心灵之旅可以来一波，请登录中国体彩官网 http://www.lottery.gov.cn 获取奖金信息！");
                    }
                    /// 三等奖：投注号码与当期开奖号码中的五个前区号码相同，即中奖；
                    else if (count_pre == 5 && count_suf == 0)
                    {
                        DreamRecord record = new DreamRecord();
                        record.UserInfoId = Guid.Parse(Cast.ConToString(row["UserInfoId"]));
                        record.DreamType = Cast.ConToString(row["Type"]);
                        record.DreamStage = currentStage;
                        record.DreamNumber = Cast.ConToString(row["DreamCode"]);
                        record.DreamAmount = "10000";
                        sql.Create(record);
                        sb.AppendFormat("恭喜你中得三等奖，奖金10000元，可以买台电脑打LOL了！");
                    }
                    /// 四等奖：投注号码与当期开奖号码中的任意四个前区号码及两个后区号码相同，即中奖；
                    else if (count_pre == 4 && count_suf == 2)
                    {
                        DreamRecord record = new DreamRecord();
                        record.UserInfoId = Guid.Parse(Cast.ConToString(row["UserInfoId"]));
                        record.DreamType = Cast.ConToString(row["Type"]);
                        record.DreamStage = currentStage;
                        record.DreamNumber = Cast.ConToString(row["DreamCode"]);
                        record.DreamAmount = "3000";
                        sql.Create(record);
                        sb.AppendFormat("恭喜你中得四等奖，奖金3000元，Oppo Reno Ace了解一下！");
                    }
                    /// 五等奖：投注号码与当期开奖号码中的任意四个前区号码及任意一个后区号码相同，即中奖；
                    else if (count_pre == 4 && count_suf == 1)
                    {
                        DreamRecord record = new DreamRecord();
                        record.UserInfoId = Guid.Parse(Cast.ConToString(row["UserInfoId"]));
                        record.DreamType = Cast.ConToString(row["Type"]);
                        record.DreamStage = currentStage;
                        record.DreamNumber = Cast.ConToString(row["DreamCode"]);
                        record.DreamAmount = "300";
                        sql.Create(record);
                        sb.AppendFormat("恭喜你中得五等奖，奖金300元，买支口红给女朋友吧....没有？就算了！");
                    }
                    /// 六等奖：投注号码与当期开奖号码中的任意三个前区号码及两个后区号码相同，即中奖；
                    else if (count_pre == 3 && count_suf == 2)
                    {
                        DreamRecord record = new DreamRecord();
                        record.UserInfoId = Guid.Parse(Cast.ConToString(row["UserInfoId"]));
                        record.DreamType = Cast.ConToString(row["Type"]);
                        record.DreamStage = currentStage;
                        record.DreamNumber = Cast.ConToString(row["DreamCode"]);
                        record.DreamAmount = "200";
                        sql.Create(record);
                        sb.AppendFormat("恭喜你中得六等奖，奖金200元，楼下网吧充值200送500！");
                    }
                    /// 七等奖：投注号码与当期开奖号码中的任意四个前区号码相同，即中奖；
                    else if (count_pre == 4 && count_suf == 0)
                    {
                        DreamRecord record = new DreamRecord();
                        record.UserInfoId = Guid.Parse(Cast.ConToString(row["UserInfoId"]));
                        record.DreamType = Cast.ConToString(row["Type"]);
                        record.DreamStage = currentStage;
                        record.DreamNumber = Cast.ConToString(row["DreamCode"]);
                        record.DreamAmount = "100";
                        sql.Create(record);
                        sb.AppendFormat("恭喜你中得七等奖，奖金100元，该干嘛干嘛吧！");
                    }
                    /// 八等奖：投注号码与当期开奖号码中的任意三个前区号码及任意一个后区号码相同，或者任意两个前区号码及两个后区号码相同，即中奖；
                    else if ((count_pre == 3 && count_suf == 1) || (count_pre == 2 && count_suf == 2))
                    {
                        DreamRecord record = new DreamRecord();
                        record.UserInfoId = Guid.Parse(Cast.ConToString(row["UserInfoId"]));
                        record.DreamType = Cast.ConToString(row["Type"]);
                        record.DreamStage = currentStage;
                        record.DreamNumber = Cast.ConToString(row["DreamCode"]);
                        record.DreamAmount = "15";
                        sql.Create(record);
                        sb.AppendFormat("恭喜你中得八等奖，奖金15元，给自己点个黄焖鸡吧！");
                    }
                    /// 九等奖：投注号码与当期开奖号码中的任意三个前区号码相同，或者任意一个前区号码及两个后区号码相同，
                    /// 或者任意两个前区号码及任意一个后区号码相同，或者两个后区号码相同，即中奖。
                    else if ((count_pre == 3 && count_suf == 0) || (count_pre == 1 && count_suf == 2) || (count_pre == 2 && count_suf == 1) || (count_pre == 0 && count_suf == 2))
                    {
                        DreamRecord record = new DreamRecord();
                        record.UserInfoId = Guid.Parse(Cast.ConToString(row["UserInfoId"]));
                        record.DreamType = Cast.ConToString(row["Type"]);
                        record.DreamStage = currentStage;
                        record.DreamNumber = Cast.ConToString(row["DreamCode"]);
                        record.DreamAmount = "5";
                        sql.Create(record);
                        sb.AppendFormat("恭喜你中得九等奖，奖金5元，建议加一元再买三张！");
                    }
                    else
                    {
                        sb.Append("别看了，这期你没中奖！");
                    }
                    EmailHelper.SendEmailByQQ(email, "圆梦大使带你走上人生巅峰", sb.ToString());
                    JobResult.Append("成功通知：" + email + "；");
                    JobResult.AppendLine();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
            finally
            {
                sql.CloseDb();
            }
        }

        #endregion

        protected override async void JobRun()
        {
            try
            {
                string resp = await HttpHelper.GetAsync("https://webapi.sporttery.cn/gateway/lottery/getHistoryPageListV1.qry?gameNo=85&provinceId=0&pageSize=1&isVerify=1&pageNo=1&termLimits=1");
                log.Info("********************************************");
                log.Info(resp);
                if (string.IsNullOrEmpty(resp))
                    throw new Exception("读取彩票接口失败！");
                Root result = JsonConvert.DeserializeObject<Root>(resp);

                if (result.success == "false" || result.value == null || result.value.list == null || result.value.list.Count == 0)
                    throw new Exception("获取中奖号码失败：" + result.errorMessage);
                string[] nicecode = result.value.list[0].lotteryDrawResult.Split(' ');
                string[] nicecode_pre = new string[5];
                nicecode_pre[0] = nicecode[0];
                nicecode_pre[1] = nicecode[1];
                nicecode_pre[2] = nicecode[2];
                nicecode_pre[3] = nicecode[3];
                nicecode_pre[4] = nicecode[4];
                string[] nicecode_suf = new string[2];
                nicecode_suf[0] = nicecode[5];
                nicecode_suf[1] = nicecode[6];

                string currentStage = result.value.list[0].lotteryDrawNum;

                string sqlString = @"SELECT ui.Email,
                                            di.Type,
                                            di.DreamCode,
                                            di.StartStage,
                                            di.EndStage,
                                            di.UserInfoId
                                     FROM   DreamInfo di WITH(nolock)
                                            INNER JOIN UserInfo ui WITH(nolock)
                                              ON di.UserInfoId = ui.UserInfoId
                                     WHERE  di.IsDeleted = 0
                                            AND di.Type = @type--大乐透
                                            AND di.StartStage <= @stage
                                            AND di.EndStage >= @stage 
                                     ";
                DataTable dt = sql.Query(sqlString, new Dictionary<string, object> { { "@type", DreamInfoEnum.Type.DLT.GetHashCode() }, { "@stage", currentStage } });
                if (dt == null || dt.Rows.Count == 0)
                {
                    JobResult.Append("暂未查询到待开奖大乐透！");
                    return;
                }
                sql.OpenDb();
                sql.Execute("INSERT INTO tbl_wincode(Type, Period, Code) VALUES('大乐透', @p, @code)", new Dictionary<string, object> { { "@p", currentStage }, { "@code", string.Join(' ', nicecode) } });
                foreach (DataRow row in dt.Rows)
                {
                    string email = Cast.ConToString(row["Email"]);
                    if (string.IsNullOrEmpty(email))
                        continue;
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("【{0}】期大乐透中奖号码：{1}", currentStage, string.Join(' ', nicecode));
                    sb.AppendLine();
                    sb.AppendFormat("【{1}】--【{2}】期 你的圆梦号码：{0}", Cast.ConToString(row["DreamCode"]), Cast.ConToString(row["StartStage"]), Cast.ConToString(row["EndStage"]));
                    sb.AppendLine();
                    string[] dreamcode = Cast.ConToString(row["DreamCode"]).Split(' ');
                    string[] dreamcode_pre = new string[5];
                    dreamcode_pre[0] = dreamcode[0];
                    dreamcode_pre[1] = dreamcode[1];
                    dreamcode_pre[2] = dreamcode[2];
                    dreamcode_pre[3] = dreamcode[3];
                    dreamcode_pre[4] = dreamcode[4];
                    string[] dreamcode_suf = new string[2];
                    dreamcode_suf[0] = dreamcode[5];
                    dreamcode_suf[1] = dreamcode[6];
                    string[] same_pre = nicecode_pre.Intersect(dreamcode_pre).OrderBy(item => item).ToArray();
                    string[] same_suf = nicecode_suf.Intersect(dreamcode_suf).OrderBy(item => item).ToArray();
                    int count_pre = same_pre.Length;
                    int count_suf = same_suf.Length;
                    if (count_pre > 0)
                        sb.AppendFormat("前区相同号码：{0}", string.Join(' ', same_pre));
                    else
                        sb.AppendFormat("前区相同号码：nonono");
                    sb.AppendLine();
                    if (count_suf > 0)
                        sb.AppendFormat("后区相同号码：{0}", string.Join(' ', same_suf));
                    else
                        sb.AppendFormat("后区相同号码：nonono");
                    sb.AppendLine();
                    /// 一等奖：投注号码与当期开奖号码全部相同(顺序不限，下同)，即中奖；
                    if (count_pre == 5 && count_suf == 2)
                    {
                        DreamRecord record = new DreamRecord();
                        record.UserInfoId = Guid.Parse(Cast.ConToString(row["UserInfoId"]));
                        record.DreamType = Cast.ConToString(row["Type"]);
                        record.DreamStage = currentStage;
                        record.DreamNumber = Cast.ConToString(row["DreamCode"]);
                        record.DreamAmount = "10000000";
                        sql.Create(record);
                        sb.AppendFormat("恭喜你中得一等奖，从此走上人生巅峰，请登录中国体彩官网 http://www.lottery.gov.cn 获取奖金信息！");
                    }
                    /// 二等奖：投注号码与当期开奖号码中的五个前区号码及任意一个后区号码相同，即中奖；
                    else if (count_pre == 5 && count_suf == 1)
                    {
                        DreamRecord record = new DreamRecord();
                        record.UserInfoId = Guid.Parse(Cast.ConToString(row["UserInfoId"]));
                        record.DreamType = Cast.ConToString(row["Type"]);
                        record.DreamStage = currentStage;
                        record.DreamNumber = Cast.ConToString(row["DreamCode"]);
                        record.DreamAmount = "5000000";
                        sql.Create(record);
                        sb.AppendFormat("恭喜你中得二等奖，西藏心灵之旅可以来一波，请登录中国体彩官网 http://www.lottery.gov.cn 获取奖金信息！");
                    }
                    /// 三等奖：投注号码与当期开奖号码中的五个前区号码相同，即中奖；
                    else if (count_pre == 5 && count_suf == 0)
                    {
                        DreamRecord record = new DreamRecord();
                        record.UserInfoId = Guid.Parse(Cast.ConToString(row["UserInfoId"]));
                        record.DreamType = Cast.ConToString(row["Type"]);
                        record.DreamStage = currentStage;
                        record.DreamNumber = Cast.ConToString(row["DreamCode"]);
                        record.DreamAmount = "10000";
                        sql.Create(record);
                        sb.AppendFormat("恭喜你中得三等奖，奖金10000元，可以买台电脑打LOL了！");
                    }
                    /// 四等奖：投注号码与当期开奖号码中的任意四个前区号码及两个后区号码相同，即中奖；
                    else if (count_pre == 4 && count_suf == 2)
                    {
                        DreamRecord record = new DreamRecord();
                        record.UserInfoId = Guid.Parse(Cast.ConToString(row["UserInfoId"]));
                        record.DreamType = Cast.ConToString(row["Type"]);
                        record.DreamStage = currentStage;
                        record.DreamNumber = Cast.ConToString(row["DreamCode"]);
                        record.DreamAmount = "3000";
                        sql.Create(record);
                        sb.AppendFormat("恭喜你中得四等奖，奖金3000元，Oppo Reno Ace了解一下！");
                    }
                    /// 五等奖：投注号码与当期开奖号码中的任意四个前区号码及任意一个后区号码相同，即中奖；
                    else if (count_pre == 4 && count_suf == 1)
                    {
                        DreamRecord record = new DreamRecord();
                        record.UserInfoId = Guid.Parse(Cast.ConToString(row["UserInfoId"]));
                        record.DreamType = Cast.ConToString(row["Type"]);
                        record.DreamStage = currentStage;
                        record.DreamNumber = Cast.ConToString(row["DreamCode"]);
                        record.DreamAmount = "300";
                        sql.Create(record);
                        sb.AppendFormat("恭喜你中得五等奖，奖金300元，买支口红给女朋友吧....没有？就算了！");
                    }
                    /// 六等奖：投注号码与当期开奖号码中的任意三个前区号码及两个后区号码相同，即中奖；
                    else if (count_pre == 3 && count_suf == 2)
                    {
                        DreamRecord record = new DreamRecord();
                        record.UserInfoId = Guid.Parse(Cast.ConToString(row["UserInfoId"]));
                        record.DreamType = Cast.ConToString(row["Type"]);
                        record.DreamStage = currentStage;
                        record.DreamNumber = Cast.ConToString(row["DreamCode"]);
                        record.DreamAmount = "200";
                        sql.Create(record);
                        sb.AppendFormat("恭喜你中得六等奖，奖金200元，楼下网吧充值200送500！");
                    }
                    /// 七等奖：投注号码与当期开奖号码中的任意四个前区号码相同，即中奖；
                    else if (count_pre == 4 && count_suf == 0)
                    {
                        DreamRecord record = new DreamRecord();
                        record.UserInfoId = Guid.Parse(Cast.ConToString(row["UserInfoId"]));
                        record.DreamType = Cast.ConToString(row["Type"]);
                        record.DreamStage = currentStage;
                        record.DreamNumber = Cast.ConToString(row["DreamCode"]);
                        record.DreamAmount = "100";
                        sql.Create(record);
                        sb.AppendFormat("恭喜你中得七等奖，奖金100元，该干嘛干嘛吧！");
                    }
                    /// 八等奖：投注号码与当期开奖号码中的任意三个前区号码及任意一个后区号码相同，或者任意两个前区号码及两个后区号码相同，即中奖；
                    else if ((count_pre == 3 && count_suf == 1) || (count_pre == 2 && count_suf == 2))
                    {
                        DreamRecord record = new DreamRecord();
                        record.UserInfoId = Guid.Parse(Cast.ConToString(row["UserInfoId"]));
                        record.DreamType = Cast.ConToString(row["Type"]);
                        record.DreamStage = currentStage;
                        record.DreamNumber = Cast.ConToString(row["DreamCode"]);
                        record.DreamAmount = "15";
                        sql.Create(record);
                        sb.AppendFormat("恭喜你中得八等奖，奖金15元，给自己点个黄焖鸡吧！");
                    }
                    /// 九等奖：投注号码与当期开奖号码中的任意三个前区号码相同，或者任意一个前区号码及两个后区号码相同，
                    /// 或者任意两个前区号码及任意一个后区号码相同，或者两个后区号码相同，即中奖。
                    else if ((count_pre == 3 && count_suf == 0) || (count_pre == 1 && count_suf == 2) || (count_pre == 2 && count_suf == 1) || (count_pre == 0 && count_suf == 2))
                    {
                        DreamRecord record = new DreamRecord();
                        record.UserInfoId = Guid.Parse(Cast.ConToString(row["UserInfoId"]));
                        record.DreamType = Cast.ConToString(row["Type"]);
                        record.DreamStage = currentStage;
                        record.DreamNumber = Cast.ConToString(row["DreamCode"]);
                        record.DreamAmount = "5";
                        sql.Create(record);
                        sb.AppendFormat("恭喜你中得九等奖，奖金5元，建议加一元再买三张！");
                    }
                    else
                    {
                        sb.Append("别看了，这期你没中奖！");
                    }
                    EmailHelper.SendEmailByQQ(email, "圆梦大使带你走上人生巅峰", sb.ToString());
                    JobResult.Append("成功通知：" + email + "；");
                    JobResult.AppendLine();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
            finally
            {
                sql.CloseDb();
            }
        }
    }

    //如果好用，请收藏地址，帮忙分享。
    public class PrizeLevelListItem
    {
        /// <summary>
        /// 
        /// </summary>
        public int awardType { get; set; }
        /// <summary>
        /// 一等奖
        /// </summary>
        public string prizeLevel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int sort { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string stakeAmount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string stakeCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string totalPrizeamount { get; set; }
    }

    public class ListItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string drawFlowFund { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string drawFlowFundRj { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string estimateDrawTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int isGetKjpdf { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int isGetXlpdf { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string lotteryDrawNum { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string lotteryDrawResult { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int lotteryDrawStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string lotteryDrawTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int lotteryEquipmentCount { get; set; }
        /// <summary>
        /// 超级大乐透
        /// </summary>
        public string lotteryGameName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string lotteryGameNum { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int lotteryGamePronum { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int lotteryPromotionFlag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string lotterySaleBeginTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string lotterySaleEndtime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int lotterySuspendedFlag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string lotteryUnsortDrawresult { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> matchList { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int pdfType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string poolBalanceAfterdraw { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string poolBalanceAfterdrawRj { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<PrizeLevelListItem> prizeLevelList { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> prizeLevelListRj { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ruleType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> termList { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string totalSaleAmount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string totalSaleAmountRj { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int verify { get; set; }
    }

    public class Value
    {
        /// <summary>
        /// 
        /// </summary>
        public List<ListItem> list { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int pageNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int pageSize { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int pages { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int total { get; set; }
    }

    public class Root
    {
        /// <summary>
        /// 
        /// </summary>
        public string dataFrom { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string emptyFlag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string errorCode { get; set; }
        /// <summary>
        /// 处理成功
        /// </summary>
        public string errorMessage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string success { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Value value { get; set; }
    }
}

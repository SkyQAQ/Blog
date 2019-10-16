using System;
using System.Data;
using Blog.Core.Common;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Blog.Core.Job
{
    public class TestJob1 : BaseJob
    {
        protected override void JobRun()
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
                nicecode_suf[0] = nicecode[0];
                nicecode_suf[1] = nicecode[1];

                if (result[0].eventName == null)
                    throw new Exception("获取中奖期数失败！");
                string currentStage = ((JArray)result[0].eventName).Select(item => item.ToString()).ToArray()[0];

                string sqlString = @"SELECT ui.Email,
                                            di.Type,
                                            di.DreamCode,
                                            di.StartStage,
                                            di.EndStage
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
                    dreamcode_suf[0] = dreamcode[0];
                    dreamcode_suf[1] = dreamcode[1];
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
                        sb.AppendFormat("恭喜你中得一等奖，从此走上人生巅峰，请登录中国体彩官网 http://www.lottery.gov.cn 获取奖金信息！");
                    }
                    /// 二等奖：投注号码与当期开奖号码中的五个前区号码及任意一个后区号码相同，即中奖；
                    else if (count_pre == 5 && count_suf == 1)
                    {
                        sb.AppendFormat("恭喜你中得二等奖，西藏心灵之旅可以来一波，请登录中国体彩官网 http://www.lottery.gov.cn 获取奖金信息！");
                    }
                    /// 三等奖：投注号码与当期开奖号码中的五个前区号码相同，即中奖；
                    else if (count_pre == 5 && count_suf == 0)
                    {
                        sb.AppendFormat("恭喜你中得三等奖，奖金10000元，可以买台电脑打LOL了！");
                    }
                    /// 四等奖：投注号码与当期开奖号码中的任意四个前区号码及两个后区号码相同，即中奖；
                    else if (count_pre == 4 && count_suf == 2)
                    {
                        sb.AppendFormat("恭喜你中得四等奖，奖金3000元，Oppo Reno Ace了解一下！");
                    }
                    /// 五等奖：投注号码与当期开奖号码中的任意四个前区号码及任意一个后区号码相同，即中奖；
                    else if (count_pre == 4 && count_suf == 1)
                    {
                        sb.AppendFormat("恭喜你中得五等奖，奖金300元，买支口红给女朋友吧....没有？就算了！");
                    }
                    /// 六等奖：投注号码与当期开奖号码中的任意三个前区号码及两个后区号码相同，即中奖；
                    else if (count_pre == 3 && count_suf == 2)
                    {
                        sb.AppendFormat("恭喜你中得六等奖，奖金200元，楼下网吧充值200送500！");
                    }
                    /// 七等奖：投注号码与当期开奖号码中的任意四个前区号码相同，即中奖；
                    else if (count_pre == 4 && count_suf == 0)
                    {
                        sb.AppendFormat("恭喜你中得七等奖，奖金100元，该干嘛干嘛吧！");
                    }
                    /// 八等奖：投注号码与当期开奖号码中的任意三个前区号码及任意一个后区号码相同，或者任意两个前区号码及两个后区号码相同，即中奖；
                    else if ((count_pre == 3 && count_suf == 1) || (count_pre == 2 && count_suf == 2))
                    {
                        sb.AppendFormat("恭喜你中得八等奖，奖金15元，给自己点个黄焖鸡吧！");
                    }
                    /// 九等奖：投注号码与当期开奖号码中的任意三个前区号码相同，或者任意一个前区号码及两个后区号码相同，或者任意两个前区号码及任意一个后区号码相同，或者两个后区号码相同，即中奖。
                    else if ((count_pre == 3 && count_suf == 0) || (count_pre == 1 && count_suf == 2) || (count_pre == 2 && count_suf == 1) || (count_pre == 0 && count_suf == 2))
                    {
                        sb.AppendFormat("恭喜你中得九等奖，奖金5元，建议加一元再买三张！");
                    }
                    else
                    {
                        sb.AppendLine();
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
        }
    }
}

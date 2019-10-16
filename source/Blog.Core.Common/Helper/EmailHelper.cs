using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.ComponentModel;
using System.Data;

namespace Blog.Core.Common
{
    /// <summary>
    /// Email帮助类
    /// </summary>
    public static class EmailHelper
    {
        /// <summary>
        /// 发送结果
        /// </summary>
        private static string result = "";

        /// <summary>
        /// 配置文件
        /// </summary>
        private static ConfigHelper config = new ConfigHelper();

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="codetype"></param>
        /// <param name="code"></param>
        /// <param name="copy"></param>
        /// <param name="isSync"></param>
        /// <returns></returns>
        public static string SendEmailByQQ(string to, string subject, string body, string codetype = "", string code = "",  string[] copy = null, bool isSync = false)
        {
            try
            {
                string result = SendEmail(config.Email_QQ_Account, to, config.Email_QQ_Host, config.Email_QQ_Password, subject, body, copy, isSync);
                if (!string.IsNullOrEmpty(codetype) && string.IsNullOrEmpty(code))
                {
                    DeleteVerifyCodeLog(to, Constants.ReceiveTypeEmail, codetype);
                }
                RecodeVerifyCodeLog(to, Constants.ReceiveTypeEmail, body, result, code, codetype);
                return result;
            }
            catch (Exception ex)
            {
                RecodeVerifyCodeLog(to, Constants.ReceiveTypeEmail, body, ex.Message, code, codetype);
                throw new Exception("发送失败！"); 
            }
        }

        /// <summary>
        /// 记录验证码发送日志
        /// </summary>
        /// <param name="to"></param>
        /// <param name="totype"></param>
        /// <param name="body"></param>
        /// <param name="result1"></param>
        /// <param name="code"></param>
        /// <param name="codetype"></param>
        private static void RecodeVerifyCodeLog(string to, string totype, string body, string result1, string code = "", string codetype = "")
        {
            SqlHelper sql = new SqlHelper();
            DataTable dt = sql.Query(@"SELECT VerifycodeLogId FROM tbl_verifycodelog WHERE Receive = @to AND ReceiveType = @totype AND CodeType = @codetype",
                new Dictionary<string, object> { { "@to", to }, { "@totype", totype }, { "@codetype", codetype } });
            string sqlString = string.Empty;
            Dictionary<string, object> paramList = new Dictionary<string, object>();
            paramList.Add("@code", code);
            paramList.Add("@message", body);
            paramList.Add("@createdon", DateTimeUtils.NowBeijing());
            if (dt != null && dt.Rows.Count > 0)
            {
                sqlString = "UPDATE tbl_verifycodelog SET Code = @code, Message = @message, CreatedOn = @createdon  WHERE VerifycodeLogId = @id AND IsSuccess = @issucess";
                paramList.Add("@id", dt.Rows[0]["VerifycodeLogId"]);
            }
            else
            {
                sqlString = @"INSERT INTO [dbo].[tbl_verifycodelog]
                                           ([VerifycodeLogId]
                                           ,[Receive]
                                           ,[ReceiveType]
                                           ,[Code]
                                           ,[CodeType]
                                           ,[Message]
		                                   ,[CreatedOn]
		                                   ,[IsSuccess])
                                     VALUES
                                           (NEWID()
                                           ,@receive
                                           ,@receivetype
                                           ,@code
                                           ,@codetype
                                           ,@message
		                                   ,@createdon
		                                   ,@issucess)";

                paramList.Add("@receive", to);
                paramList.Add("@receivetype", totype);
                paramList.Add("@codetype", codetype);
            }

            if (result1 == "发送完成")
            {
                paramList.Add("@issucess", Constants.Boolean_Yes);
            }
            else
            {
                paramList.Add("@issucess", Constants.Boolean_No);
            }
            try
            {
                sql.OpenDb();
                sql.Execute(sqlString, paramList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sql.CloseDb();
            }
        }

        /// <summary>
        /// 删除验证码发送日志
        /// </summary>
        /// <param name="to"></param>
        /// <param name="totype"></param>
        /// <param name="codetype"></param>
        private static void DeleteVerifyCodeLog(string to, string totype, string codetype)
        {
            SqlHelper sql = new SqlHelper();
            try
            {
                sql.OpenDb();
                sql.Execute(@"DELETE FROM tbl_verifycodelog WHERE Receive = @to AND ReceiveType = @totype AND CodeType = @codetype",
                new Dictionary<string, object> { { "@to", to }, { "@totype", totype }, { "@codetype", codetype } });
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sql.CloseDb();
            }
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="from">发件地址</param>
        /// <param name="to">收件地址</param>
        /// <param name="host">邮箱host</param>
        /// <param name="password">发件箱密码</param>
        /// <param name="subject">邮件主题</param>
        /// <param name="body">邮件内容</param>
        /// <param name="copy">抄送邮箱</param>
        /// <param name="isSync">是否异步</param>
        /// <returns></returns>
        public static string SendEmail(string from, string to, string host, string password, string subject, string body, string[] copy = null, bool isSync = false)
        {
            try
            {
                MailAddress fromAddress = new MailAddress(from, "淮安市三轮车");
                MailAddress toAddress = new MailAddress(to, "召唤师");
                MailMessage message = new MailMessage(fromAddress, toAddress);
                // 添加抄送
                if (copy != null && copy.Length > 0)
                {
                    foreach(var c in copy)
                    {
                        message.Bcc.Add(c);
                    }
                }
                // 设置邮件的优先级为正常
                message.Priority = MailPriority.Normal;
                message.Subject = subject;
                message.Body = body;
                message.BodyEncoding = Encoding.Default;
                SmtpClient client = new SmtpClient { Host = host };
                client.EnableSsl = true;
                client.Port = 587;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(from, password);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Host = host;
                if (isSync)
                {
                    client.SendCompleted += new
                    SendCompletedEventHandler(SendCompletedCallback);
                    client.SendAsync(message, "Blog");
                }
                else
                {
                    client.Send(message);
                    result = "发送完成";
                }
                client.Dispose();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 发送成功后
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            String token = (string)e.UserState;
            if (e.Cancelled)
            {
                result = "取消发送";
            }
            if (e.Error != null)
            {
                result = e.Error.ToString();
            }
            else
            {
                result = "发送完成";
            }
        }
    }
}

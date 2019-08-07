using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Quartz;

namespace Blog.Core.Common
{
    /// <summary>
    /// Job基类
    /// </summary>
    public abstract class BaseJob : IJob
    {
        /// <summary>
        /// Job运行结果
        /// </summary>
        protected StringBuilder JobResult = new StringBuilder("");

        /// <summary>
        /// Job执行方法
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task Execute(IJobExecutionContext context)
        {
            DateTime start = DateTimeUtils.NowBeijing();
            try
            {
                JobRun();
                JobLog(context.JobDetail.Key.Group, context.JobDetail.Key.Name, JobResult.ToString(), start, DateTimeUtils.NowBeijing(), Constants.Result_Success);
            }
            catch (Exception ex)
            {
                JobLog(context.JobDetail.Key.Group, context.JobDetail.Key.Name, ex.Message, start, DateTimeUtils.NowBeijing(), Constants.Result_Failure);
            }
            return Task.FromResult(true);
        }

        /// <summary>
        /// Job运行内容
        /// </summary>
        public abstract void JobRun();

        /// <summary>
        /// 记录Job运行日志
        /// </summary>
        /// <param name="group">Job组</param>
        /// <param name="name">Job名</param>
        /// <param name="mssg">Job运行记录信息</param>
        /// <param name="start">Job开始时间</param>
        /// <param name="end">Job结束时间</param>
        /// <param name="status">Job执行状态</param>
        private void JobLog(string group, string name, string mssg, DateTime start, DateTime end, int status)
        {
            SqlHelper sql = new SqlHelper();
            if (group.Contains("$$"))
                group = group.Split("$$")[0];
            if (name.Contains("$$"))
                name = name.Split("$$")[0];
            sql.OpenDb();
            sql.Execute(@"INSERT INTO [dbo].[tbl_joblog]
                                      ([JobLogId],
                                       [JobGroup],
                                       [JobName],
                                       [StartTime],
                                       [EndTime],
                                       [Status],
                                       [Result],
                                       [Host])
                          VALUES      (Newid(),
                                       @group,
                                       @name,
                                       @start,
                                       @end,
                                       @status,
                                       @mssg,
                                       @host) 
                          ",new Dictionary<string, object> {
                { "@group", group },
                { "@name", name },
                { "@start", start },
                { "@end", end },
                { "@status", status },
                { "@mssg", mssg },
                { "@host", WuYao.GetIpAddress() },
            });
            sql.CloseDb();
        }
    }
}

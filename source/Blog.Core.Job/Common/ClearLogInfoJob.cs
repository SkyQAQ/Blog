using System;
using System.Collections.Generic;
using System.Text;
using Blog.Core.Common;

namespace Blog.Core.Job.Common
{
    /// <summary>
    /// 清除Job运行日志
    /// </summary>
    public class ClearLogInfoJob : BaseJob
    {
        /// <summary>
        /// JobRun
        /// </summary>
        protected override void JobRun()
        {
            try
            {
                sql.OpenDb();
                int count = sql.Execute("DELETE FROM tbl_joblog WHERE StartTime <= @date", new Dictionary<string, object> { { "@date", DateTimeUtils.NowBeijing().AddDays(-2) } });
                this.JobResult.AppendFormat("成功清除{0}条运行日志", count);
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
}

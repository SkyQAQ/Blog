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
        #region 初始化
        /// <summary>
        /// Sql帮助类
        /// </summary>
        private SqlHelper sql;
        /// <summary>
        /// 日志帮助类
        /// </summary>
        private LogHelper log;
        /// <summary>
        /// JobRun
        /// </summary>
        public override void JobRun()
        {
            sql = new SqlHelper();
            log = new LogHelper();
            JobMethod();
        }
        #endregion

        #region 执行方法
        /// <summary>
        /// Job方法
        /// </summary>
        private void JobMethod()
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
        #endregion
    }
}

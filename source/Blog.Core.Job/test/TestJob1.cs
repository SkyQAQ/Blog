using System;
using Blog.Core.Common;

namespace Blog.Core.Job
{
    public class TestJob1 : BaseJob
    {
        #region 初始化
        private SqlHelper sql;
        private LogHelper log;

        public override void JobRun()
        {
            sql = new SqlHelper();
            log = new LogHelper();
            JobMethod();
        }
        #endregion

        /// <summary>
        /// Job方法
        /// </summary>
        private void JobMethod()
        {
            try
            {
                log.WriteLog("Hello China!");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
    }
}

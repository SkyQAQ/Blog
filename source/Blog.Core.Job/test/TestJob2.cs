using System;
using Blog.Core.Common;

namespace Blog.Core.Job.test
{
    public class TestJob2 : BaseJob
    {
        protected override void JobRun()
        {
            try
            {
                log.WriteLog("Hello World!");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
    }
}

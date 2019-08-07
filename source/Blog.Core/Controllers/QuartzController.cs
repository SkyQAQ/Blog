using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Blog.Core.Common;
using Blog.Core.Common.Quartz;
using Blog.Core.Model;
using Blog.Core.Biz.Quartz;
using System.Threading.Tasks;

namespace Blog.Core.Controllers
{
    /// <summary>
    /// 调度作业Controller
    /// </summary>
    [Route("api/quartz")]
    public class QuartzController : BaseController
    {
        /// <summary>
        /// 停止调度作业
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("stop")]
        public bool Stop()
        {
            return new QuartzCommand(UserIdentity).Stop();
        }

        /// <summary>
        /// 启动调度作业
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("start")]
        public bool Start()
        {
            return new QuartzCommand(UserIdentity).Start();
        }

        /// <summary>
        /// 获取调度作业状态，开始为true 其他为false
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("status")]
        public bool Status()
        {
            return new QuartzCommand(UserIdentity).Status();
        }

        /// <summary>
        /// 获取定时任务列表数据
        /// </summary>
        /// <param name="searchList"></param>
        /// <returns></returns>
        [HttpGet, Route("list")]
        public List<JobInfoListModel> GetQuartzList(string searchList)
        {
            return new QuartzCommand(UserIdentity).GetQuartzList(!string.IsNullOrEmpty(searchList) ? JsonConvert.DeserializeObject<List<SearchCondition>>(searchList) : null);
        }

        /// <summary>
        /// 获取定时任务运行日志列表数据
        /// </summary>
        /// <param name="group"></param>
        /// <param name="name"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="onlyError"></param>
        /// <returns></returns>
        [HttpGet, Route("loginfo")]
        public LogInfoListData GetLogInfoList(string group, string name, int pageIndex, int pageSize, bool onlyError = false)
        {
            return new QuartzCommand(UserIdentity).GetLogInfoList(group, name, pageIndex, pageSize, onlyError);
        }

        /// <summary>
        /// 根据Job组、Job名称获取Job编辑信息
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        [HttpGet, Route("edit")]
        public QuartzEditModel EditQuartz(string jobId)
        {
            return new QuartzCommand(UserIdentity).EditQuartz(jobId);
        }

        /// <summary>
        /// 创建Quartz
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("save")]
        public string CreateOrUpdateQuartz(QuartzEditModel model)
        {
            return new QuartzCommand(UserIdentity).CreateOrUpdateQuartz(model);
        }

        /// <summary>
        /// 删除Job
        /// </summary>
        /// <param name="ids">Job Ids</param>
        /// <returns></returns>
        [HttpPost, Route("delete")]
        public string CreateOrUpdateQuartz(string[] ids)
        {
            return new QuartzCommand(UserIdentity).DeleteQuartz(ids);
        }

        /// <summary>
        /// 处理Job
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="operate"></param>
        /// <returns></returns>
        [HttpGet, Route("handle")]
        public string HandleJobAsync(string jobId, string operate)
        {
            return new QuartzCommand(UserIdentity).HandleJobAsync(jobId, operate);
        }
        /// <summary>
        /// 获取Job类
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("jobs")]
        public List<OptionModel> GetJobClass()
        {
            return new QuartzCommand(UserIdentity).GetJobClass();
        }
    }
}

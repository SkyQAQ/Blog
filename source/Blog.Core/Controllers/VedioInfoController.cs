using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Blog.Core.Common;
using Blog.Core.Model;
using Blog.Core.Biz.Vedio;

namespace Blog.Core.Controllers
{
    /// <summary>
    /// 视频Controller
    /// </summary>
    [Route("api/vedioinfo")]
    public class VedioInfoController : BaseController
    {
        /// <summary>
        /// 获取视频列表数据
        /// </summary>
        /// <param name="searchList"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="type">1-所有；2-最新；3-最热；4-我的；</param>
        /// <returns></returns>
        [HttpGet, Route("list")]
        public VedioInfoListData GetVedioInfoList(string searchList, int pageIndex, int pageSize, string orderBy, int type)
        {
            return new VedioInfoCommand(UserIdentity).GetVedioInfoList(!string.IsNullOrEmpty(searchList) ? JsonConvert.DeserializeObject<List<SearchCondition>>(searchList) : null, pageIndex, pageSize, orderBy, type);
        }

        /// <summary>
        /// 上传视频
        /// </summary>
        /// <param name="model"></param>
        [HttpPost, Route("upload")]
        public void Upload(VedioEditModel model)
        {
            new VedioInfoCommand(UserIdentity).Upload(model, GetFiles());
        }

        /// <summary>
        /// 删除视频信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost, Route("delete")]
        public string DeleteVedioInfo(string[] ids)
        {
            return new VedioInfoCommand(UserIdentity).DeleteVedioInfo(ids);
        }
    }
}

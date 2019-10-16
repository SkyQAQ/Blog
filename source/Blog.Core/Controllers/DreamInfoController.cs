using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Blog.Core.Common;
using Blog.Core.Model;
using Blog.Core.Biz.Dream;

namespace Blog.Core.Controllers
{
    /// <summary>
    /// 梦想Controller
    /// </summary>
    [Route("api/dreaminfo")]
    public class DreamInfoController : BaseController
    {
        /// <summary>
        /// 获取梦想列表数据
        /// </summary>
        /// <param name="searchList"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        [HttpGet, Route("list")]
        public DreamInfoListData GetDreamInfoList(string searchList, int pageIndex, int pageSize, string orderBy)
        {
            return new DreamInfoCommand(UserIdentity).GetDreamInfoList(!string.IsNullOrEmpty(searchList) ? JsonConvert.DeserializeObject<List<SearchCondition>>(searchList) : null, pageIndex, pageSize, orderBy);
        }

        /// <summary>
        /// 根据梦想Id获取梦想编辑信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("edit")]
        public DreamInfoEditModel GetDreamInfoById(string id)
        {
            return new DreamInfoCommand(UserIdentity).GetDreamInfoById(id);
        }

        /// <summary>
        /// 创建或更新梦想信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("save")]
        public string CreateOrUpdateDreamInfo(DreamInfoEditModel model)
        {
            return new DreamInfoCommand(UserIdentity).CreateOrUpdateDreamInfo(model);
        }

        /// <summary>
        /// 删除梦想信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost, Route("delete")]
        public string DeleteDreamInfo(string[] ids)
        {
            return new DreamInfoCommand(UserIdentity).DeleteDreamInfo(ids);
        }
    }
}

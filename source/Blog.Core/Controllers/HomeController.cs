using Microsoft.AspNetCore.Mvc;
using Blog.Core.Model;
using Blog.Core.Common;
using System.Collections.Generic;
using Blog.Core.Biz.User;
using Newtonsoft.Json;

namespace Blog.Core.Controllers
{
    /// <summary>
    /// 主页Controller
    /// </summary>
    [Route("api/home")]
    public class HomeController : BaseController
    {
        /// <summary>
        /// 获取网易云音乐评论
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("getcloudmusiccomments")]
        public UserIdentity GetCloudMusicComents(string keywords)
        {
            return UserIdentity;
        }
    }
}

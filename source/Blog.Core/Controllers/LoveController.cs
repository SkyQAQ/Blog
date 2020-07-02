using Microsoft.AspNetCore.Mvc;
using Blog.Core.Common;
using Blog.Core.Model;
using Blog.Core.Biz;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Blog.Core.Controllers
{
    /// <summary>
    /// ValueController
    /// </summary>
    [Route("api/love")]
    public class LoveController : Controller
    {
        /// <summary>
        /// 获取系统参数值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("getsystemparamvalue/{name}")]
        public string GetSystemParamValue(string name)
        {
            return new LoveCommand(null).GetSystemParamValue(name);
        }
    }
}

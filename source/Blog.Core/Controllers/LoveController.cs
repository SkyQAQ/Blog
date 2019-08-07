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
        ///// <summary>
        ///// 根据Id获取值
        ///// </summary>
        ///// <param name="name"></param>
        ///// <returns></returns>
        //// GET api/values/5
        //[HttpGet, Route("getloverbyname")]
        //public LoveModel Get(string name)
        //{
        //    return new LoveCommand(null).GetLoverByName(name);
        //}

        ///// <summary>
        ///// 创建值
        ///// </summary>
        ///// <param name="lover"></param>
        ///// <returns></returns>
        //[HttpPost, Route("addlover")]
        //public string Post(LoveModel lover)
        //{
        //    return new LoveCommand(null).AddLover(lover);
        //}

        ///// <summary>
        ///// 删除值
        ///// </summary>
        ///// <param name="name"></param>
        ///// <returns></returns>
        //[HttpGet, Route("dellover")]
        //public string Delete(string name)
        //{
        //    return new LoveCommand(null).DelLover(name);
        //}

        ///// <summary>
        ///// AES-256加密
        ///// </summary>
        ///// <param name="text">明文</param>
        ///// <param name="key">密钥</param>
        ///// <param name="iv">向量</param>
        ///// <returns>string</returns>
        //[HttpGet, Route("aesencrypt")]
        //public string AesEncrypt(string text, string key, string iv)
        //{
        //    return new LoveCommand(null).AesEncrypt(text, key, iv);
        //}

        ///// <summary>
        ///// AES-256解密
        ///// </summary>
        ///// <param name="text">密文</param>
        ///// <param name="key">密钥</param>
        ///// <param name="iv">向量</param>
        ///// <returns>string</returns>
        //[HttpGet, Route("aesaecrypt")]
        //public string AesDecrypt(string text, string key, string iv)
        //{
        //    return new LoveCommand(null).AesDecrypt(text, key, iv);
        //}
    }
}

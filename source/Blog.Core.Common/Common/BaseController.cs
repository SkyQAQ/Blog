using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Blog.Core.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IO;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace Blog.Core.Common
{
    /// <summary>
    /// Controller基类
    /// </summary>
    [Authorize]
    public class BaseController : Controller
    {
        /// <summary>
        /// HttpContext
        /// </summary>
        private HttpContext httpContext;

        /// <summary>
        /// 构造函数
        /// </summary>
        protected BaseController()
        {
        }

        /// <summary>
        /// 用户身份
        /// </summary>
        protected UserIdentity UserIdentity = null;

        /// <summary>
        /// 执行时操作
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            httpContext = context.HttpContext;
            UserIdentity identity = new UserIdentity();
            string authorization = string.Empty;
            if (context.HttpContext.Request.Headers.ContainsKey("Authorization"))
            {
                authorization = context.HttpContext.Request.Headers["Authorization"];
            }
            if (string.IsNullOrWhiteSpace(authorization))
            {
                throw new UnauthorizedAccessException("Unauthenticated");
            }

            #region 1.0
            //string[] tokenA = authorization.Split(' ');
            //if (tokenA[0].ToLower() != "bearer")
            //{
            //    throw new UnauthorizedAccessException("Invalid token type");
            //}
            //string token = WuYao.RsaDecrypt(tokenA[1]);
            //string[] tokeninfo = token.Split('$');
            //if (tokeninfo == null || tokeninfo.Length == 0)
            //{
            //    throw new UnauthorizedAccessException("Invalid token");
            //}
            //string userId = tokeninfo[1];
            //string ticks = tokeninfo[2];
            //if (!httpContext.Request.Path.Value.Contains("attachment"))
            //{
            //    if (Cast.ConToLong(ticks) < DateTime.UtcNow.Ticks)
            //    {
            //        throw new UnauthorizedAccessException("Expired token");
            //    }
            //}
            //identity = new AuthHelper().GetUserIdentity(userId);
            #endregion

            if (httpContext.User.Identity.IsAuthenticated)
            {
                var claims = httpContext.User.Claims.ToArray();
                identity.UserId = claims[0].Value;
                identity.UserAccount = claims[1].Value;
                identity.UserRoles = claims[2].Value == string.Empty ? null : claims[2].Value.Split(',');
            }
            else
            {
                throw new UnauthorizedAccessException("Unauthenticated");
            }
            this.UserIdentity = identity;
        }

        /// <summary>
        /// 获取多个上传文件
        /// </summary>
        /// <returns></returns>
        protected List<IFormFile> GetFiles()
        {
            if (httpContext.Request.Form == null || httpContext.Request.Form.Files == null)
            {
                return null;
            }
            return httpContext.Request.Form.Files.ToList();
        }

        /// <summary>
        /// 获取单个上传文件
        /// </summary>
        /// <returns></returns>
        protected IFormFile GetFile()
        {
            if (GetFiles() == null)
            {
                return null;
            }
            return GetFiles()[0];
        }

        /// <summary>
        /// 获取EXCEL数据
        /// </summary>
        /// <param name="file">导入文件</param>
        /// <returns>List<T></returns>
        protected DataTable GetExcelDataTable()
        {
            List<IFormFile> files = GetFiles();
            if (files == null || files.Count == 0)
            {
                throw new Exception("请选择文件上传！");
            }
            if (files.Count > 1)
            {
                throw new Exception("只能上传一个文件！");
            }
            IFormFile file = files[0];
            string[] array = file.FileName.Split('.');
            if (!new string[] { "xls", "xlsx" }.Contains(array.Last()))
            {
                throw new Exception("只能上传xlx、xlsx类型的文件！");
            }
            MemoryStream ms = new MemoryStream();
            file.CopyTo(ms);
            return FileHelper.ConvertExcelToDT(ms, array.Last());
        }
    }
}

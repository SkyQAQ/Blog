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
using System.Security.Claims;

namespace Blog.Core.Common
{
    /// <summary>
    /// Controller基类
    /// </summary>
    [Authorize]
    public abstract class BaseController : Controller
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
            if (!httpContext.User.Identity.IsAuthenticated)
                throw new UnauthorizedAccessException("Unauthenticated");
            identity.UserAccount = httpContext.User.Identity.Name;
            var claims = httpContext.User.Claims.ToArray();
            var authMethod = claims.Where<Claim>(claim => claim.Type == ClaimTypes.AuthenticationMethod).FirstOrDefault().Value;
            if (string.IsNullOrEmpty(authMethod) || authMethod != "access")
                throw new UnauthorizedAccessException("Invalid access_token type");
            identity.UserId = claims.Where<Claim>(claim => claim.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            var roles = claims.Where<Claim>(claim => claim.Type == ClaimTypes.Role).FirstOrDefault().Value;
            identity.UserRoles = string.IsNullOrEmpty(roles) ? null : roles.Split(',');
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

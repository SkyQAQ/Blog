using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Blog.Core.Model;
using Blog.Core.Common;
using System;
using Microsoft.AspNetCore.Authorization;

namespace Blog.Core.Controllers
{
    /// <summary>
    /// 身份验证Controller
    /// </summary>
    [Route("api/auth")]
    public class AuthController : Controller
    {
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("verifycode")]
        public VerifyCode GetVerifyCode()
        {
            return new AuthHelper().GetVerifyCode();
        }

        /// <summary>
        /// 获取公钥
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("publickey")]
        public string GetPublicKey()
        {
            return new AuthHelper().GetPublicKey();
        }

        /// <summary>
        /// 获取Token
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("token")]
        public AuthToken GetAuthToken(LoginCredit credit)
        {
            string[] ipArr = HttpContext.Connection.RemoteIpAddress.ToString().Split(':');
            string clientIp = "localhost";
            if (ipArr.Length >= 4)
            {
                clientIp = ipArr[3];
            }
            return new AuthHelper(clientIp).GetAuthToken(credit);            
        }

        /// <summary>
        /// 登出
        /// </summary>
        [HttpGet, Route("logout")]
        public string Logout(string userId)
        {
            return new AuthHelper().Logout(userId);
        }

        /// <summary>
        /// 获取用户验证码
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("receiveverifycode")]
        public string GetRegisterVerifyCode(string receive, string codetype)
        {
            return new AuthHelper().GetReceiveVerifyCode(receive, codetype);
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("register")]
        public string Register(string receive, string verifycode)
        {
            return new AuthHelper().CreateUser(receive, verifycode);
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("resetpwd")]
        public string ResetPwd(string receive, string verifycode)
        {
            return new AuthHelper().ResetPwd(receive, verifycode);
        }

        /// <summary>
        /// 获取头像
        /// </summary>
        /// <param name="id">附件Id</param>
        /// <returns></returns>
        [HttpGet, Route("avatar")]
        public FileResult GetAvatar(string id)
        {
            Attachment attachment = new AuthHelper().GetAvatar(id);
            var stream = System.IO.File.OpenRead(attachment.FilePath);
            return File(stream, attachment.MimeType, attachment.FileName);
        }
    }
}

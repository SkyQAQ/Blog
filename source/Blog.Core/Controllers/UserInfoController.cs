using Microsoft.AspNetCore.Mvc;
using Blog.Core.Model;
using Blog.Core.Common;
using System.Collections.Generic;
using Blog.Core.Biz.User;
using Newtonsoft.Json;

namespace Blog.Core.Controllers
{
    /// <summary>
    /// 用户信息Controller
    /// </summary>
    [Route("api/userinfo")]
    public class UserInfoController : BaseController
    {
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("getuserinfo")]
        public UserInfoModel GetUserInfo()
        {
            return new UserInfoCommand(UserIdentity).GetUserInfo();
        }

        /// <summary>
        /// 获取用户列表数据
        /// </summary>
        /// <param name="searchList"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        [HttpGet, Route("list")]
        public UserListData GetUserList(string searchList, int pageIndex, int pageSize, string orderBy)
        {
            return new UserInfoCommand(UserIdentity).GetUserList(!string.IsNullOrEmpty(searchList) ? JsonConvert.DeserializeObject<List<SearchCondition>>(searchList) : null, pageIndex, pageSize, orderBy);
        }

        /// <summary>
        /// 禁用用户
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost, Route("disable")]
        public string DisableUser(string[] ids)
        {
            return new UserInfoCommand(UserIdentity).DisableUser(ids);
        }

        /// <summary>
        /// 启用用户
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost, Route("enable")]
        public string EnableUser(string[] ids)
        {
            return new UserInfoCommand(UserIdentity).EnableUser(ids);
        }

        /// <summary>
        /// 根据用户Id获取角色Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("rolelist")]
        public string[] GetRoleInfoList(string id)
        {
            return new UserInfoCommand(UserIdentity).GetRoleInfoList(id);
        }

        /// <summary>
        /// 创建角色与菜单对应关系信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost, Route("saveuir")]
        public string CreateMIR(UIREditModel info)
        {
            return new UserInfoCommand(UserIdentity).CreateUIR(info);
        }

        /// <summary>
        /// 根据用户Id、角色Id删除用户角色
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet, Route("delrole")]
        public string DeleteUserRole(string userId, string roleId)
        {
            return new UserInfoCommand(UserIdentity).DeleteUserRole(userId, roleId);
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="name">用户名称</param>
        /// <param name="pwd">用户密码</param>
        /// <returns></returns>
        [HttpGet, Route("save")]
        public string Save(string name, string pwd)
        {
            return new UserInfoCommand(UserIdentity).Save(name, pwd);
        }

        /// <summary>
        /// 更换邮箱
        /// </summary>
        /// <param name="receive">邮箱</param>
        /// <param name="verifycode">验证码</param>
        /// <returns></returns>
        [HttpGet, Route("changeemail")]
        public string ChangeEmail(string receive, string verifycode)
        {
            return new UserInfoCommand(UserIdentity).ChangeEmail(receive, verifycode);
        }

        /// <summary>
        /// 更新用户头像
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("changeavatar")]
        public string ChangeAvatar()
        {
            var file = GetFile();
            if (file == null)
                throw new System.Exception("请上传图片");
            return new UserInfoCommand(UserIdentity).ChangeAvatar(file);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Core.Biz.User
{
    /// <summary>
    /// 注册账号Model
    /// </summary>
    public class UserInfoRegisterModel
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
    }

    /// <summary>
    /// 用户列表数据
    /// </summary>
    public class UserListData
    {
        /// <summary>
        /// 总记录数
        /// </summary>
        public int RecordCount { get; set; }

        /// <summary>
        /// 数据列表
        /// </summary>
        public List<UserListModel> RecordList { get; set; }

        /// <summary>
        /// 导出数据
        /// </summary>
        public string ExportBuffString { get; set; } = string.Empty;
    }

    /// <summary>
    /// 用户列表模型
    /// </summary>
    public class UserListModel
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserInfoId { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户账号
        /// </summary>
        public string UserAccount { get; set; }

        /// <summary>
        /// 用户邮箱
        /// </summary>
        public string UserEmail { get; set; }

        /// <summary>
        /// 用户角色
        /// </summary>
        public List<UserRoleModel> UserRoles { get; set; }

        /// <summary>
        /// 登录状态
        /// </summary>
        public string LoginStatus { get; set; }

        /// <summary>
        /// 上次登录时间
        /// </summary>
        public string LoginOn { get; set; }

        /// <summary>
        /// 上次登录IP
        /// </summary>
        public string LoginClientIp { get; set; }

        /// <summary>
        /// 启用状态
        /// </summary>
        public string UserStatus { get; set; }
    }

    /// <summary>
    /// 用户角色模型
    /// </summary>
    public class UserRoleModel
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        public string RoleInfoId { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 角色编码
        /// </summary>
        public string RoleCode { get; set; }
    }

    /// <summary>
    /// 用户对应角色关系编辑模型
    /// </summary>
    public class UIREditModel
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public string userInfoId { get; set; }

        /// <summary>
        /// 角色Ids
        /// </summary>
        public string[] roleInfoIds { get; set; }
    }

    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfoModel
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string UserAccount { get; set; }
        
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户邮箱
        /// </summary>
        public string UserEmail { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string UserAvatar { get; set; }

        /// <summary>
        /// 用户角色
        /// </summary>
        public string[] UserRoles { get; set; }
    }
}
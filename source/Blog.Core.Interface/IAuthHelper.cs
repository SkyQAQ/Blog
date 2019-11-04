using System;
using System.Collections.Generic;
using System.Text;
using Blog.Core.Model;

namespace Blog.Core.Interface
{
    /// <summary>
    /// Authorize接口
    /// </summary>
    public interface IAuthHelper
    {
        /// <summary>
        /// 验证用户
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="password">密码（与数据库中一致）</param>
        void ValidUser(string account, string password);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="account">账号(需存在于本地数据库)</param>
        /// <param name="password">Rsa解密密码</param>
        void ChangePassword(string account, string password);

        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        string Logout(string userId);
    }
}

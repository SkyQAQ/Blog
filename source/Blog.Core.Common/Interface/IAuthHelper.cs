using Blog.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Core.Common
{
    /// <summary>
    /// Authorizej接口
    /// </summary>
    public interface IAuthHelper
    {
        /// <summary>
        /// 使用第三方登录
        /// </summary>
        /// <param name="account">账号(需存在于本地数据库)</param>
        /// <param name="password">Rsa解密密码</param>
        void ValidUser(string account, string password);

        /// <summary>
        /// 修改第三方密码
        /// </summary>
        /// <param name="account">账号(需存在于本地数据库)</param>
        /// <param name="password">Rsa解密密码</param>
        void ChangePassword(string account, string password);
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Core.Interface
{
    public interface IUserIdentity
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        string UserId { get; set; }

        /// <summary>
        /// 用户账号
        /// </summary>
        string UserAccount { get; set; }

        /// <summary>
        /// 用户角色
        /// </summary>
        string[] UserRoles { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Core.Biz.Home
{
    /// <summary>
    /// 在线用户信息模型
    /// </summary>
    public class OnlineUserModel
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户账号
        /// </summary>
        public string UserAccount { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }
    }
}

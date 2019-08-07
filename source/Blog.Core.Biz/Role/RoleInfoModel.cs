using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Core.Biz.Role
{
    /// <summary>
    /// 角色编信息辑模型
    /// </summary>
    public class RoleInfoEditModel
    {
        /// <summary>
        /// 角色ID
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
    /// 角色对应菜单关系编辑模型
    /// </summary>
    public class MIREditModel
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        public string roleInfoId { get; set; }

        /// <summary>
        /// 菜单Ids
        /// </summary>
        public string[] menuInfoIds { get; set; }
    }
}

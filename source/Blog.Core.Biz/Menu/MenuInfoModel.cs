using System;
using System.Collections.Generic;
using System.Text;
using Blog.Core.Model;

namespace Blog.Core.Biz.Menu
{
    /// <summary>
    /// 菜单信息编辑Model
    /// </summary>
    public class MenuInfoEditModel
    {
        /// <summary>
        /// 菜单Id
        /// </summary>
        public string MenuInfoId { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName { get; set; }

        /// <summary>
        /// 菜单编码
        /// </summary>
        public string MenuCode { get; set; }

        /// <summary>
        /// 菜单路径
        /// </summary>
        public string MenuPath { get; set; } 

        /// <summary>
        /// 菜单排序
        /// </summary>
        public int MenuSeq { get; set; } = 0;

        /// <summary>
        /// 上级菜单Id
        /// </summary>
        public LookUpModel PMenuId { get; set; }
    }
}

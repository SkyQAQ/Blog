using System;
using System.Collections.Generic;
using System.Text;
using Blog.Core.Common;

namespace Blog.Core.Biz.Vedio
{
    /// <summary>
    /// 视频编信息辑模型
    /// </summary>
    public class VedioEditModel
    {
        /// <summary>
        /// 视频Id
        /// </summary>
        public string VedioInfoId { get; set; }

        /// <summary>
        /// 视频描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 是否公开
        /// </summary>
        public bool IsPublic { get; set; }
    }    
}
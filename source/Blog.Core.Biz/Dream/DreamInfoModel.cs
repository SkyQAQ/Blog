using System;
using System.Collections.Generic;
using System.Text;
using Blog.Core.Common;

namespace Blog.Core.Biz.Dream
{
    /// <summary>
    /// 梦想编信息辑模型
    /// </summary>
    public class DreamInfoEditModel
    {
        /// <summary>
        /// 彩票Id
        /// </summary>
        public string DreamInfoId { get; set; }

        /// <summary>
        /// 彩票号码
        /// </summary>
        public string DreamCode { get; set; }

        /// <summary>
        /// 起始期数
        /// </summary>
        public int StartStage { get; set; }

        /// <summary>
        /// 截止期数
        /// </summary>
        public int EndStage { get; set; }

        /// <summary>
        /// 彩票类型
        /// </summary>
        public int Type { get; set; }
    }    
}
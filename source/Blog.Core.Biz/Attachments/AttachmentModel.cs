using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Core.Biz.Attachments
{
    #region 上传附件模型
    /// <summary>
    /// 附件信息
    /// </summary>
    public class FileInfo
    {
        /// <summary>
        /// 模块名称
        /// </summary>
        public string ModuleType { get; set; }

        /// <summary>
        /// 模块Id
        /// </summary>
        public string ModuleId { get; set; }

        /// <summary>
        /// 附件目录
        /// </summary>
        public string FileDir { get; set; }
    }
    #endregion
}

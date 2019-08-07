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

    /// <summary>
    /// 附件上传限制
    /// </summary>
    public class FileUploadLimit
    {
        /// <summary>
        /// 请求地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 每次最多上传条数
        /// </summary>
        public int MaxCount { get; set; }

        /// <summary>
        /// 单个文件最大长度 (M)
        /// </summary>
        public int MaxLength { get; set; }

        /// <summary>
        /// 限制类型
        /// </summary>
        public string Type { get; set; }
    }
    #endregion
}

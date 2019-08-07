using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Core.Biz.Quartz
{
    /// <summary>
    /// Job运行日志列表数据
    /// </summary>
    public class LogInfoListData
    {
        /// <summary>
        /// 总记录数
        /// </summary>
        public int RecordCount { get; set; }

        /// <summary>
        /// 数据列表
        /// </summary>
        public List<LogInfoListModel> RecordList { get; set; }

        /// <summary>
        /// 导出数据
        /// </summary>
        public string ExportBuffString { get; set; } = string.Empty;
    }

    /// <summary>
    /// Job运行日志列表数据模型
    /// </summary>
    public class LogInfoListModel
    {
        /// <summary>
        /// 日志Id
        /// </summary>
        public string JobLogId { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 结果
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// Host
        /// </summary>
        public string Host { get; set; }
    }
}

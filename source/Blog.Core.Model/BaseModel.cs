using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Core.Model
{
    /// <summary>
    /// Model基类
    /// </summary>
    public class BaseModel
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="TableName"></param>
        public BaseModel(string TableName)
        {
            this.ModelName = TableName;
        }

        /// <summary>
        /// 表名
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public virtual Guid BaseId { get; set; }
    }
}

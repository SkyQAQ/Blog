using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Core.Common
{
    /// <summary>
    /// 梦想信息枚举类
    /// </summary>
    public static class DreamInfoEnum
    {
        /// <summary>
        /// 彩票类型
        /// </summary>
        public enum Type
        {
            /// <summary>
            /// 大乐透
            /// </summary>
            DLT = 1,
            /// <summary>
            /// 排列三
            /// </summary>
            PL3 = 2,
            /// <summary>
            /// 排列五
            /// </summary>
            PL5 = 3
        }
    }
}

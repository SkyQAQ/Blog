using System;
using System.Collections.Generic;
using Blog.Core.Common;
using Blog.Core.Model;
using Newtonsoft.Json;
using System.Data;

namespace Blog.Core.Biz
{
    /// <summary>
    /// LoveCommand
    /// </summary>
    public class LoveCommand : BaseCommand
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="identity"></param>
        public LoveCommand(UserIdentity identity) : base(identity)
        {
        }

        /// <summary>
        /// 获取系统参数值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetSystemParamValue(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    return "";
                DataTable dt = _sql.Query("SELECT ParamValue FROM SystemParam WHERE ParamName = @name", new Dictionary<string, object> { { "@name", name } });
                if (dt == null || dt.Rows.Count == 0)
                    return "";
                return Cast.ConToString(dt.Rows[0]["ParamValue"]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

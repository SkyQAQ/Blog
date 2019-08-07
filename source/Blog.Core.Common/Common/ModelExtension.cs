using Blog.Core.Model;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Blog.Core.Common
{
    /// <summary>
    /// Model扩展类
    /// </summary>
    public static class ModelExtension
    {
        /// <summary>
        /// 根据Model自动填充TableModel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="obj"></param>
        public static void FillTableWithModel<T>(this object model, T t) where T : BaseModel
        {
            try
            {
                if (model == null)
                {
                    throw new Exception("model is null!");
                }
                if (t == null)
                {
                    throw new Exception("obj is null!");
                }
                var mType = model.GetType();
                var objType = t.GetType();
                var piList = objType.GetProperties();
                foreach (var pi in piList)
                {
                    if (pi == null || pi.IsSpecialName)
                        continue;
                    if (!pi.CanWrite)
                        continue;
                    PropertyInfo mPi = mType.GetProperty(pi.Name);
                    if (mPi == null)
                        continue;
                    if (!mPi.CanRead)
                        continue;
                    object value = mPi.GetValue(model, null);
                    if (value == null)
                        continue;
                    if (pi.PropertyType.Name.ToLower() == "string")
                    {
                        if (value.GetType().Name.ToLower() == "datetime")
                        {
                            pi.SetValue(t, Convert.ToDateTime(value).ToString("yyyy-MM-dd HH:mm:ss"), null);
                        }
                        else
                        {
                            pi.SetValue(t, Convert.ToString(value), null);
                        }
                    }
                    else if (pi.PropertyType.Name.ToLower() == "int32" || pi.PropertyType.Name.ToLower() == "nullable`1")
                    {
                        pi.SetValue(t, Convert.ToInt32(value), null);
                    }
                    else if (pi.PropertyType.Name.ToLower() == "decimal")
                    {
                        pi.SetValue(t, Convert.ToDecimal(value), null);
                    }
                    else if (pi.PropertyType.Name.ToLower() == "datetime")
                    {
                        pi.SetValue(t, Convert.ToDateTime(value), null);
                    }
                    else if (pi.PropertyType.Name.ToLower() == "boolean")
                    {
                        pi.SetValue(t, Convert.ToBoolean(value), null);
                    }
                    else if (pi.PropertyType.Name.ToLower() == "guid")
                    {
                        if (mPi.PropertyType.Name.ToLower() == "lookupmodel")
                        {
                            LookUpModel lookUp = value as LookUpModel;
                            if (!string.IsNullOrEmpty(lookUp.Id))
                            {
                                pi.SetValue(t, Guid.Parse(lookUp.Id), null);
                            }
                        }
                        else
                        {
                            pi.SetValue(t, Guid.Parse(value.ToString()), null);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

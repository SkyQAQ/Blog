using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using Blog.Core.Model;

namespace Blog.Core.Common
{
    /// <summary>
    /// DataTable扩展类
    /// </summary>
    public static class DataTableExtension
    {
        /// <summary>
        /// DataTable转List
        /// </summary>
        /// <typeparam name="T">模板</typeparam>
        /// <param name="dt">DataTable</param>
        /// <param name="model">数据</param>
        /// <returns></returns>
        public static List<T> ToModelList<T>(this DataTable dt) where T : new ()
        {
            try
            {
                if (dt == null || dt.Rows.Count == 0)
                {
                    return new List<T>();
                }
                List<T> ts = new List<T>();
                Type type = typeof(T);
                string tempName = string.Empty;
                if (dt.Columns.IndexOf("Sno") <= -1)
                    dt.Columns.Add("Sno", typeof(int));
                int index = 1;
                foreach (DataRow dr in dt.Rows)
                {
                    dr["Sno"] = index;
                    T t = new T();
                    // 获得此模型的公共属性  
                    PropertyInfo[] propertys = t.GetType().GetProperties();
                    foreach (PropertyInfo pi in propertys)
                    {
                        tempName = pi.Name;
                        if (dt.Columns.Contains(tempName))
                        {
                            if (!pi.CanWrite)
                                continue;
                            object value = dr[tempName];
                            if (value is DBNull)
                                continue;
                            if (pi.PropertyType.Name.ToLower() == "string")
                            {
                                if (value.GetType().Name.ToLower() == "guid")
                                {
                                    pi.SetValue(t, value.ToString(), null);
                                }
                                else if (value.GetType().Name.ToLower() == "datetime")
                                {
                                    pi.SetValue(t, Convert.ToDateTime(value).ToString("yyyy-MM-dd HH:mm:ss"), null);
                                }
                                else
                                {
                                    if (dt.Columns[tempName].Caption.ToLower() == tempName.ToLower())
                                    {
                                        pi.SetValue(t, Convert.ToString(value), null);
                                    }
                                    else if (dt.Columns[tempName].Caption == Constants.EncryptColoumn)
                                    {
                                        pi.SetValue(t, WuYao.AesDecrypt(Convert.ToString(value)), null);
                                    }
                                    else if (dt.Columns[tempName].Caption == Constants.DecryptColoumn)
                                    {
                                        pi.SetValue(t, WuYao.AesEncrypt(Convert.ToString(value)), null);
                                    }
                                    else
                                    {
                                        pi.SetValue(t, Convert.ToString(value), null);
                                    }
                                }
                            }
                            else if (pi.PropertyType.Name.ToLower() == "lookupmodel")
                            {
                                if (dt.Columns.Contains(string.Concat(tempName, "Name")))
                                {
                                    object valuename = dr[string.Concat(tempName, "Name")];
                                    LookUpModel lum = new LookUpModel();
                                    if (valuename != DBNull.Value)
                                    { 
                                        lum.Id = Convert.ToString(value);
                                        lum.Name = Convert.ToString(valuename);
                                        pi.SetValue(t, lum, null);
                                    }
                                    else
                                    {
                                        pi.SetValue(t, lum, null);
                                        //throw new Exception(string.Format("The value of column '{0}' is null!", string.Concat(tempName, "Name")));
                                    }
                                }
                                else
                                {
                                    throw new Exception(string.Format("The column '{0}' dose not exist!", string.Concat(tempName, "Name")));
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
                            else if(pi.PropertyType.Name.ToLower() == "guid")
                            {
                                pi.SetValue(t, Guid.Parse(value.ToString()), null);
                            }
                        }
                    }
                    ts.Add(t);
                    index++;
                }
                return ts;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 实体类转换成DataTable
        /// </summary>
        /// <param name="modelList">实体类列表</param>
        /// <returns></returns>
        public static DataTable FillDataTable<T>(this List<T> modelList)
        {
            if (modelList == null || modelList.Count == 0)
            {
                return null;
            }
            DataTable dt = CreateData(modelList[0]);

            foreach (T model in modelList)
            {
                DataRow dataRow = dt.NewRow();
                foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
                {
                    if (propertyInfo.PropertyType.Name.ToLower() == "lookupmodel")
                    {
                        dataRow[propertyInfo.Name] =  (propertyInfo.GetValue(model, null) as LookUpModel).Id;
                    }
                    else
                    {
                        dataRow[propertyInfo.Name] = propertyInfo.GetValue(model, null);
                    }
                }
                dt.Rows.Add(dataRow);
            }
            if (dt.Columns.Contains("BaseId"))
                dt.Columns.Remove("BaseId");
            if (dt.Columns.Contains("ModelName"))
                dt.Columns.Remove("ModelName");
            return dt;
        }

        /// <summary>
        /// 根据实体类得到表结构
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns></returns>
        private static DataTable CreateData<T>(T model)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
            {
                dataTable.Columns.Add(new DataColumn(propertyInfo.Name, propertyInfo.PropertyType));
            }
            return dataTable;
        }

        /// <summary>
        /// DataTable转TreeNode
        /// 必需列 id label pid
        /// 非必需 label1 label2
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<TreeNode> ToTreeNode(this DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                throw new Exception("DataTable is Null!");
            }
            List<TreeNode> data = new List<TreeNode>();
            // 存放父节点
            Dictionary<string, TreeNode> dadNoodMap = new Dictionary<string, TreeNode>();
            // 存放子节点
            Dictionary<string, TreeNode> kidNoodMap = new Dictionary<string, TreeNode>();
            bool isLable1 = dt.Columns.Contains("label1");
            bool isLable2 = dt.Columns.Contains("label2");
            foreach (DataRow row in dt.Rows)
            {
                TreeNode node = new TreeNode();
                node.id = Cast.ConToString(row["id"]);
                node.label = Cast.ConToString(row["label"]);
                if (isLable1)
                    node.label1 = Cast.ConToString(row["label1"]);
                if (isLable2)
                    node.label2 = Cast.ConToString(row["label2"]);
                node.pid = Cast.ConToString(row["pid"]);
                if (string.IsNullOrEmpty(node.pid))
                {
                    dadNoodMap.Add(node.id, node);
                }
                else
                {
                    kidNoodMap.Add(node.id, node);
                }
            }
            foreach(var map in dadNoodMap)
            {
                FillTreeChildren(map.Value, kidNoodMap);
                data.Add(map.Value);
            }
            return data;
        }

        /// <summary>
        /// 填充子节点
        /// </summary>
        /// <param name="dadNode"></param>
        /// <param name="kidNoodMap"></param>
        private static void FillTreeChildren(TreeNode dadNode, Dictionary<string, TreeNode> kidNoodMap)
        {
            foreach(var map in kidNoodMap)
            {
                if (map.Value.pid == dadNode.id)
                {
                    if (dadNode.children == null) { dadNode.children = new List<TreeNode>(); }
                    FillTreeChildren(map.Value, kidNoodMap);
                    dadNode.children.Add(map.Value);
                }
            }
        }
    }
}

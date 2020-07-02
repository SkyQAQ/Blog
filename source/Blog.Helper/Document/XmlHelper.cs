using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Xml.Linq;
using System.Reflection;
using System.Linq;

namespace Blog.Helper.Document
{
    /// <summary>
    /// XML帮助方法
    /// </summary>
    public static class XmlHelper
    {
        /// <summary>
        /// 转换DataTable至Xml
        /// </summary>
        /// <param name="dt">DT</param>
        /// <param name="xmlname">xml名称</param>
        public static void ConvertDataTableToXml(DataTable dt,string xmlname)
        {
            string xmlpath = Environment.CurrentDirectory + "\\Xml";
            ConvertDataTableToXml(dt, xmlname, xmlpath);
        }

        /// <summary>
        /// 转换DataTable至Xml
        /// </summary>
        /// <param name="dt">DT</param>
        /// <param name="xmlname">xml名称</param>
        /// <param name="xmlpath">xml路径</param>
        public static void ConvertDataTableToXml(DataTable dt, string xmlname, string xmlpath)
        {
            try
            {
                if (dt == null || dt.Rows == null || dt.Rows.Count == 0)
                    throw new Exception("DataTable不能为空！");
                if (string.IsNullOrWhiteSpace(xmlpath))
                    throw new Exception("XML文件路径不能为空！");
                if (string.IsNullOrWhiteSpace(xmlname))
                    throw new Exception("XML文件名不能为空！");
                if (!Directory.Exists(xmlpath))
                    Directory.CreateDirectory(xmlpath);
                if (!xmlname.ToLower().EndsWith(".xml"))
                    xmlname += ".xml";
                XDocument xmldoc = new XDocument(new XElement("DataTable"));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow row = dt.Rows[i];
                    XElement xrow = new XElement("DataRow", new XAttribute("ID", i));
                    foreach (DataColumn col in dt.Columns)
                    {
                        string value = row[col.ColumnName] == null ? "" : Convert.ToString(row[col.ColumnName]);
                        xrow.Add(new XElement(col.ColumnName, value));
                    }
                    xmldoc.Root.Add(xrow);
                }
                xmldoc.Save(Path.Combine(xmlpath, xmlname));
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 加载XML文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static T LoadXmlFile<T>(string filePath) where T : Config.IConfig, new()
        {
            try
            {
                T result = new T();
                if (!File.Exists(filePath))
                    throw new Exception("未找到xml文件：" + Path.GetFileName(filePath));
                XDocument xmldoc = XDocument.Load(filePath);
                XElement xElement = xmldoc.Element(result.GetType().Name);
                if (xElement == null)
                    throw new Exception("未找到元素：" + result.GetType().Name);
                PropertyInfo[] pi = result.GetType().GetProperties();
                if (pi == null)
                    return result;
                foreach (PropertyInfo property in pi)
                {
                    XElement element = xElement.Element(property.Name);
                    if (element != null)
                        property.SetValue(result, element.Value);
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 加载XML文件至Dictionary
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static Dictionary<string, string> LoadXmlFileToDic(string filePath)
        { 
            try
            {
                Dictionary<string, string> result = new Dictionary<string, string>();
                if (!File.Exists(filePath))
                    throw new Exception("未找到xml文件：" + Path.GetFileName(filePath));
                XDocument xmldoc = XDocument.Load(filePath);
                XElement[] elements = xmldoc.Root.HasElements ? xmldoc.Root.Elements().ToArray() : xmldoc.Elements().ToArray();
                foreach (XElement element in elements)
                {
                    if (!result.ContainsKey(element.Name.LocalName))
                        result.Add(element.Name.LocalName, element.Value);
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 加载XML文件至DataTable
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static DataTable LoadXmlFileToDT(string filePath)
        {
            try
            {
                DataTable result = new DataTable();
                if (!File.Exists(filePath))
                    throw new Exception("未找到xml文件：" + Path.GetFileName(filePath));
                XDocument xmldoc = XDocument.Load(filePath);
                XElement element = xmldoc.Element("DataTable");
                if (element == null)
                    throw new Exception("未找到元素DataTable");
                IEnumerable<XElement> elements = element.Elements();
                XElement element1 = elements.First();
                if (!element1.HasAttributes)
                    throw new Exception("未找到元素DataRow中的列数据");
                foreach (XElement col in element1.Elements())
                {
                    result.Columns.Add(col.Name.LocalName, typeof(string));
                }
                foreach (XElement xe in elements)
                {
                    DataRow row = result.NewRow();
                    foreach (XElement col in xe.Elements())
                    {
                        row[col.Name.LocalName] = col.Value;
                    }
                    result.Rows.Add(row);
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
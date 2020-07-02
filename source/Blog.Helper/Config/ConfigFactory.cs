using System;
using System.Collections.Generic;

namespace Blog.Helper.Config
{
    /// <summary>
    /// 配置工厂
    /// </summary>
    public static class ConfigFactory
    {
        /// <summary>
        /// 注册配置文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T RegisterConfigFile<T>() where T : IConfig, new()
        {
            string filePath = Environment.CurrentDirectory + "\\Config\\" + new T().GetType().Name + ".config.xml";
            return Document.XmlHelper.LoadXmlFile<T>(filePath);
        }
        
        /// <summary>
        /// 注册配置文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static Dictionary<string, string> RegisterConfigFile(string filePath)
        {
            return Document.XmlHelper.LoadXmlFileToDic(filePath);
        }
    }
}

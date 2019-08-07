using System;
using log4net;

namespace Blog.Core.Common
{
    public class LogHelper
    {
        private ILog log;

        public LogHelper()
        {
            log = LogManager.GetLogger(Constants.LogRepositoryName, GetType());
        }

        public LogHelper(string name)
        {
            log = LogManager.GetLogger(Constants.LogRepositoryName, name);
        }

        public LogHelper(Type type, string repository = Constants.LogRepositoryName)
        {
            log = LogManager.GetLogger(repository, type);
        }

        public LogHelper(Type type)
        {
            log = LogManager.GetLogger(type);
        }

        public void Info(object text)
        {
            log.Info(text);
        }

        public void Info(object text, Exception ex)
        {
            log.Info(text, ex);
        }

        public void Warn(object text)
        {
            log.Warn(text);
        }

        public void Warn(object text, Exception ex)
        {
            log.Warn(text, ex);
        }

        public void Error(object text)
        {
            log.Error(text);
        }

        public void Error(object text, Exception ex)
        {
            log.Error(text, ex);
        }

        public void Debug(object text)
        {
            log.Debug(text);
        }

        public void Debug(object text, Exception ex)
        {
            log.Debug(text, ex);
        }

        public void WriteLog(string info, string filePath = "", string fileName = "")
        {
            byte[] myByte = System.Text.Encoding.UTF8.GetBytes("[" + DateTimeUtils.NowBeijing().ToString("yyyy-MM-dd HH:mm:ss") + "][Info]:" + info + "\r\n");
            if (string.IsNullOrEmpty(filePath))
            {
                filePath = Constants.ServerMapPath() + "\\log\\";
            }
            if (!System.IO.Directory.Exists(filePath))
            {
                System.IO.Directory.CreateDirectory(filePath);
            }
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = "LogInfo_" + DateTimeUtils.NowBeijing().ToString("yyyyMMddHH");
            }
            string strPathLog = filePath + fileName + ".txt";
            using (System.IO.FileStream fsWrite = new System.IO.FileStream(strPathLog, System.IO.FileMode.Append))
            {
                fsWrite.Write(myByte, 0, myByte.Length);
            };
        }
    }
}

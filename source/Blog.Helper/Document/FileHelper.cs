using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Blog.Helper.Document
{
    /// <summary>
    /// 文件帮助
    /// </summary>
    public static class FileHelper
    {
        public static string CreateText(string filename)
        {
            string path = @"E:\test.txt";
            //FileInfo file = new FileInfo(@"E:\test.txt");
            //using(StreamWriter sw = file.AppendText())
            //{
            //    sw.
            //}
            FileInfo fi = new FileInfo(@"E:\test1.txt");
            using (FileStream fs = fi.Open(FileMode.OpenOrCreate))
            {
                
            }

            return "";
        }
    }
}

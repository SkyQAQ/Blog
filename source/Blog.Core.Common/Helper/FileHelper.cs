using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using System.Data;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.SS.Util;

namespace Blog.Core.Common
{
    public static class FileHelper
    {
        /// <summary>
        /// 压缩多个附件
        /// </summary>
        /// <param name="fileList">文件信息列表（文件路径，文件名）</param>
        /// <returns></returns>
        public static byte[] ZipFile(Dictionary<string, string> fileList)
        {
            string filePath = Constants.ServerMapPath() + Constants.ZipPath + Guid.NewGuid().ToString().Replace("-", "") + ".zip";
            try
            {
                if (fileList == null || fileList.Count == 0)
                {
                    throw new Exception("FileHelper:文件路径错误！");
                }
                using (ZipOutputStream zos = new ZipOutputStream(File.Create(filePath)))
                {
                    zos.SetLevel(6);                                                   //设置压缩等级，等级越高压缩效果越明显，但占用CPU也会更多
                    {
                        byte[] buffer = new byte[4 * 1024];                            //缓冲区，每次操作大小
                        foreach(var path in fileList.Keys)
                        {
                            FileStream fs = File.OpenRead(path);
                            ZipEntry entry = new ZipEntry(fileList[path]);             //创建压缩包内的文件
                            entry.DateTime = DateTimeUtils.NowBeijing();               //文件创建时间
                            zos.PutNextEntry(entry);                                   //将文件写入压缩包
                            int sourceBytes;
                            do
                            {
                                sourceBytes = fs.Read(buffer, 0, buffer.Length);       //读取文件内容(1次读4M，写4M)
                                zos.Write(buffer, 0, sourceBytes);                     //将文件内容写入压缩相应的文件
                            } while (sourceBytes > 0);
                            fs.Close();
                        }
                    }
                    zos.CloseEntry();
                }
                return File.ReadAllBytes(filePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                File.Delete(filePath);
            }
        }

        /// <summary>
        /// 将excel中的数据导入到DataTable中
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <param name="fileType">文件类型</param>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <returns>返回的DataTable</returns>
        public static DataTable ConvertExcelToDT(Stream stream, string fileType, string sheetName = null, bool isFirstRowColumn = true)
        {
            ISheet sheet = null;
            DataTable data = new DataTable();
            int startRow = 0;
            try
            {
                stream.Position = 0;
                IWorkbook workbook = null;
                if (fileType == "xlsx") // 2007版本
                    workbook = new XSSFWorkbook(stream);
                else if (fileType == "xls") // 2003版本
                    workbook = new HSSFWorkbook(stream);
                if (sheetName != null)
                {
                    sheet = workbook.GetSheet(sheetName);
                    if (sheet == null) //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
                    {
                        sheet = workbook.GetSheetAt(0);
                    }
                }
                else
                {
                    sheet = workbook.GetSheetAt(0);
                }
                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(0);
                    int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数

                    if (isFirstRowColumn)
                    {
                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                        {
                            ICell cell = firstRow.GetCell(i);
                            if (cell != null)
                            {
                                string cellValue = GetCellString(cell);
                                if (!string.IsNullOrEmpty(cellValue))
                                {
                                    DataColumn column = new DataColumn(cellValue);
                                    data.Columns.Add(column);
                                }
                            }
                        }
                        startRow = sheet.FirstRowNum + 1;
                    }
                    else
                    {
                        startRow = sheet.FirstRowNum;
                    }

                    //最后一列的标号
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue; //没有数据的行默认是null　　　　　　　

                        DataRow dataRow = data.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                                dataRow[j] = row.GetCell(j).ToString();
                        }
                        data.Rows.Add(dataRow);
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                stream.Close();
            }
        }

        /// <summary>
        /// 将excel中的数据导入到DataTable中
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <returns>返回的DataTable</returns>
        private static DataTable ConvertExcelToDT(string filePath, string sheetName = null, bool isFirstRowColumn = true)
        {
            if (!File.Exists(filePath))
            {
                throw new Exception(filePath + "：未找到文件！");
            }
            string fileType = Path.GetExtension(filePath);
            FileStream stream = File.OpenRead(filePath);
            return ConvertExcelToDT(stream, fileType, sheetName, isFirstRowColumn);
        }

        /// <summary>
        /// 获取Cell字符串
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public static string GetCellString(ICell cell)
        {
            string cellValue = "";
            if (null != cell)
            {
                // 以下是判断数据的类型
                switch (cell.CellType)
                {
                    case CellType.Numeric: // 数字
                        if (HSSFDateUtil.IsCellDateFormatted(cell))
                        {// 判断是否为日期类型
                            cellValue = cell.DateCellValue.ToString("yyyyMMdd HH:mm:ss");
                        }
                        else
                        {
                            // 有些数字过大，直接输出使用的是科学计数法： 2.67458622E8 要进行处理
                            DecimalFormat df = new DecimalFormat("####.####");
                            cellValue = df.Format(cell.NumericCellValue);
                            // cellValue = cell.getNumericCellValue() + "";
                        }
                        break;
                    case CellType.String: // 字符串
                        cellValue = cell.StringCellValue;
                        break;
                    case CellType.Boolean: // Boolean
                        cellValue = cell.BooleanCellValue ? "1" : "0";
                        break;
                    default:
                        cellValue = "wuwuyaoyao";
                        break;
                }
            }
            return cellValue;
        }

        public static string ReadTxt(string path)
        {
            StreamReader sr = new StreamReader(path, Encoding.UTF8);
            return sr.ReadToEnd();
        }
    }
}

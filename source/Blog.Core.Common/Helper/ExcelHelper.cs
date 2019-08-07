using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO;
using System.Linq;

namespace Blog.Core.Common
{
    /// <summary>
    /// Excel帮助类
    /// https://docs.microsoft.com/zh-cn/office/open-xml/structure-of-a-spreadsheetml-document
    /// </summary>
    public static class ExcelHelper
    {
        /// <summary>
        /// 下载Excel
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static string DownloadExcel(DataTable dataTable)
        {
            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                throw new Exception("Data is Null !");
            }
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    // Create a spreadsheet document by supplying the filepath.
                    // By default, AutoSave = true, Editable = true, and Type = xlsx.
                    using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(ms, SpreadsheetDocumentType.Workbook))
                    {
                        // Add a WorkbookPart to the document.
                        WorkbookPart workbookpart = spreadsheetDocument.AddWorkbookPart();
                        workbookpart.Workbook = new Workbook();

                        // Add a WorksheetPart to the WorkbookPart.
                        WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
                        worksheetPart.Worksheet = new Worksheet(new SheetData());

                        // Add Sheets to the Workbook.
                        Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());

                        // Append a new worksheet and associate it with the workbook.
                        Sheet sheet = new Sheet() { Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "sheet1" };
                        sheets.Append(sheet);

                        // Get the sheetData cell table.
                        SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();
                        UInt32Value rowIndex = 1;
                        Row headerTitle = new Row() { RowIndex = rowIndex };
                        List<string> columnNames = new List<string>();
                        foreach (DataColumn dtColumn in dataTable.Columns)
                        {
                            Cell cell = new Cell();
                            cell.DataType = new EnumValue<CellValues>(CellValues.String);
                            cell.CellValue = new CellValue(dtColumn.ColumnName);
                            headerTitle.Append(cell);
                            columnNames.Add(dtColumn.ColumnName);
                        }
                        sheetData.Append(headerTitle);
                        foreach (DataRow dtRow in dataTable.Rows)
                        {
                            rowIndex++;
                            // Add a row to the sheetData.
                            Row row = new Row() { RowIndex = rowIndex };
                            foreach (string columnname in columnNames)
                            {
                                Cell cell = new Cell();
                                cell.DataType = new EnumValue<CellValues>(CellValues.String);
                                if (dataTable.Columns[columnname].Caption == Constants.DecryptColoumn)
                                {
                                    cell.CellValue = new CellValue(WuYao.AesDecrypt(Cast.ConToString(dtRow[columnname])));
                                }
                                else
                                {
                                    cell.CellValue = new CellValue(Cast.ConToString(dtRow[columnname]));
                                }
                                row.Append(cell);
                            }
                            sheetData.AppendChild(row);
                        }
                        worksheetPart.Worksheet.Save();
                    }
                    return Convert.ToBase64String(ms.GetBuffer());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }        
    }
}

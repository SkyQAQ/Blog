using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace Blog.Core.Common
{
    public static class ModelHelper
    {

        private static string _connstr = string.Empty;

        public static void Main(string[] args)
        {
            // 数据库连接字符串
            string DataSource = "";
            string InitialCatalog = "";
            string UserId = "";
            string Password = "";
            // 命名空间
            string namespaces = "";
            // 文件名
            string filename = "";
            if (args != null && args.Length > 0)
            {
                foreach (var arg in args)
                {
                    string[] sa = arg.Split(':');
                    switch (sa[0])
                    {
                        case "DataSource":
                            DataSource = sa[1];
                            break;
                        case "InitialCatalog":
                            InitialCatalog = sa[1];
                            break;
                        case "UserId":
                            UserId = sa[1];
                            break;
                        case "Password":
                            Password = sa[1];
                            break;
                        case "namespaces":
                            namespaces = sa[1];
                            break;
                        case "filename":
                            filename = sa[1];
                            break;
                    }
                }
            }
            _connstr = string.Format("Data Source={0};Initial Catalog={1};User Id={2};Password={3}", DataSource, InitialCatalog, UserId, Password);
            Console.WriteLine(_connstr);
            Console.WriteLine("开始生成，请等待...");
            Generate(namespaces, filename);
            Console.WriteLine("生成成功...");
        }

        private static void Generate(string namespaces, string filename)
        {
            byte[] myByte = Encoding.UTF8.GetBytes(BuildTemplete(namespaces));
            string filepath = Environment.CurrentDirectory + "\\" + filename;
            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }
            using (FileStream fsWrite = new FileStream(filepath, FileMode.Append))
            {
                fsWrite.Write(myByte, 0, myByte.Length);
            };
        }

        /// <summary>
        /// 创建模板
        /// </summary>
        /// <param name="namespaces"></param>
        /// <returns></returns>
        private static string BuildTemplete(string namespaces)
        {
            StringBuilder templete = new StringBuilder("using System;\n");
            templete.Append("using System.Collections.Generic;\n\n");
            templete.AppendFormat("namespace {0}\n{{\n", namespaces);
            List<TableModel> tables = GetTables();
            foreach (var table in tables)
            {
                templete.AppendFormat("    #region {0}\n", table.name);
                templete.Append("    /// <summary>\n");
                templete.AppendFormat("    /// {0}模型\n", table.name);
                templete.Append("    /// </summary>\n");
                templete.Append("    [Serializable]\n");
                templete.AppendFormat("    public class {0} : BaseModel\n    {{", table.name);
                templete.Append("\n");
                templete.Append("        /// <summary>\n");
                templete.Append("        /// 表名\n");
                templete.Append("        /// </summary>\n");
                templete.AppendFormat("        public static readonly string TableName = \"{0}\";\n", table.name);
                templete.Append("\n");
                templete.Append("        /// <summary>\n");
                templete.Append("        /// 构造函数\n");
                templete.Append("        /// </summary>\n");
                templete.AppendFormat("        public {0}() : base(TableName) {{ }}\n", table.name);
                templete.Append("        private Guid Id = Guid.Empty;\n");
                table.columns.ForEach(columu =>
                {
                    templete.Append("\n");
                    templete.Append("        /// <summary>\n");
                    templete.AppendFormat("        /// {0}\n", columu.ColComment);
                    templete.Append("        /// </summary>\n");
                    if (columu.IsPk)
                    {
                        templete.AppendFormat("        public Guid {0}\n", columu.ColName);
                        templete.Append("        {\n");
                        templete.Append("            get { return Id; }\n");
                        templete.Append("            set\n");
                        templete.Append("            {\n");
                        templete.Append("                Id = value;\n");
                        templete.Append("                if (value != null)\n");
                        templete.Append("                {\n");
                        templete.Append("                    base.BaseId = value;\n");
                        templete.Append("                }\n");
                        templete.Append("            }\n");
                        templete.Append("        }\n");
                    }
                    else
                    {
                        templete.AppendFormat("        public {0} {1} {{ get; set; }} {2}\n", GetCSType(columu.ColType), columu.ColName, GetCSDefault(columu.ColDefault));
                    }
                });
                templete.Append("    }");

                templete.Append("\n");

                templete.Append("    /// <summary>\n");
                templete.AppendFormat("    /// {0}数据模型\n", table.name);
                templete.Append("    /// </summary>\n");
                templete.Append("    [Serializable]\n");
                templete.AppendFormat("    public class {0}ListData\n    {{", table.name);
                templete.Append("\n");
                templete.Append("        /// <summary>\n");
                templete.Append("        /// 总记录数\n");
                templete.Append("        /// </summary>\n");
                templete.Append("        public int RecordCount { get; set; }\n");
                templete.Append("\n");
                templete.Append("        /// <summary>\n");
                templete.Append("        /// 数据列表\n");
                templete.Append("        /// </summary>\n");
                templete.AppendFormat("        public List<{0}ListModel> RecordList {{ get; set; }}\n", table.name);
                templete.Append("\n");
                templete.Append("        /// <summary>\n");
                templete.Append("        /// 导出数据\n");
                templete.Append("        /// </summary>\n");
                templete.Append("        public string ExportBuffString { get; set; } = string.Empty;\n");
                templete.Append("    }");

                templete.Append("\n");

                templete.Append("    /// <summary>\n");
                templete.AppendFormat("    /// {0}列表模型\n", table.name);
                templete.Append("    /// </summary>\n");
                templete.Append("    [Serializable]\n");
                templete.AppendFormat("    public partial class {0}ListModel\n    {{", table.name);
                templete.Append("\n");
                table.columns.ForEach(columu =>
                {
                    if (columu.ColName != "IsDeleted")
                    {
                        templete.Append("\n");
                        templete.Append("        /// <summary>\n");
                        templete.AppendFormat("        /// {0}\n", columu.ColComment);
                        templete.Append("        /// </summary>\n");
                        if (new string[] { "Guid", "DateTime" }.Contains(GetCSType(columu.ColType)))
                        {
                            templete.AppendFormat("        public string {0} {{ get; set; }}\n", columu.ColName);
                        }
                        else
                        {
                            templete.AppendFormat("        public {0} {1} {{ get; set; }}\n", GetCSType(columu.ColType), columu.ColName);
                        }
                    }
                });
                templete.Append("    }\n");
                templete.Append("    #endregion\n");
                templete.Append("\n");
            }
            templete = templete.Remove(templete.Length - 2, 1);
            templete.Append("}");
            return templete.ToString();
        }

        private static List<TableModel> GetTables()
        {
            List<TableModel> tables = new List<TableModel>();
            DataTable tabName = Query("SELECT name AS TableName FROM sysobjects WHERE xtype = 'U'");
            DataTable colName = Query(@"--获取表名、字段名称、字段类型、字段说明、字段默认值
                                        SELECT obj.name  AS TableName,--表名
                                               col.name  AS ColName,--列名
                                               typ.name  AS ColType,--字段类型
                                               cmt.value AS ColComment,--字段说明
                                               dft.text  AS ColDefault--字段默认值
                                        FROM   syscolumns col--字段
                                               INNER JOIN sysobjects obj--表
                                                       ON col.id = obj.id
                                                          AND obj.xtype = 'U'--表示用户表
                                               LEFT JOIN systypes typ--类型
                                                      ON col.xtype = typ.xusertype
                                               LEFT JOIN sys.extended_properties cmt--字段说明
                                                      ON col.id = cmt.major_id--表Id
                                                         AND col.colid = cmt.minor_id--字段Id
                                               LEFT JOIN syscomments dft--默认值
                                                      ON col.cdefault = dft.id
                                        ORDER  BY obj.name,
                                                  col.id ASC 
                                        ");
            DataTable pk = Query(@"--获取表的主键字段名
                                   SELECT CCU.COLUMN_NAME,
                                          TC.TABLE_NAME
                                   FROM   INFORMATION_SCHEMA.TABLE_CONSTRAINTS TC
                                          INNER JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE CCU
                                                  ON TC.CONSTRAINT_NAME = CCU.CONSTRAINT_NAME
                                   WHERE  TC.CONSTRAINT_TYPE = 'PRIMARY KEY' 
                                   ");
            foreach (DataRow row in tabName.Rows)
            {
                if (row["TableName"].ToString().StartsWith("tbl") || row["TableName"].ToString().StartsWith("QRTZ"))
                {
                    continue;
                }
                TableModel table = new TableModel();
                table.name = row["TableName"].ToString(); ;
                table.columns = new List<ColumnModel>();
                DataRow[] cols = colName.Select(string.Format("TableName = '{0}'", row["TableName"].ToString()));
                DataRow[] pks = pk.Select(string.Format("TABLE_NAME = '{0}'", row["TableName"].ToString()));
                string primarykey = pks == null || pks.Length == 0 ? "" : pks[0]["COLUMN_NAME"].ToString();
                foreach (DataRow col in cols)
                {
                    ColumnModel column = new ColumnModel();
                    column.IsPk = primarykey == col["ColName"].ToString();
                    column.ColName = col["ColName"].ToString();
                    column.ColType = col["ColType"].ToString();
                    column.ColComment = col["ColComment"].ToString();
                    column.ColDefault = col["ColDefault"].ToString();
                    table.columns.Add(column);
                }
                tables.Add(table);
            }
            return tables;

        }

        private static DataTable Query(string sqlString)
        {
            DataTable dt = new DataTable();
            // 数据库连接字符串
            using (SqlConnection conn = new SqlConnection(_connstr))
            {
                using (SqlCommand command = conn.CreateCommand())
                {
                    command.CommandText = sqlString;
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = command;
                    adapter.Fill(dt);
                }
            }
            return dt;
        }

        private static string GetCSType(string sqlType)
        {
            switch (sqlType)
            {
                case "datetime":
                    return "DateTime";
                case "int":
                    return "int";
                case "nchar":
                    return "string";
                case "nvarchar":
                    return "string";
                case "varchar":
                    return "string";
                case "text":
                    return "string";
                case "ntext":
                    return "string";
                case "uniqueidentifier":
                    return "Guid";
                case "decimal":
                    return "decimal";
                case "float":
                    return "float";
                case "bit":
                    return "byte";
                case "binary":
                    return "byte []";
                case "varbinary":
                    return "byte []";
                case "timestamp":
                    return "int";
                default:
                    return "";
            }
        }

        private static string GetCSDefault(string sqlValue)
        {
            switch (sqlValue)
            {
                case "((0))":
                    return "= 0;";
                case "('')":
                    return "= string.Empty;";
                case "('00000000-0000-0000-0000-000000000000')":
                    return "= Guid.Empty;";
                default:
                    return "";
            }
        }
    }

    public class TableModel
    {
        public string name { get; set; }
        public List<ColumnModel> columns { get; set; }
    }

    public class ColumnModel
    {
        public bool IsPk { get; set; }
        public string ColName { get; set; }
        public string ColType { get; set; }
        public string ColComment { get; set; }
        public string ColDefault { get; set; }
    }
}

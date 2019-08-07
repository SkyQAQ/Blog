using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.Linq;
using Blog.Core.Model;

namespace Blog.Core.Common
{
    /// <summary>
    /// 数据库帮助类
    /// </summary>
    public class SqlHelper: IDisposable
    {
        #region 初始化
        private LogHelper _log = null;
        private UserIdentity _identity = null;
        private SqlConnection connection = null;
        private SqlTransaction transaction = null;
        public SqlHelper(bool IsReadOnly = false)
        {
            _log = new LogHelper();
            ConfigHelper config = new ConfigHelper();
            if (IsReadOnly)
                connection = new SqlConnection(config.MSSQL_Readonly);
            else
                connection = new SqlConnection(config.MSSQL);
        }
        public SqlHelper(UserIdentity identity)
        {
            ConfigHelper config = new ConfigHelper();
            _identity = identity;
            connection = new SqlConnection(config.MSSQL);
        }
        public SqlHelper(LogHelper log, bool IsReadOnly = false)
        {
            ConfigHelper config = new ConfigHelper();
            _log = log;
            if (IsReadOnly)
                connection = new SqlConnection(config.MSSQL_Readonly);
            else
                connection = new SqlConnection(config.MSSQL);
        }
        public SqlHelper(LogHelper log, ConfigHelper config, UserIdentity identity)
        {
            _log = log;
            _identity = identity;
            connection = new SqlConnection(config.MSSQL);
        }
        #endregion

        /// <summary>
        /// 打开连接
        /// </summary>
        public void OpenDb()
        {
            connection.Open();
        }
        /// <summary>
        /// 关闭连接
        /// </summary>
        public void CloseDb()
        {
            connection.Close();
        }
        /// <summary>
        /// DISPOSE
        /// </summary>
        public void Dispose()
        {
            connection.Close();
            if (transaction != null)
            {
                transaction.Dispose();
            }
        }

        /// <summary>
        /// 开启事务
        /// </summary>
        /// <param name="transName"></param>
        public void BeginTrans(string transName = "")
        {
            if (!string.IsNullOrEmpty(transName))
            {
                transaction = connection.BeginTransaction(transName);
            }
            else
            {
                transaction = connection.BeginTransaction();
            }
        }
        /// <summary>
        /// 提交事务
        /// </summary>
        public void Commit()
        {
            if (transaction != null)
            {
                transaction.Commit();
                transaction = null;
            }
        }
        /// <summary>
        /// 回滚事务
        /// </summary>
        public void Rollback()
        {
            if (transaction != null)
            {
                transaction.Rollback();
                transaction = null;
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sqlString">SQL语句</param>
        /// <returns>DataTable</returns>
        public DataTable Query(string sqlString) =>
            Query(sqlString, null);
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sqlString">SQL语句</param>
        /// <param name="paramList">参数</param>
        /// <returns>DataTable</returns>
        public DataTable Query(string sqlString, Dictionary<string, object> paramList)
        {
            try
            {
                DataTable dt = new DataTable();
                DateTime start = DateTimeUtils.NowBeijing();
                string parameters = string.Empty;
                using (SqlCommand command = connection.CreateCommand())
                {
                    if (paramList != null && paramList.Count > 0)
                    {
                        foreach (var param in paramList)
                        {
                            SqlParameter parameter = new SqlParameter();
                            parameter.ParameterName = param.Key;
                            parameter.Value = param.Value;
                            command.Parameters.Add(parameter);
                            parameters += param.Key + ":" + param.Value + ",";
                        }
                    }
                    command.CommandText = sqlString;
                    if (transaction != null)
                    {
                        command.Transaction = transaction;
                    }
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = command;
                    adapter.Fill(dt);
                }
                DateTime end = DateTimeUtils.NowBeijing();
                double ss = (double)(end - start).Milliseconds / 1000;
                if (_log != null)
                {
                    _log.Debug(string.Format("[{0}s]{1}\n{2}", ss, sqlString, parameters));
                    if (ss > 10)
                    {
                        _log.WriteLog(string.Format("[{0}s]{1}\n{2}", ss, sqlString, parameters), "", "超过10秒的SQL" + DateTimeUtils.NowBeijing().ToString("yyyyMMddHH"));
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                if (_log != null)
                {
                    _log.Error(ex.Message, ex);
                    _log.Error(sqlString);
                }
                throw ex;
            }
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="sqlString">SQL语句</param>
        /// <param name="paramList">参数</param>
        /// <param name="orderBy">排序</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="recordCount">记录数</param>
        /// <returns>DataTable</returns>
        public DataTable Query(string sqlString, Dictionary<string, object> paramList, string orderBy, int pageSize, int pageIndex, out int recordCount)
        {
            if (pageSize <= 0 || pageIndex <= 0)
            {
                recordCount = 0;
                return null;
            }
            int startIndex = (pageIndex - 1) * pageSize;
            int endIndx = pageIndex * pageSize + 1;
            DataTable dtcount = Query(string.Format(@"SELECT COUNT(1) FROM({0}) main", sqlString), paramList);
            sqlString = string.Format(@"SELECT *
                                  FROM   (SELECT Row_number()
                                                   OVER (
                                                     {0} ) rownum,
                                                 main.*
                                          FROM   ({1}) main) pagedata
                                  WHERE pagedata.rownum > {2} AND pagedata.rownum < {3}", orderBy, sqlString, startIndex, endIndx);
            
            recordCount = Cast.ConToInt(dtcount.Rows[0][0]);
            return Query(sqlString, paramList);
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>int</returns>
        public int Execute(string sql) =>
            Execute(sql, null);
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="paramList">参数</param>
        /// <returns>int</returns>
        public int Execute(string sql, Dictionary<string, object> paramList)
        {
            try
            {
                int result = 0;
                DateTime start = DateTimeUtils.NowBeijing();
                string parameters = string.Empty;
                using (SqlCommand command = connection.CreateCommand())
                {
                    if (paramList != null && paramList.Count > 0)
                    {
                        foreach (var param in paramList)
                        {
                            SqlParameter parameter = new SqlParameter();
                            parameter.ParameterName = param.Key;
                            parameter.Value = param.Value;
                            command.Parameters.Add(parameter);
                            parameters += param.Key + ":" + param.Value + ",";
                        }
                    }
                    command.CommandText = sql;
                    if (transaction != null)
                    {
                        command.Transaction = transaction;
                    }
                    result = command.ExecuteNonQuery();
                }
                DateTime end = DateTimeUtils.NowBeijing();
                double ss = (double)(end - start).Milliseconds / 1000;
                if (_log != null)
                {
                    _log.Debug(string.Format("[{0}s]{1}\n{2}", ss, sql, parameters));
                    if (ss > 10)
                    {
                        _log.WriteLog(string.Format("[{0}s]{1}\n{2}", ss, sql, parameters), "", "超过10秒的SQL" + DateTimeUtils.NowBeijing().ToString("yyyyMMddHH"));
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                if (_log != null)
                {
                    _log.Error(ex.Message, ex);
                    _log.Error(sql);
                }
                throw ex;
            }
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model">数据</param>
        /// <param name="tableName">表名</param>
        /// <param name="account">操作人账号</param>
        /// <returns>数据Id</returns>
        public Guid Create(BaseModel model)
        {
            string account = _identity == null ? "WUWUYAOYAO" : _identity.UserAccount;
            Guid guid = model.BaseId == null || model.BaseId == Guid.Empty ? Guid.NewGuid() : model.BaseId;
            StringBuilder sb = new StringBuilder();
            DateTime date = DateTimeUtils.NowBeijing();
            string[] exceptColoumns = { model.ModelName + "Id", "CreatedOn", "CreatedBy", "ModifiedOn", "ModifiedBy", "Id", "BaseId", "ModelName" };
            sb.AppendFormat("INSERT INTO {0} ({0}Id, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy", model.ModelName);
            foreach (PropertyInfo property in model.GetType().GetProperties())
            {
                if (exceptColoumns.Contains(property.Name))
                    continue;
                sb.Append("," + property.Name);
            }
            sb.AppendFormat(") VALUES(@{0}Id, @CreatedOn, @CreatedBy, @ModifiedOn, @ModifiedBy", model.ModelName);
            Dictionary<string, object> paramList = new Dictionary<string, object>()
            {
                { "@" + model.ModelName + "Id", guid },
                { "@CreatedOn", date },
                { "@CreatedBy", account },
                { "@ModifiedOn", date },
                { "@ModifiedBy", account },
            };
            foreach (PropertyInfo property in model.GetType().GetProperties())
            {
                if (exceptColoumns.Contains(property.Name))
                    continue;
                sb.Append(",@" + property.Name);
                if (property.PropertyType.Name.ToLower() == "datetime")
                {
                    if((DateTime)property.GetValue(model, null) != DateTime.MinValue)
                        paramList.Add("@" + property.Name, property.GetValue(model, null)); 
                    else
                        paramList.Add("@" + property.Name, Cast.NullDateTime);
                }
                else if (property.PropertyType.Name.ToLower() == "guid")
                {
                    paramList.Add("@" + property.Name, property.GetValue(model, null));
                }
                else
                {
                    if (property.GetValue(model, null) != null)
                        paramList.Add("@" + property.Name, property.GetValue(model, null));
                    else
                        sb.Replace("," + property.Name, "").Replace(",@" + property.Name, "");
                }
            }
            sb.Append(")");
            Execute(sb.ToString(), paramList);
            return guid;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">数据Id</param>
        /// <param name="tableName">表名</param>
        /// <returns>int</returns>
        public int Delete(string id, string tableName)
        {
            string account = _identity == null ? "WUWUYAOYAO" : _identity.UserAccount;
            return Execute(string.Format(@"DELETE FROM {0} WHERE {0}Id = @Id", tableName), 
                new Dictionary<string, object> { { "@Id", id } });
        }    
        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model">数据</param>
        /// <param name="tableName">表名</param>
        /// <param name="account">操作人账号</param>
        /// <returns>数据Id</returns>
        public int Update(BaseModel model)
        {
            string account = _identity == null ? "WUWUYAOYAO" : _identity.UserAccount;
            StringBuilder sb = new StringBuilder();
            DateTime date = DateTimeUtils.NowBeijing();
            string[] exceptColoumns = { model.ModelName + "Id", "CreatedOn", "CreatedBy", "ModifiedOn", "ModifiedBy", "Id", "BaseId", "ModelName" };
            sb.AppendFormat("UPDATE {0} SET ModifiedOn = @ModifiedOn, ModifiedBy = @ModifiedBy", model.ModelName);
            Dictionary<string, object> paramList = new Dictionary<string, object>()
            {
                { "@ModifiedOn", date },
                { "@ModifiedBy", account },
            };
            foreach (PropertyInfo property in model.GetType().GetProperties())
            {
                if (exceptColoumns.Contains(property.Name))
                    continue;
                sb.AppendFormat(",{0} = @{0}", property.Name);
                paramList.Add("@" + property.Name, property.GetValue(model, null));
            }
            sb.AppendFormat(" WHERE {0}Id = @{0}Id", model.ModelName);
            paramList.Add("@" + model.ModelName + "Id", model.BaseId);
            if (model.BaseId == Guid.Empty)
            {
                throw new Exception(model.ModelName + "Id:值为null！");
            }
            return Execute(sb.ToString(), paramList);
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Id">单据Id</param>
        /// <returns></returns>
        public T Search<T>(string Id) where T : BaseModel, new()
        {
            List<T> list = Search<T>(string.Format("SELECT * FROM {0} WHERE IsDeleted = 0 AND {0}Id = @Id", new T().ModelName), new Dictionary<string, object> { { "@Id", Id } });
            if (list.Count > 0)
            {
                return list[0];
            }
            return null;
        }
        public List<T> Search<T>(string sqlString, Dictionary<string, object> paramList) where T : new()
        {
            List<T> result = new List<T>();
            DataTable dt = Query(sqlString, paramList);
            if (dt != null && dt.Rows.Count > 0)
            {
                result = dt.ToModelList<T>();
            }
            return result;
        }
        /// <summary>
        /// 启用
        /// </summary>
        /// <param name="id">数据Id</param>
        /// <param name="tableName">表名</param>
        /// <returns>int</returns>
        public int Enable(string id, string tableName)
        {
            string account = _identity == null ? "WUWUYAOYAO" : _identity.UserAccount;
            return Execute(string.Format(@"UPDATE {0} SET IsDeleted = 0, ModifiedOn = @nowtime, ModifiedBy = @ModifiedBy  WHERE {0}Id = @Id", tableName),
                new Dictionary<string, object> { { "@ModifiedBy", account }, { "@Id", id }, { "@nowtime", DateTimeUtils.NowBeijing() } });
        }
        /// <summary>
        /// 禁用
        /// </summary>
        /// <param name="id">数据Id</param>
        /// <param name="tableName">表名</param>
        /// <returns>int</returns>
        public int Disable(string id, string tableName)
        {
            string account = _identity == null ? "WUWUYAOYAO" : _identity.UserAccount;
            return Execute(string.Format(@"UPDATE {0} SET IsDeleted = 1, ModifiedOn = @nowtime, ModifiedBy = @ModifiedBy  WHERE {0}Id = @Id", tableName),
                new Dictionary<string, object> { { "@ModifiedBy", account }, { "@Id", id }, { "@nowtime", DateTimeUtils.NowBeijing() } });
        }

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="tableName">插入数据表名</param>
        /// <param name="dataTable">插入数据</param>
        public void BulkCopy(string tableName, DataTable dataTable)
        {
            using(SqlBulkCopy bulkcopy = new SqlBulkCopy(connection))
            {
                // 每个批处理中的行数。 在每个批处理结束时，批处理中的行将发送到服务器
                bulkcopy.BatchSize = 10000;
                // 目标表
                bulkcopy.DestinationTableName = tableName;
                // 开始插入
                bulkcopy.WriteToServer(dataTable);
            }
        }
    }
}

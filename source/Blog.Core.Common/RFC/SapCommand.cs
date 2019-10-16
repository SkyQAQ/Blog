using System;
using System.Collections.Generic;
using System.Text;
using SAP.Middleware.Connector;
using System.Data;
using System.Linq;
namespace Blog.Core.Common
{
    /// <summary>
    /// SAPCommand
    /// </summary>
    public static class SAPCommand
    {
        /// <summary>
        /// 连接目标
        /// </summary>
        private static RfcDestination destination = null;

        /// <summary>
        /// 获取连接SAP参数
        /// </summary>
        /// <returns></returns>
        private static RfcConfigParameters GetRfcConfigParameters()
        {
            RfcConfigParameters pairs = new RfcConfigParameters();
            pairs.Add(RfcConfigParameters.Name, "SAP连接名");
            pairs.Add(RfcConfigParameters.AppServerHost, "SAP服务器地址");
            pairs.Add(RfcConfigParameters.SystemNumber, "00");
            pairs.Add(RfcConfigParameters.SystemID, "D01");
            pairs.Add(RfcConfigParameters.User, "SAP账号");
            pairs.Add(RfcConfigParameters.Password, "SAP密码");
            pairs.Add(RfcConfigParameters.Client, "客户端");
            pairs.Add(RfcConfigParameters.Language, "en");
            pairs.Add(RfcConfigParameters.PoolSize, "5");
            pairs.Add(RfcConfigParameters.MaxPoolSize, "10");
            pairs.Add(RfcConfigParameters.IdleTimeout, "30");
            return pairs;
        }

        /// <summary>
        /// 初始化SAP连接
        /// </summary>
        /// <returns></returns>
        private static void InitialRfcDestination()
        {
            if (destination == null) 
                destination = RfcDestinationManager.GetDestination(GetRfcConfigParameters());
        }

        /// <summary>
        /// Get SAP Datatable Info
        /// </summary>
        /// <param name="rfcFuctionName">SAP Function Module</param>
        /// <param name="rfcTableName">SAP Function Group</param>
        /// <param name="keyValues">SAP所需参数</param>
        /// <returns>DataTable</returns>
        public static DataTable GetDatatableFromSAP(string rfcFuctionName, string rfcTableName, Dictionary<string, object> keyValues = null) =>
            GetDataSetFromSAP(rfcFuctionName, new List<string> { rfcTableName }, keyValues)[rfcTableName];

        /// <summary>
        /// Get SAP DataSet Info（SAP返回多个表）
        /// </summary>
        /// <param name="rfcFuctionName">SAP Function Module</param>
        /// <param name="rfcTableNameList">SAP Function Groups</param>
        /// <param name="keyValues">SAP所需参数</param>
        /// <returns>Dictionary<string ,DataTable></returns>
        public static Dictionary<string, DataTable> GetDataSetFromSAP(string rfcFuctionName, List<string> rfcTableNameList, Dictionary<string, object> keyValues = null)
        {
            if (string.IsNullOrEmpty(rfcFuctionName) || rfcTableNameList == null || rfcTableNameList.Count <= 0)
                return null;
            List<string> rfcTableNames = rfcTableNameList.Distinct().ToList();
            try
            {
                InitialRfcDestination();
                Dictionary<string, DataTable> result = new Dictionary<string, DataTable>();
                Dictionary<string, IRfcTable> rfcTableDic = new Dictionary<string, IRfcTable>();
                IRfcFunction func = destination.Repository.CreateFunction(rfcFuctionName);
                if (keyValues != null && keyValues.Count > 0)
                {
                    foreach (var item in keyValues)
                    {
                        func.SetValue(item.Key, item.Value);
                    }
                }
                rfcTableNames.ForEach(item =>
                {
                    IRfcTable rfcTable = func.GetTable(item);
                    rfcTableDic.Add(item, rfcTable);
                });
                func.Invoke(destination);
                rfcTableNames.ForEach(item =>
                {
                    result.Add(item, GetDataTableFromRFCTable(rfcTableDic[item]));
                });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 转换IRfcTable为Datatable
        /// </summary>
        /// <param name="myrfcTable"></param>
        /// <returns></returns>
        private static DataTable GetDataTableFromRFCTable(IRfcTable myrfcTable)
        {

            DataTable loTable = new DataTable();
            int liElement = 0;
            for (liElement = 0; liElement <= myrfcTable.ElementCount - 1; liElement++)
            {
                RfcElementMetadata metadata = myrfcTable.GetElementMetadata(liElement);
                loTable.Columns.Add(metadata.Name);
            }
            foreach (IRfcStructure Row in myrfcTable)
            {
                DataRow ldr = loTable.NewRow();
                for (liElement = 0; liElement <= myrfcTable.ElementCount - 1; liElement++)
                {
                    RfcElementMetadata metadata = myrfcTable.GetElementMetadata(liElement);
                    ldr[metadata.Name] = Row.GetString(metadata.Name);
                }
                loTable.Rows.Add(ldr);
            }
            return loTable;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Blog.Core.Common;
using Blog.Core.Model;

namespace Blog.Core.Biz.Role
{
    /// <summary>
    /// 角色Command
    /// </summary>
    public class RoleInfoCommand : BaseCommand
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="identity"></param>
        public RoleInfoCommand(UserIdentity identity) : base(identity)
        {
        }

        /// <summary>
        /// 获取角色信息List
        /// </summary>
        /// <returns></returns>
        public RoleInfoListData GetRoleInfoList(List<SearchCondition> searchList, int pageIndex, int pageSize, string orderBy)
        {
            try
            {
                RoleInfoListData result = new RoleInfoListData();

                #region 初始化数据
                string sqlString = @"SELECT * FROM RoleInfo WHERE IsDeleted = 0";
                Dictionary<string, object> paramList = new Dictionary<string, object>();
                #endregion

                #region 条件过滤
                if (searchList != null && searchList.Count > 0)
                {
                    searchList.ForEach(s =>
                    {
                        switch (s.Type)
                        {
                            case Constants.COMMON_SEARCHCONDITIONTYPE_EQUAL:
                                sqlString += string.Concat(" AND ", s.Table, s.Key, " = @", s.Key);
                                paramList.Add(string.Concat("@", s.Key), s.Value);
                                break;
                            case Constants.COMMON_SEARCHCONDITIONTYPE_LIKE:
                                sqlString += string.Concat(" AND ", s.Table, s.Key, " like @", s.Key);
                                paramList.Add(string.Concat("@", s.Key), s.Value + "%");
                                break;
                            case Constants.COMMON_SEARCHCONDITIONTYPE_RANGE:
                                if (s.Key.ToLower().Contains("after"))
                                {
                                    sqlString += string.Concat(" AND ", s.Table, s.Key.Replace("after", ""), " >= @", s.Key);
                                    paramList.Add(string.Concat("@", s.Key), Cast.ConToDateTime(s.Value).ToString("yyyy-MM-dd HH:mm:ss"));
                                }
                                else if (s.Key.ToLower().Contains("before"))
                                {
                                    sqlString += string.Concat(" AND ", s.Table, s.Key.Replace("before", ""), " <= @", s.Key);
                                    paramList.Add(string.Concat("@", s.Key), Cast.ConToDateTime(s.Value).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"));
                                }
                                else
                                {
                                    throw new Exception("调用的参数错误，无法认定区间起始:" + s.Value);
                                }
                                break;
                            case Constants.COMMON_SEARCHCONDITIONTYPE_CUSTOMER:
                                break;
                        }
                    });
                }
                #endregion

                #region 查询数据
                DataTable dtResult = null;
                int recordCount = 0;
                if (pageSize > 0 && pageIndex > 0)
                {
                    dtResult = _sql.Query(sqlString, paramList, orderBy, pageSize, pageIndex, out recordCount);
                }
                else
                {
                    sqlString += orderBy;
                    dtResult = _sql.Query(sqlString, paramList);
                }
                #endregion

                #region 构建结果
                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    if (pageSize > 0 && pageIndex > 0)
                    {
                        result.RecordCount = recordCount;
                        result.RecordList = dtResult.ToModelList<RoleInfoListModel>();
                    }
                    else
                    {
                        result.ExportBuffString = ExcelHelper.DownloadExcel(dtResult);
                        result.RecordCount = dtResult.Rows.Count;
                    }
                }
                #endregion

                return result;
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// 根据角色Id获取角色编辑信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RoleInfoEditModel GetRoleInfoById(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    throw new Exception("角色Id不能为空！");
                }
                RoleInfoEditModel result = new RoleInfoEditModel();
                DataTable dt = _sql.Query("SELECT RoleName,RoleCode FROM RoleInfo WHERE IsDeleted = 0 AND RoleInfoId = @id", new Dictionary<string, object> { { "@id", id } });
                if (dt != null && dt.Rows.Count > 0)
                {
                    result.RoleInfoId = id;
                    result.RoleName = Cast.ConToString(dt.Rows[0]["RoleName"]);
                    result.RoleCode = Cast.ConToString(dt.Rows[0]["RoleCode"]);
                }
                return result;
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// 创建或更新角色信息
        /// </summary>
        /// <param name="info">角色编辑信息</param>
        /// <returns></returns>
        public string CreateOrUpdateRoleInfo(RoleInfoEditModel info)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(info.RoleName))
                    throw new Exception("角色名称不能为空！");
                if (string.IsNullOrWhiteSpace(info.RoleCode))
                    throw new Exception("角色编码不能为空！");
                _sql.OpenDb();
                string result = string.Empty;
                RoleInfo roleInfo = new RoleInfo();
                info.FillTableWithModel<RoleInfo>(roleInfo);
                if (!string.IsNullOrEmpty(info.RoleInfoId))
                {
                    _sql.Update(roleInfo);
                    result = Constants.UpdateSuccessMssg;
                }
                else
                {
                    DataTable dt = _sql.Query("SELECT RoleName FROM RoleInfo WHERE IsDeleted = 0 AND RoleCode = @code", new Dictionary<string, object> { { "@code", info.RoleCode } });
                    if (dt != null && dt.Rows.Count > 0)
                        throw new Exception(string.Format("当前角色【{0}:{1}】已存在！", info.RoleCode, Cast.ConToString(dt.Rows[0]["RoleName"])));
                    _sql.Create(roleInfo);
                    result = Constants.CreateSuccessMssg;
                }
                return result;
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                throw ex;
            }
            finally
            {
                _sql.CloseDb();
            }
        }

        /// <summary>
        /// 删除角色信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public string DeleteRoleInfo(string[] ids)
        {
            try
            {
                _sql.OpenDb();
                _sql.BeginTrans();
                if (ids == null || ids.Length == 0)
                {
                    throw new Exception("角色Id不能为空！");
                }
                foreach(string id in ids)
                {
                    _sql.Execute("DELETE FROM UserInRole WHERE RoleInfoId = @id", new Dictionary<string, object> { { "@id", id } });
                    _sql.Execute("DELETE FROM MenuInRole WHERE RoleInfoId = @id", new Dictionary<string, object> { { "@id", id } });
                    _sql.Execute("DELETE FROM RoleInfo WHERE RoleInfoId = @id", new Dictionary<string, object> { { "@id", id } });
                }
                _sql.Commit();
                return Constants.DeleteSuccessMssg;
            }
            catch (Exception ex)
            {
                _sql.Rollback();
                _log.Error(ex);
                throw ex;
            }
            finally
            {
                _sql.CloseDb();
            }
        }

        /// <summary>
        /// 根据角色Id获取菜单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string[] GetMenuInfoList(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    throw new Exception("角色Id不能为空！");
                }
                DataTable dt = _sql.Query("SELECT MenuInfoId FROM MenuInRole WITH(Nolock) WHERE IsDeleted = 0 AND RoleInfoId = @id", new Dictionary<string, object> { { "@id", id } });
                if (dt != null && dt.Rows.Count > 0)
                {
                    string ids = string.Empty;
                    foreach(DataRow row in dt.Rows)
                    {
                        ids += Cast.ConToString(row["MenuInfoId"]) + ",";
                    }
                    return ids.Substring(0, ids.Length - 1).Split(',');
                }
                else
                {
                    return new string[] { };
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// 创建角色与菜单对应关系信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public string CreateMIR(MIREditModel info)
        {
            try
            {
                if (info == null || string.IsNullOrEmpty(info.roleInfoId))
                {
                    throw new Exception("角色Id不能为空！");
                }
                RoleInfo role = _sql.Search<RoleInfo>(info.roleInfoId);
                _sql.OpenDb();
                _sql.BeginTrans();
                _sql.Execute("DELETE FROM MenuInRole WHERE RoleInfoId = @id", new Dictionary<string, object> { { "@id", info.roleInfoId } });
                if (info.menuInfoIds != null && info.menuInfoIds.Length > 0)
                {
                    foreach (string menuInfoId in info.menuInfoIds)
                    {
                        MenuInfo menu = _sql.Search<MenuInfo>(menuInfoId);
                        MenuInRole main = new MenuInRole();
                        main.RoleInfoId = role.RoleInfoId;
                        main.RoleCode = role.RoleCode;
                        main.MenuInfoId = menu.MenuInfoId;
                        main.MenuCode = menu.MenuCode;
                        _sql.Create(main);
                    }
                }
                _sql.Commit();
                return Constants.SaveSuccessMssg;
            }
            catch (Exception ex)
            {
                _sql.Rollback();
                _log.Error(ex);
                throw ex;
            }
            finally
            {
                _sql.CloseDb();
            }
        }

        /// <summary>
        /// 导入角色
        /// </summary>
        /// <param name="importData"></param>
        /// <returns></returns>
        public string UploadRole(DataTable importData)
        {
            try
            {
                StringBuilder sbResult = new StringBuilder();

                #region Excel数据验证
                if (importData == null || importData.Rows.Count == 0)
                {
                    throw new Exception("请维护导入数据！");
                }
                if(!importData.Columns.Contains("角色编码"))
                {
                    throw new Exception("请维护列【角色编码】！");
                }
                if (!importData.Columns.Contains("角色名称"))
                {
                    throw new Exception("请维护列【角色名称】！");
                }
                #endregion

                #region 初始化数据
                _sql.OpenDb();
                int index = 2;// 导入数据的行数(数据从excel中第二行读取)
                int totalCount = importData.Rows.Count;// 待导入的数据量
                int createCount = 0;// 成功创建数据量
                int updateCount = 0;// 成功更新数据量
                int unoperateCount = 0;// 未操作数据量
                // 导入开始...
                sbResult.Append("导入开始 \n");
                // totalCount条数据等待导入
                sbResult.Append(totalCount + "条数据等待导入 \n");
                #endregion

                #region BulkCopy测试
                //List<RoleInfo> list = new List<RoleInfo>();
                //foreach (DataRow row in importData.Rows)
                //{
                //    RoleInfo role = new RoleInfo();
                //    role.RoleInfoId = Guid.NewGuid();
                //    role.RoleCode = Cast.ConToString(row["RoleCode"]);
                //    role.RoleName = Cast.ConToString(row["RoleName"]);
                //    role.CreatedOn = DateTimeUtils.NowBeijing();
                //    role.ModifiedOn = DateTimeUtils.NowBeijing();
                //    role.CreatedBy = _identity.UserAccount;
                //    role.ModifiedBy = _identity.UserAccount;
                //    //_sql.Create(role);
                //    list.Add(role);
                //}
                //_sql.BulkCopy(RoleInfo.TableName, list.FillDataTable<RoleInfo>());
                #endregion

                #region 数据导入
                foreach (DataRow row in importData.Rows)
                {
                    if (string.IsNullOrWhiteSpace(Cast.ConToString(row["角色编码"])))
                    {
                        sbResult.AppendFormat("第{0}行数据角色编码为空！ \n", index);
                        unoperateCount++;
                        index++;
                        continue;
                    }
                    if (string.IsNullOrWhiteSpace(Cast.ConToString(row["角色名称"])))
                    {
                        sbResult.AppendFormat("第{0}行数据角色名称为空！ \n", index);
                        unoperateCount++;
                        index++;
                        continue;
                    }
                    RoleInfo role = new RoleInfo();
                    role.RoleCode = Cast.ConToString(row["角色编码"]);
                    role.RoleName = Cast.ConToString(row["角色名称"]);
                    DataTable dtrole = _sql.Query("SELECT RoleInfoId FROM RoleInfo WHERE IsDeleted = 0 AND RoleCode = @code", new Dictionary<string, object> { { "@code", Cast.ConToString(row["角色编码"]) } });
                    if (dtrole != null && dtrole.Rows.Count > 0)
                    {
                        role.RoleInfoId = new Guid(Cast.ConToString(dtrole.Rows[0]["RoleInfoId"]));
                        _sql.Update(role);
                        updateCount++;
                    }
                    else
                    {
                        _sql.Create(role);
                        createCount++;
                    }
                }
                sbResult.AppendFormat("成功创建{0}条记录 \n", createCount);
                sbResult.AppendFormat("成功更新{0}条记录 \n", updateCount);
                sbResult.AppendFormat("未操作{0}条记录 \n", unoperateCount);
                #endregion

                return sbResult.ToString();
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex);
                throw ex;
            }
            finally
            {
                _sql.CloseDb();
            }
        }

        /// <summary>
        /// 获取角色穿梭框数据
        /// </summary>
        /// <returns></returns>
        public List<TransferModel> GetRoleTransferData()
        {
            try
            {
                DataTable dt = _sql.Query("SELECT RoleInfoId AS 'key',RoleName AS label,RoleCode AS search FROM RoleInfo WHERE IsDeleted = 0");
                return dt.ToModelList<TransferModel>();
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                throw ex;
            }
        }
    }
}

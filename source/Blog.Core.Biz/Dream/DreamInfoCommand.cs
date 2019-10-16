using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Blog.Core.Common;
using Blog.Core.Model;

namespace Blog.Core.Biz.Dream
{
    /// <summary>
    /// 梦想Command
    /// </summary>
    public class DreamInfoCommand : BaseCommand
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="identity"></param>
        public DreamInfoCommand(UserIdentity identity) : base(identity)
        {
        }

        /// <summary>
        /// 获取梦想信息List
        /// </summary>
        /// <returns></returns>
        public DreamInfoListData GetDreamInfoList(List<SearchCondition> searchList, int pageIndex, int pageSize, string orderBy)
        {
            try
            {
                DreamInfoListData result = new DreamInfoListData();

                #region 初始化数据
                string sqlString = @"SELECT * FROM DreamInfo WHERE IsDeleted = 0 AND UserInfoId = @userId";
                Dictionary<string, object> paramList = new Dictionary<string, object> { { "@userId", _identity.UserId } };
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
                                    paramList.Add(string.Concat("@", s.Key), Cast.ConToInt(s.Value));
                                }
                                else if (s.Key.ToLower().Contains("before"))
                                {
                                    sqlString += string.Concat(" AND ", s.Table, s.Key.Replace("before", ""), " <= @", s.Key);
                                    paramList.Add(string.Concat("@", s.Key), Cast.ConToInt(s.Value));
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
                    Dictionary<int, string> dic = new Dictionary<int, string>();
                    dic.Add(DreamInfoEnum.Type.DLT.GetHashCode(), "大乐透");
                    dic.Add(DreamInfoEnum.Type.PL3.GetHashCode(), "排列三");
                    dic.Add(DreamInfoEnum.Type.PL5.GetHashCode(), "排列五");
                    dtResult.Columns.Add("TypeText", typeof(string));
                    for (int i = 0;i < dtResult.Rows.Count; i++)
                    {
                        dtResult.Rows[i]["TypeText"] = dic[Cast.ConToInt(dtResult.Rows[i]["Type"])];
                    }
                    if (pageSize > 0 && pageIndex > 0)
                    {
                        result.RecordCount = recordCount;
                        result.RecordList = dtResult.ToModelList<DreamInfoListModel>();
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
        /// 根据梦想Id获取梦想编辑信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DreamInfoEditModel GetDreamInfoById(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    throw new Exception("梦想Id不能为空！");
                }
                DreamInfoEditModel result = new DreamInfoEditModel();
                DataTable dt = _sql.Query("SELECT DreamCode,StartStage,EndStage,Type FROM DreamInfo WHERE IsDeleted = 0 AND DreamInfoId = @id", new Dictionary<string, object> { { "@id", id } });
                if (dt != null && dt.Rows.Count > 0)
                {
                    result.DreamInfoId = id;
                    result.DreamCode = Cast.ConToString(dt.Rows[0]["DreamCode"]);
                    result.StartStage = Cast.ConToInt(dt.Rows[0]["StartStage"]);
                    result.EndStage = Cast.ConToInt(dt.Rows[0]["EndStage"]);
                    result.Type = Cast.ConToInt(dt.Rows[0]["Type"]);
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
        /// 创建或更新梦想信息
        /// </summary>
        /// <param name="info">梦想编辑信息</param>
        /// <returns></returns>
        public string CreateOrUpdateDreamInfo(DreamInfoEditModel info)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(info.DreamCode))
                    throw new Exception("彩票号码不能为空！");
                info.DreamCode=info.DreamCode.Trim(' ');
                if(!info.DreamCode.Contains(' '))
                    throw new Exception("彩票号码以空格隔开！");
                if(info.Type == (int)DreamInfoEnum.Type.DLT && info.DreamCode.Split(' ').Length != 7)
                    throw new Exception("大乐透号码必须为7位！");
                else if (info.Type == (int)DreamInfoEnum.Type.PL3 && info.DreamCode.Split(' ').Length != 3)
                    throw new Exception("排列三号码必须为3位！");
                else if (info.Type == (int)DreamInfoEnum.Type.PL5 && info.DreamCode.Split(' ').Length != 5)
                    throw new Exception("排列五号码必须为5位！");
                _sql.OpenDb();
                string result = string.Empty;
                DreamInfo DreamInfo = new DreamInfo();
                info.FillTableWithModel<DreamInfo>(DreamInfo);
                if (!string.IsNullOrEmpty(info.DreamInfoId))
                {
                    DreamInfo.UserInfoId = Guid.Parse(_identity.UserId);
                    _sql.Update(DreamInfo);
                    result = Constants.UpdateSuccessMssg;
                }
                else
                {
                    DreamInfo.UserInfoId = Guid.Parse(_identity.UserId);
                    _sql.Create(DreamInfo);
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
        /// 删除梦想信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public string DeleteDreamInfo(string[] ids)
        {
            try
            {
                _sql.OpenDb();
                _sql.BeginTrans();
                if (ids == null || ids.Length == 0)
                {
                    throw new Exception("梦想Id不能为空！");
                }
                foreach (string id in ids)
                {
                    _sql.Delete(id, DreamInfo.TableName);
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
    }
}

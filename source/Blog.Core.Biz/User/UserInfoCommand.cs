using System;
using System.Data;
using System.Collections.Generic;
using Blog.Core.Model;
using Blog.Core.Common;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace Blog.Core.Biz.User
{
    /// <summary>
    /// 用户Command
    /// </summary>
    public class UserInfoCommand : BaseCommand
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="identity"></param>
        public UserInfoCommand(UserIdentity identity) : base(identity)
        {

        }

        /// <summary>
        /// 获取角色信息List
        /// </summary>
        /// <returns></returns>
        public UserListData GetUserList(List<SearchCondition> searchList, int pageIndex, int pageSize, string orderBy)
        {
            try
            {
                UserListData result = new UserListData();

                #region 初始化数据
                string sqlString = @"SELECT DISTINCT
                                            u.UserInfoId,
                                            u.Name    AS UserName,
                                            u.Account AS UserAccount,
                                            u.Email   AS UserEmail,
                                            CASE
                                              WHEN u.IsDeleted = 0 THEN '可用'
                                              ELSE '禁用'
                                            END       UserStatus
                                     FROM   UserInfo u
                                            LEFT JOIN UserInRole uir
                                                   ON u.UserInfoId = uir.UserInfoId
                                     WHERE  1 = 1 
                                     ";
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
                        result.RecordList = dtResult.ToModelList<UserListModel>();
                        result.RecordList.ForEach(item =>
                        {
                            item.UserRoles = GetUserRolesByUserId(item.UserInfoId);
                            DataTable dtLog = _sql.Query("SELECT TOP 1 LoginOn, ClientIp FROM tbl_loginlog WHERE UserInfoId = @userId Order By LoginOn DESC", new Dictionary<string, object> { { "@userId", item.UserInfoId } });
                            if (dtLog != null && dtLog.Rows.Count > 0)
                            {
                                item.LoginOn = Cast.ConToDateTimeString(dtLog.Rows[0]["LoginOn"]);
                                item.LoginClientIp = Cast.ConToString(dtLog.Rows[0]["ClientIp"]);
                            }
                            item.LoginStatus = CacheHelper.Exists("CHAT_" + item.UserInfoId.ToUpper()) ? "在线" : "离线";
                        });
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
        /// 启用用户
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public string EnableUser(string[] ids)
        {
            try
            {
                _sql.OpenDb();
                if (ids == null || ids.Length == 0)
                {
                    throw new Exception("角色Id不能为空！");
                }
                foreach (string id in ids)
                {
                    _sql.Execute("UPDATE UserInfo SET LoginFiledTimes = 0, IsDeleted = 0, ModifiedOn = @nowtime, ModifiedBy = @ModifiedBy  WHERE UserInfoId = @Id", 
                        new Dictionary<string, object> { { "@nowtime", DateTimeUtils.NowBeijing()}, { "@ModifiedBy", _identity.UserAccount }, { "@Id", id } });
                }
                return Constants.OperateSuccessMssg;
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
        /// 禁用用户
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public string DisableUser(string[] ids)
        {
            try
            {
                _sql.OpenDb();
                if (ids == null || ids.Length == 0)
                {
                    throw new Exception("角色Id不能为空！");
                }
                foreach (string id in ids)
                {
                    _sql.Disable(id, UserInfo.TableName);
                    if (CacheHelper.Exists(id.ToUpper()))
                    {
                        CacheHelper.Remove(id.ToUpper());
                    }
                }
                return Constants.OperateSuccessMssg;
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
        /// 根据用户信息获取用户角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private List<UserRoleModel> GetUserRolesByUserId(string userId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                {
                    throw new Exception("用户Id不能为空！");
                }
                DataTable dt = _sql.Query(@"SELECT uir.RoleInfoId,
                                                 uir.RoleCode,
                                                 r.RoleName
                                          FROM   UserInRole uir
                                                 INNER JOIN RoleInfo r
                                                         ON uir.RoleInfoId = r.RoleInfoId
                                          WHERE  uir.UserInfoId = @userId 
                                          ", new Dictionary<string, object> { { "@userId", userId } });
                return dt.ToModelList<UserRoleModel>();
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// 根据用户Id获取角色Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string[] GetRoleInfoList(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    throw new Exception("用户Id不能为空！");
                }
                DataTable dt = _sql.Query("SELECT RoleInfoId FROM UserInRole WITH(Nolock) WHERE IsDeleted = 0 AND UserInfoId = @id", new Dictionary<string, object> { { "@id", id } });
                if (dt != null && dt.Rows.Count > 0)
                {
                    string ids = string.Empty;
                    foreach (DataRow row in dt.Rows)
                    {
                        ids += Cast.ConToString(row["RoleInfoId"]) + ",";
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
        /// 创建用户与角色对应关系信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public string CreateUIR(UIREditModel info)
        {
            try
            {
                if (info == null || string.IsNullOrEmpty(info.userInfoId))
                {
                    throw new Exception("用户Id不能为空！");
                }
                UserInfo user = _sql.Search<UserInfo>(info.userInfoId);
                _sql.OpenDb();
                _sql.BeginTrans();
                _sql.Execute("DELETE FROM UserInRole WHERE UserInfoId = @id", new Dictionary<string, object> { { "@id", info.userInfoId } });
                if (info.roleInfoIds != null && info.roleInfoIds.Length > 0)
                {
                    foreach (string roleInfoId in info.roleInfoIds)
                    {
                        RoleInfo role = _sql.Search<RoleInfo>(roleInfoId);
                        UserInRole main = new UserInRole();
                        main.RoleInfoId = role.RoleInfoId;
                        main.RoleCode = role.RoleCode;
                        main.UserInfoId = user.UserInfoId;
                        main.UserCode = user.Account;
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
        /// 删除用户角色
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public string DeleteUserRole(string userId, string roleId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(roleId))
                {
                    throw new Exception("用户或角色Id不能为空！");
                }
                _sql.OpenDb();
                _sql.Execute("DELETE FROM UserInRole WHERE UserInfoId = @userId AND RoleInfoId = @roleId", new Dictionary<string, object> { { "@userId", userId }, { "@roleId", roleId } });
                return Constants.DeleteSuccessMssg;
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
        /// 更新用户信息
        /// </summary>
        /// <param name="name">用户名称</param>
        /// <param name="pwd">用户密码</param>
        /// <returns></returns>
        public string Save(string name, string pwd)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                    throw new Exception("请输入名称！");
                UserInfo user = _sql.Search<UserInfo>(_identity.UserId);
                user.Name = name;
                if (!string.IsNullOrEmpty(pwd))
                {
                    var instance = WuYao.GetSubClass(typeof(IAuthHelper));
                    if (instance != null)
                    {
                        var authInstance = instance as IAuthHelper;
                        authInstance.ChangePassword(_identity.UserAccount, WuYao.RsaDecrypt(pwd));
                    }
                    else
                    {
                        pwd = WuYao.GetPasswordCipher(pwd);
                        if (pwd.ToUpper() == user.Password.ToUpper())
                            throw new Exception("新密码不能与旧密码相同！");
                        user.Password = pwd;
                    }
                }
                _sql.OpenDb();
                _sql.Update(user);
                return Constants.UpdateSuccessMssg;
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
        /// 更换邮箱
        /// </summary>
        /// <param name="receive"></param>
        /// <param name="verifycode"></param>
        /// <returns></returns>
        public string ChangeEmail(string receive, string verifycode)
        {
            try
            {
                string valid = new AuthHelper().ValidReceiveVerifyCode(receive, Constants.CodeTypeChangeEmail, verifycode);
                if (!string.IsNullOrEmpty(valid))
                {
                    return valid;
                }
                _sql.OpenDb();
                _sql.Execute("UPDATE UserInfo SET Email = @email WHERE UserInfoId = @userId", new Dictionary<string, object> { { "@email", receive }, { "@userId", _identity.UserId } });
                return Constants.UpdateSuccessMssg;
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
        /// 更换头像
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public string ChangeAvatar(IFormFile file)
        {
            try
            {
                Attachments.AttachmentCommand helper = new Attachments.AttachmentCommand(_identity);
                UserInfo user = _sql.Search<UserInfo>(_identity.UserId);
                if (!string.IsNullOrEmpty(user.Avatar))
                {
                    helper.DeleteFile(new string[] { user.Avatar });
                }
                string attachMentId = helper.UploadFile(UserInfo.TableName, _identity.UserId, file, Constants.AvatarPath);
                _sql.OpenDb();
                _sql.Execute("UPDATE UserInfo SET Avatar = @avatar WHERE UserInfoId = @uid", new Dictionary<string, object> { { "@avatar", attachMentId }, { "@uid", _identity.UserId } });
                return attachMentId;
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
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using Blog.Core.Common;
using Blog.Core.Model;

namespace Blog.Core.Biz.Menu
{
    /// <summary>
    /// 菜单Command
    /// </summary>
    public class MenuInfoCommand : BaseCommand
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="identity"></param>
        public MenuInfoCommand(UserIdentity identity) : base(identity)
        {
        }

        /// <summary>
        /// 获取菜单树形结果数据
        /// </summary>
        /// <returns></returns>
        public List<TreeNode> GetMenuTreeData()
        {
            List<TreeNode> result = new List<TreeNode>();

            #region 查询所有菜单信息
            DataTable dtMenu = _sql.Query(@"SELECT MenuInfoId AS id,
                                                   MenuName   AS label,
                                                   CASE
                                                     WHEN PMenuId = '00000000-0000-0000-0000-000000000000' THEN null
                                                     ELSE PMenuId
                                                   END        AS pid
                                            FROM   MenuInfo WITH(nolock)
                                            ORDER  BY MenuSeq ASC ");
            #endregion

            #region 构建结果
            if (dtMenu != null && dtMenu.Rows.Count > 0)
            {
                result = dtMenu.ToTreeNode();
            }
            #endregion

            return result;
        }

        /// <summary>
        /// 根据用户Id获取菜单树形结果数据
        /// </summary>
        /// <returns></returns>
        public List<TreeNode> GetMenuTreeDataByUserId()
        {
            List<TreeNode> result = new List<TreeNode>();

            #region 查询所有菜单信息
            DataTable dtMenu = _sql.Query(@"SELECT DISTINCT m.MenuInfoId AS id,
                                                            m.MenuName   AS label,
                                                            m.MenuCode   AS label1,
                                                            m.MenuPath   AS label2,
                                                            CASE
                                                              WHEN m.PMenuId = '00000000-0000-0000-0000-000000000000' THEN NULL
                                                              ELSE m.PMenuId
                                                            END          AS pid,
                                                            m.MenuSeq
                                            FROM   UserInRole uir
                                                   INNER JOIN MenuInRole mir
                                                           ON uir.RoleInfoId = mir.RoleInfoId
                                                   INNER JOIN MenuInfo m
                                                           ON mir.MenuInfoId = m.MenuInfoId
                                            WHERE  uir.UserInfoId = @userId
                                            ORDER  BY m.MenuSeq ASC  ", new Dictionary<string, object> { { "@userId", _identity.UserId } });
            #endregion

            #region 构建结果
            if (dtMenu != null && dtMenu.Rows.Count > 0)
            {
                result = dtMenu.ToTreeNode();
            }
            #endregion

            return result;
        }

        /// <summary>
        /// 创建或更新菜单信息
        /// </summary>
        /// <param name="info">菜单编辑信息</param>
        /// <returns></returns>
        public string CreateOrUpdateMenuInfo(MenuInfoEditModel info)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(info.MenuName))
                    throw new Exception("菜单名称不能为空！");
                if (string.IsNullOrWhiteSpace(info.MenuCode))
                    throw new Exception("菜单编码不能为空！");
                if (string.IsNullOrWhiteSpace(info.MenuPath))
                    throw new Exception("菜单路径不能为空！");
                if (Cast.ConToInt(info.MenuSeq) == 0)
                    throw new Exception("菜单排序不能为空！");
                _sql.OpenDb();
                string result = string.Empty;
                MenuInfo main = new MenuInfo();
                info.FillTableWithModel<MenuInfo>(main);
                if (!string.IsNullOrEmpty(info.MenuInfoId))
                {
                    if (main.MenuInfoId == main.PMenuId)
                        throw new Exception("上级菜单不能为当前菜单！");
                    _sql.Update(main);
                    result = Constants.UpdateSuccessMssg;
                }
                else
                {
                    DataTable dt = _sql.Query("SELECT MenuName FROM MenuInfo WHERE IsDeleted = 0 AND MenuCode = @code", new Dictionary<string, object> { { "@code", info.MenuCode } });
                    if (dt != null && dt.Rows.Count > 0)
                        throw new Exception(string.Format("当前菜单【{0}:{1}】已存在！", info.MenuCode, Cast.ConToString(dt.Rows[0]["MenuName"])));
                    _sql.Create(main);
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
        /// 根据菜单Id获取菜单编辑信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MenuInfoEditModel GetMenuInfoById(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    throw new Exception("菜单Id不能为空！");
                }
                MenuInfoEditModel result = new MenuInfoEditModel();
                DataTable dt = _sql.Query(@"SELECT m.MenuName,
                                                   m.MenuCode,
                                                   m.MenuPath,
                                                   m.MenuSeq,
                                                   m.MenuInfoId,
                                                   m.PMenuId,
                                                   pm.MenuName AS PMenuIdName
                                            FROM   MenuInfo m
                                                   LEFT  JOIN MenuInfo pm
                                                           ON m.PMenuId = pm.MenuInfoId
                                            WHERE  m.MenuInfoId = @id 
                                            ", new Dictionary<string, object> { { "@id", id } });
                if (dt != null && dt.Rows.Count > 0)
                {
                    result = dt.ToModelList<MenuInfoEditModel>()[0];
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
        /// 根据菜单Id删除菜单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string DeleteMenuInfoById(string id)
        {
            try
            {
                _sql.OpenDb();
                _sql.BeginTrans();
                if (string.IsNullOrWhiteSpace(id))
                {
                    throw new Exception("菜单Id不能为空！");
                }
                _sql.Execute("DELETE FROM MenuInRole WHERE MenuInfoId = @id", new Dictionary<string, object> { { "@id", id } });
                _sql.Execute("DELETE FROM MenuInfo WHERE MenuInfoId = @id", new Dictionary<string, object> { { "@id", id } });
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
        /// 获取当前账号角色可访问的路由
        /// </summary>
        /// <returns></returns>
        public List<string> GetAccessMenuRoute()
        {
            try
            {
                List<string> result = new List<string>();
                DataTable dt = _sql.Query(@"SELECT DISTINCT mi.MenuPath
                                            FROM   UserInfo ui WITH(nolock)
                                                   INNER JOIN UserInRole uir WITH(nolock)
                                                           ON ui.UserInfoId = uir.UserInfoId
                                                   INNER JOIN MenuInRole mir WITH(nolock)
                                                           ON uir.RoleInfoId = mir.RoleInfoId
                                                   INNER JOIN MenuInfo mi WITH(nolock)
                                                           ON mir.MenuInfoId = mi.MenuInfoId
                                            WHERE  mi.IsDeleted = 0
                                                   AND ui.UserInfoId = @userId 
                                            ", new Dictionary<string, object> { { "@userId", _identity.UserId } });
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach(DataRow row in dt.Rows)
                    {
                        result.Add(Cast.ConToString(row["MenuPath"]));
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                throw ex;
            }
        }
    }
}

using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Blog.Core.Common;
using Blog.Core.Model;
using Blog.Core.Biz.Role;

namespace Blog.Core.Controllers
{
    /// <summary>
    /// 角色Controller
    /// </summary>
    [Route("api/roleinfo")]
    public class RoleInfoController : BaseController
    {
        /// <summary>
        /// 获取角色列表数据
        /// </summary>
        /// <param name="searchList"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        [HttpGet, Route("list")]
        public RoleInfoListData GetRoleInfoList(string searchList, int pageIndex, int pageSize, string orderBy)
        {
            return new RoleInfoCommand(UserIdentity).GetRoleInfoList(!string.IsNullOrEmpty(searchList) ? JsonConvert.DeserializeObject<List<SearchCondition>>(searchList) : null, pageIndex, pageSize, orderBy);
        }

        /// <summary>
        /// 根据角色Id获取角色编辑信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("edit")]
        public RoleInfoEditModel GetRoleInfoById(string id)
        {
            return new RoleInfoCommand(UserIdentity).GetRoleInfoById(id);
        }

        /// <summary>
        /// 创建或更新角色信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("save")]
        public string CreateOrUpdateRoleInfo(RoleInfoEditModel model)
        {
            return new RoleInfoCommand(UserIdentity).CreateOrUpdateRoleInfo(model);
        }

        /// <summary>
        /// 删除角色信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost, Route("delete")]
        public string DeleteRoleInfo(string[] ids)
        {
            return new RoleInfoCommand(UserIdentity).DeleteRoleInfo(ids);
        }

        /// <summary>
        /// 根据角色Id获取菜单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("menulist")]
        public string[] GetMenuInfoList(string id)
        {
            return new RoleInfoCommand(UserIdentity).GetMenuInfoList(id);
        }

        /// <summary>
        /// 创建角色与菜单对应关系信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost, Route("savemir")]
        public string CreateMIR(MIREditModel info)
        {
            return new RoleInfoCommand(UserIdentity).CreateMIR(info);
        }

        /// <summary>
        /// 上传角色信息
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("importrole")]
        public string UploadRolesInfo()
        {
            var dataTable = GetExcelDataTable();
            return new RoleInfoCommand(UserIdentity).UploadRole(dataTable);
        }

        /// <summary>
        /// 获取角色穿梭框数据
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("roledata")]
        public List<TransferModel> GetRoleTransferData()
        {
            return new RoleInfoCommand(UserIdentity).GetRoleTransferData();
        }
    }
}

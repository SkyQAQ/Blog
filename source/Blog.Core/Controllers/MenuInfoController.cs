using Microsoft.AspNetCore.Mvc;
using Blog.Core.Common;
using Blog.Core.Model;
using Blog.Core.Biz.Menu;
using System.Collections.Generic;

namespace Blog.Core.Controllers
{
    /// <summary>
    /// 菜单Controller
    /// </summary>
    [Route("api/menuinfo")]
    public class MenuInfoController : BaseController
    {
        /// <summary>
        /// 获取菜单树形结果数据
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("treedata")]
        public List<TreeNode> GetMenuTreeData()
        {
            return new MenuInfoCommand(UserIdentity).GetMenuTreeData();
        }

        /// <summary>
        /// 根据用户Id获取菜单树形结果数据
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("router")]
        public List<TreeNode> GetMenuTreeDataByUserId()
        {
            return new MenuInfoCommand(UserIdentity).GetMenuTreeDataByUserId();
        }

        /// <summary>
        /// 根据菜单Id获取菜单编辑信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("edit")]
        public MenuInfoEditModel GetMenuInfoById(string id)
        {
            return new MenuInfoCommand(UserIdentity).GetMenuInfoById(id);
        }

        /// <summary>
        /// 创建或更新菜单信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("save")]
        public string CreateOrUpdateMenuInfo(MenuInfoEditModel model)
        {
            return new MenuInfoCommand(UserIdentity).CreateOrUpdateMenuInfo(model);
        }

        /// <summary>
        /// 根据菜单Id删除菜单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("delete")]
        public string DeleteMenuInfoById(string id)
        {
            return new MenuInfoCommand(UserIdentity).DeleteMenuInfoById(id);
        }

        /// <summary>
        /// 获取当前账号角色可访问的路由
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("accessroute")]
        public List<string> GetAccessMenuRoute(string id)
        {
            return new MenuInfoCommand(UserIdentity).GetAccessMenuRoute();
        }
    }
}
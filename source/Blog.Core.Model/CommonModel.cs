using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Core.Model
{
    #region 返回结果
    /// <summary>
    /// 返回结果
    /// </summary>
    public class Result
    {
        /// <summary>
        /// 结果代码
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 结果信息
        /// </summary>
        public string mssg { get; set; }

        /// <summary>
        /// 结果数据
        /// </summary>
        public object data { get; set; }
    }
    #endregion

    #region LookUp
    /// <summary>
    /// LookUp
    /// </summary>
    public class LookUpModel
    {
        /// <summary>
        /// id
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// name
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
    #endregion

    #region 登录认证信息
    /// <summary>
    /// 登录认证信息
    /// </summary>
    public class LoginCredit
    {
        /// <summary>
        /// 验证类型
        /// </summary>
        public string grant_type { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string username { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }

        /// <summary>
        /// 验证码1
        /// </summary>
        public string verifycode1 { get; set; }

        /// <summary>
        /// 验证码2
        /// </summary>
        public string verifycode2 { get; set; }

        /// <summary>
        /// mac地址
        /// </summary>
        public string mac { get; set; }

        /// <summary>
        /// 刷新Token
        /// </summary>
        public string refresh_token { get; set; }
    }
    #endregion

    #region Token模型
    /// <summary>
    /// Token模型
    /// </summary>
    public class AuthToken
    {
        /// <summary>
        /// 请求token
        /// </summary>
        public string access_token { get; set; }

        /// <summary>
        /// 刷新token
        /// </summary>
        public string refresh_token { get; set; }

        /// <summary>
        /// token类型
        /// </summary>
        public string token_type { get; set; }

        /// <summary>
        /// 有效期
        /// </summary>
        public string expires_in { get; set; }
    }
    #endregion

    #region 用户身份模型
    /// <summary>
    /// 用户身份模型
    /// </summary>
    public class UserIdentity
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户账号
        /// </summary>
        public string UserAccount { get; set; }

        /// <summary>
        /// 用户角色
        /// </summary>
        public string[] UserRoles { get; set; }
    }
    #endregion

    #region 客户端信息模型
    /// <summary>
    /// 客户端信息模型
    /// </summary>
    public class ClientInfo
    {
        /// <summary>
        /// 客户端Id
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// 客户端主机名
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 客户端浏览器
        /// </summary>
        public string Browser { get; set; }

        /// <summary>
        /// 客户端浏览器版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 客户端操作系统
        /// </summary>
        public string PlatForm { get; set; }
    }
    #endregion

    #region 验证码
    /// <summary>
    /// 验证码
    /// </summary>
    public class VerifyCode
    {
        /// <summary>
        /// 验证码
        /// </summary>
        public string VerifyCodeString { get; set; }

        /// <summary>
        /// 验证码图片String
        /// </summary>
        public string VerifyCodeBaseString { get; set; }
    }
    #endregion

    #region 查询条件
    /// <summary>
    /// 查询条件
    /// </summary>
    public class SearchCondition
    {
        /// <summary>
        /// 查询类型  1-equal查询；2-like查询；3-coustmer自定义；4-range区间查询（用于时间查询）；
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 表名  例如role.RoleName = '基础角色'中的【role.】
        /// </summary>
        public string Table { get; set; }

        /// <summary>
        /// 查询字段  例如role.RoleName = '基础角色'中的【RoleName】
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 查询字段值  例如site.new_name = '基础角色'中的【基础角色】
        /// </summary>
        public string Value { get; set; }
    }
    #endregion

    #region Elment树形结构
    /// <summary>
    /// Elment树形结构
    /// </summary>
    public class TreeNode
    {
        /// <summary>
        /// 节点ID
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 父节点ID
        /// </summary>
        public string pid { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public string label { get; set; }

        /// <summary>
        /// 标签1
        /// </summary>
        public string label1 { get; set; }

        /// <summary>
        /// 标签2
        /// </summary>
        public string label2 { get; set; }

        /// <summary>
        /// 子节点
        /// </summary>
        public List<TreeNode> children { get; set; }
    }
    #endregion

    #region 穿梭框Model
    /// <summary>
    /// 穿梭框Model
    /// </summary>
    public class TransferModel
    {
        /// <summary>
        /// Key
        /// </summary>
        public string key { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public string label { get; set; }

        /// <summary>
        /// 搜索字段
        /// </summary>
        public string search { get; set; }
    }
    #endregion

    #region 下拉框Model
    /// <summary>
    /// 下拉框Model
    /// </summary>
    public class OptionModel
    {
        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public string Text { get; set; }
    }
    #endregion
}

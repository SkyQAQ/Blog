using System;
using System.Collections.Generic;

namespace Blog.Core.Model
{
    #region VedioInfo
    /// <summary>
    /// VedioInfo模型
    /// </summary>
    [Serializable]
    public class VedioInfo : BaseModel
    {
        /// <summary>
        /// 表名
        /// </summary>
        public static readonly string TableName = "VedioInfo";

        /// <summary>
        /// 构造函数
        /// </summary>
        public VedioInfo() : base(TableName) { }
        private Guid Id = Guid.Empty;

        /// <summary>
        /// 视频Id
        /// </summary>
        public Guid VedioInfoId
        {
            get { return Id; }
            set
            {
                Id = value;
                if (value != null)
                {
                    base.BaseId = value;
                }
            }
        }

        /// <summary>
        /// 视频描述
        /// </summary>
        public string Description { get; set; } 

        /// <summary>
        /// 视频源地址
        /// </summary>
        public string SourceUrl { get; set; } 

        /// <summary>
        /// 视频源类型
        /// </summary>
        public string SourceType { get; set; } 

        /// <summary>
        /// 视频封面
        /// </summary>
        public string Poster { get; set; } 

        /// <summary>
        /// 视频大小
        /// </summary>
        public string Size { get; set; } 

        /// <summary>
        /// 是否公开
        /// </summary>
        public int IsPublic { get; set; } = 0;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; } 

        /// <summary>
        /// 创建人账号
        /// </summary>
        public string CreatedBy { get; set; } 

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifiedOn { get; set; } 

        /// <summary>
        /// 更改人账号
        /// </summary>
        public string ModifiedBy { get; set; } 

        /// <summary>
        /// 是否删除：0-否；1-是；
        /// </summary>
        public int IsDeleted { get; set; } = 0;
    }
    /// <summary>
    /// VedioInfo数据模型
    /// </summary>
    [Serializable]
    public class VedioInfoListData
    {
        /// <summary>
        /// 总记录数
        /// </summary>
        public int RecordCount { get; set; }

        /// <summary>
        /// 数据列表
        /// </summary>
        public List<VedioInfoListModel> RecordList { get; set; }

        /// <summary>
        /// 导出数据
        /// </summary>
        public string ExportBuffString { get; set; } = string.Empty;
    }
    /// <summary>
    /// VedioInfo列表模型
    /// </summary>
    [Serializable]
    public partial class VedioInfoListModel
    {

        /// <summary>
        /// 视频Id
        /// </summary>
        public string VedioInfoId { get; set; }

        /// <summary>
        /// 视频描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 视频源地址
        /// </summary>
        public string SourceUrl { get; set; }

        /// <summary>
        /// 视频源类型
        /// </summary>
        public string SourceType { get; set; }

        /// <summary>
        /// 视频封面
        /// </summary>
        public string Poster { get; set; }

        /// <summary>
        /// 视频大小
        /// </summary>
        public string Size { get; set; }

        /// <summary>
        /// 是否公开
        /// </summary>
        public int IsPublic { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreatedOn { get; set; }

        /// <summary>
        /// 创建人账号
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public string ModifiedOn { get; set; }

        /// <summary>
        /// 更改人账号
        /// </summary>
        public string ModifiedBy { get; set; }
    }
    #endregion

    #region MenuInRole
    /// <summary>
    /// MenuInRole模型
    /// </summary>
    [Serializable]
    public class MenuInRole : BaseModel
    {
        /// <summary>
        /// 表名
        /// </summary>
        public static readonly string TableName = "MenuInRole";

        /// <summary>
        /// 构造函数
        /// </summary>
        public MenuInRole() : base(TableName) { }
        private Guid Id = Guid.Empty;

        /// <summary>
        /// 菜单角色关系Id
        /// </summary>
        public Guid MenuInRoleId
        {
            get { return Id; }
            set
            {
                Id = value;
                if (value != null)
                {
                    base.BaseId = value;
                }
            }
        }

        /// <summary>
        /// 菜单Id
        /// </summary>
        public Guid MenuInfoId { get; set; } = Guid.Empty;

        /// <summary>
        /// 菜单编码
        /// </summary>
        public string MenuCode { get; set; } = string.Empty;

        /// <summary>
        /// 角色Id
        /// </summary>
        public Guid RoleInfoId { get; set; } = Guid.Empty;

        /// <summary>
        /// 角色编码
        /// </summary>
        public string RoleCode { get; set; } = string.Empty;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; } 

        /// <summary>
        /// 创建人账号
        /// </summary>
        public string CreatedBy { get; set; } 

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifiedOn { get; set; } 

        /// <summary>
        /// 更改人账号
        /// </summary>
        public string ModifiedBy { get; set; } 

        /// <summary>
        /// 是否删除：0-否；1-是；
        /// </summary>
        public int IsDeleted { get; set; } = 0;
    }
    /// <summary>
    /// MenuInRole数据模型
    /// </summary>
    [Serializable]
    public class MenuInRoleListData
    {
        /// <summary>
        /// 总记录数
        /// </summary>
        public int RecordCount { get; set; }

        /// <summary>
        /// 数据列表
        /// </summary>
        public List<MenuInRoleListModel> RecordList { get; set; }

        /// <summary>
        /// 导出数据
        /// </summary>
        public string ExportBuffString { get; set; } = string.Empty;
    }
    /// <summary>
    /// MenuInRole列表模型
    /// </summary>
    [Serializable]
    public partial class MenuInRoleListModel
    {

        /// <summary>
        /// 菜单角色关系Id
        /// </summary>
        public string MenuInRoleId { get; set; }

        /// <summary>
        /// 菜单Id
        /// </summary>
        public string MenuInfoId { get; set; }

        /// <summary>
        /// 菜单编码
        /// </summary>
        public string MenuCode { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        public string RoleInfoId { get; set; }

        /// <summary>
        /// 角色编码
        /// </summary>
        public string RoleCode { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreatedOn { get; set; }

        /// <summary>
        /// 创建人账号
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public string ModifiedOn { get; set; }

        /// <summary>
        /// 更改人账号
        /// </summary>
        public string ModifiedBy { get; set; }
    }
    #endregion

    #region JobInfo
    /// <summary>
    /// JobInfo模型
    /// </summary>
    [Serializable]
    public class JobInfo : BaseModel
    {
        /// <summary>
        /// 表名
        /// </summary>
        public static readonly string TableName = "JobInfo";

        /// <summary>
        /// 构造函数
        /// </summary>
        public JobInfo() : base(TableName) { }
        private Guid Id = Guid.Empty;

        /// <summary>
        /// JobInfoId
        /// </summary>
        public Guid JobInfoId
        {
            get { return Id; }
            set
            {
                Id = value;
                if (value != null)
                {
                    base.BaseId = value;
                }
            }
        }

        /// <summary>
        /// 定时任务状态
        /// </summary>
        public string JobStatus { get; set; } 

        /// <summary>
        /// Job组
        /// </summary>
        public string JobGroup { get; set; } 

        /// <summary>
        /// Job名
        /// </summary>
        public string JobName { get; set; } 

        /// <summary>
        /// Job描述
        /// </summary>
        public string JobDesc { get; set; } = string.Empty;

        /// <summary>
        /// Job类
        /// </summary>
        public string JobClass { get; set; } 

        /// <summary>
        /// Cron表达式
        /// </summary>
        public string Cron { get; set; } 

        /// <summary>
        /// Cron描述
        /// </summary>
        public string CronDesc { get; set; } = string.Empty;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; } 

        /// <summary>
        /// 创建人账号
        /// </summary>
        public string CreatedBy { get; set; } 

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifiedOn { get; set; } 

        /// <summary>
        /// 更改人账号
        /// </summary>
        public string ModifiedBy { get; set; } 

        /// <summary>
        /// 是否删除：0-否；1-是；
        /// </summary>
        public int IsDeleted { get; set; } = 0;
    }
    /// <summary>
    /// JobInfo数据模型
    /// </summary>
    [Serializable]
    public class JobInfoListData
    {
        /// <summary>
        /// 总记录数
        /// </summary>
        public int RecordCount { get; set; }

        /// <summary>
        /// 数据列表
        /// </summary>
        public List<JobInfoListModel> RecordList { get; set; }

        /// <summary>
        /// 导出数据
        /// </summary>
        public string ExportBuffString { get; set; } = string.Empty;
    }
    /// <summary>
    /// JobInfo列表模型
    /// </summary>
    [Serializable]
    public partial class JobInfoListModel
    {

        /// <summary>
        /// JobInfoId
        /// </summary>
        public string JobInfoId { get; set; }

        /// <summary>
        /// 定时任务状态
        /// </summary>
        public string JobStatus { get; set; }

        /// <summary>
        /// Job组
        /// </summary>
        public string JobGroup { get; set; }

        /// <summary>
        /// Job名
        /// </summary>
        public string JobName { get; set; }

        /// <summary>
        /// Job描述
        /// </summary>
        public string JobDesc { get; set; }

        /// <summary>
        /// Job类
        /// </summary>
        public string JobClass { get; set; }

        /// <summary>
        /// Cron表达式
        /// </summary>
        public string Cron { get; set; }

        /// <summary>
        /// Cron描述
        /// </summary>
        public string CronDesc { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreatedOn { get; set; }

        /// <summary>
        /// 创建人账号
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public string ModifiedOn { get; set; }

        /// <summary>
        /// 更改人账号
        /// </summary>
        public string ModifiedBy { get; set; }
    }
    #endregion

    #region UserInfo
    /// <summary>
    /// UserInfo模型
    /// </summary>
    [Serializable]
    public class UserInfo : BaseModel
    {
        /// <summary>
        /// 表名
        /// </summary>
        public static readonly string TableName = "UserInfo";

        /// <summary>
        /// 构造函数
        /// </summary>
        public UserInfo() : base(TableName) { }
        private Guid Id = Guid.Empty;

        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserInfoId
        {
            get { return Id; }
            set
            {
                Id = value;
                if (value != null)
                {
                    base.BaseId = value;
                }
            }
        }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; } = string.Empty;

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; } 

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; } = string.Empty;

        /// <summary>
        /// 密码状态：0-正常；1-半年没修改密码；2-首次登陆；
        /// </summary>
        public int PwdState { get; set; } = 0;

        /// <summary>
        /// 密码修改时间
        /// </summary>
        public DateTime PwdModifiedOn { get; set; } 

        /// <summary>
        /// 登录失败时间
        /// </summary>
        public DateTime LoginFiledOn { get; set; } 

        /// <summary>
        /// 登录失败次数
        /// </summary>
        public int LoginFiledTimes { get; set; } = 0;

        /// <summary>
        /// 部门Id
        /// </summary>
        public Guid DepartmentId { get; set; } = Guid.Empty;

        /// <summary>
        /// 登录状态：0-未登录；1-已登录
        /// </summary>
        public int IsLogin { get; set; } = 0;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; } 

        /// <summary>
        /// 创建人账号
        /// </summary>
        public string CreatedBy { get; set; } 

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifiedOn { get; set; } 

        /// <summary>
        /// 更改人账号
        /// </summary>
        public string ModifiedBy { get; set; } 

        /// <summary>
        /// 是否删除：0-否；1-是；
        /// </summary>
        public int IsDeleted { get; set; } = 0;
    }
    /// <summary>
    /// UserInfo数据模型
    /// </summary>
    [Serializable]
    public class UserInfoListData
    {
        /// <summary>
        /// 总记录数
        /// </summary>
        public int RecordCount { get; set; }

        /// <summary>
        /// 数据列表
        /// </summary>
        public List<UserInfoListModel> RecordList { get; set; }

        /// <summary>
        /// 导出数据
        /// </summary>
        public string ExportBuffString { get; set; } = string.Empty;
    }
    /// <summary>
    /// UserInfo列表模型
    /// </summary>
    [Serializable]
    public partial class UserInfoListModel
    {

        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserInfoId { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 密码状态：0-正常；1-半年没修改密码；2-首次登陆；
        /// </summary>
        public int PwdState { get; set; }

        /// <summary>
        /// 密码修改时间
        /// </summary>
        public string PwdModifiedOn { get; set; }

        /// <summary>
        /// 登录失败时间
        /// </summary>
        public string LoginFiledOn { get; set; }

        /// <summary>
        /// 登录失败次数
        /// </summary>
        public int LoginFiledTimes { get; set; }

        /// <summary>
        /// 部门Id
        /// </summary>
        public string DepartmentId { get; set; }

        /// <summary>
        /// 登录状态：0-未登录；1-已登录
        /// </summary>
        public int IsLogin { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreatedOn { get; set; }

        /// <summary>
        /// 创建人账号
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public string ModifiedOn { get; set; }

        /// <summary>
        /// 更改人账号
        /// </summary>
        public string ModifiedBy { get; set; }
    }
    #endregion

    #region Attachment
    /// <summary>
    /// Attachment模型
    /// </summary>
    [Serializable]
    public class Attachment : BaseModel
    {
        /// <summary>
        /// 表名
        /// </summary>
        public static readonly string TableName = "Attachment";

        /// <summary>
        /// 构造函数
        /// </summary>
        public Attachment() : base(TableName) { }
        private Guid Id = Guid.Empty;

        /// <summary>
        /// 
        /// </summary>
        public Guid AttachmentId
        {
            get { return Id; }
            set
            {
                Id = value;
                if (value != null)
                {
                    base.BaseId = value;
                }
            }
        }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; } 

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; } 

        /// <summary>
        /// 文件大小
        /// </summary>
        public decimal FileSize { get; set; } 

        /// <summary>
        /// 文件类型
        /// </summary>
        public string MimeType { get; set; } 

        /// <summary>
        /// 模块类型
        /// </summary>
        public string ModuleType { get; set; } 

        /// <summary>
        /// 模块Id
        /// </summary>
        public string ModuleId { get; set; } 

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; } 

        /// <summary>
        /// 创建人账号
        /// </summary>
        public string CreatedBy { get; set; } 

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifiedOn { get; set; } 

        /// <summary>
        /// 更改人账号
        /// </summary>
        public string ModifiedBy { get; set; } 

        /// <summary>
        /// 是否删除：0-否；1-是；
        /// </summary>
        public int IsDeleted { get; set; } = 0;
    }
    /// <summary>
    /// Attachment数据模型
    /// </summary>
    [Serializable]
    public class AttachmentListData
    {
        /// <summary>
        /// 总记录数
        /// </summary>
        public int RecordCount { get; set; }

        /// <summary>
        /// 数据列表
        /// </summary>
        public List<AttachmentListModel> RecordList { get; set; }

        /// <summary>
        /// 导出数据
        /// </summary>
        public string ExportBuffString { get; set; } = string.Empty;
    }
    /// <summary>
    /// Attachment列表模型
    /// </summary>
    [Serializable]
    public partial class AttachmentListModel
    {

        /// <summary>
        /// 
        /// </summary>
        public string AttachmentId { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public decimal FileSize { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public string MimeType { get; set; }

        /// <summary>
        /// 模块类型
        /// </summary>
        public string ModuleType { get; set; }

        /// <summary>
        /// 模块Id
        /// </summary>
        public string ModuleId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreatedOn { get; set; }

        /// <summary>
        /// 创建人账号
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public string ModifiedOn { get; set; }

        /// <summary>
        /// 更改人账号
        /// </summary>
        public string ModifiedBy { get; set; }
    }
    #endregion

    #region RoleInfo
    /// <summary>
    /// RoleInfo模型
    /// </summary>
    [Serializable]
    public class RoleInfo : BaseModel
    {
        /// <summary>
        /// 表名
        /// </summary>
        public static readonly string TableName = "RoleInfo";

        /// <summary>
        /// 构造函数
        /// </summary>
        public RoleInfo() : base(TableName) { }
        private Guid Id = Guid.Empty;

        /// <summary>
        /// 角色Id
        /// </summary>
        public Guid RoleInfoId
        {
            get { return Id; }
            set
            {
                Id = value;
                if (value != null)
                {
                    base.BaseId = value;
                }
            }
        }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; } = string.Empty;

        /// <summary>
        /// 角色编码
        /// </summary>
        public string RoleCode { get; set; } = string.Empty;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; } 

        /// <summary>
        /// 创建人账号
        /// </summary>
        public string CreatedBy { get; set; } 

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifiedOn { get; set; } 

        /// <summary>
        /// 更改人账号
        /// </summary>
        public string ModifiedBy { get; set; } 

        /// <summary>
        /// 是否删除：0-否；1-是；
        /// </summary>
        public int IsDeleted { get; set; } = 0;
    }
    /// <summary>
    /// RoleInfo数据模型
    /// </summary>
    [Serializable]
    public class RoleInfoListData
    {
        /// <summary>
        /// 总记录数
        /// </summary>
        public int RecordCount { get; set; }

        /// <summary>
        /// 数据列表
        /// </summary>
        public List<RoleInfoListModel> RecordList { get; set; }

        /// <summary>
        /// 导出数据
        /// </summary>
        public string ExportBuffString { get; set; } = string.Empty;
    }
    /// <summary>
    /// RoleInfo列表模型
    /// </summary>
    [Serializable]
    public partial class RoleInfoListModel
    {

        /// <summary>
        /// 角色Id
        /// </summary>
        public string RoleInfoId { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 角色编码
        /// </summary>
        public string RoleCode { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreatedOn { get; set; }

        /// <summary>
        /// 创建人账号
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public string ModifiedOn { get; set; }

        /// <summary>
        /// 更改人账号
        /// </summary>
        public string ModifiedBy { get; set; }
    }
    #endregion

    #region UserInRole
    /// <summary>
    /// UserInRole模型
    /// </summary>
    [Serializable]
    public class UserInRole : BaseModel
    {
        /// <summary>
        /// 表名
        /// </summary>
        public static readonly string TableName = "UserInRole";

        /// <summary>
        /// 构造函数
        /// </summary>
        public UserInRole() : base(TableName) { }
        private Guid Id = Guid.Empty;

        /// <summary>
        /// 用户角色关系Id
        /// </summary>
        public Guid UserInRoleId
        {
            get { return Id; }
            set
            {
                Id = value;
                if (value != null)
                {
                    base.BaseId = value;
                }
            }
        }

        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserInfoId { get; set; } = Guid.Empty;

        /// <summary>
        /// 用户账号
        /// </summary>
        public string UserCode { get; set; } = string.Empty;

        /// <summary>
        /// 角色Id
        /// </summary>
        public Guid RoleInfoId { get; set; } = Guid.Empty;

        /// <summary>
        /// 角色编码
        /// </summary>
        public string RoleCode { get; set; } = string.Empty;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; } 

        /// <summary>
        /// 创建人账号
        /// </summary>
        public string CreatedBy { get; set; } 

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifiedOn { get; set; } 

        /// <summary>
        /// 更改人账号
        /// </summary>
        public string ModifiedBy { get; set; } 

        /// <summary>
        /// 是否删除：0-否；1-是；
        /// </summary>
        public int IsDeleted { get; set; } = 0;
    }
    /// <summary>
    /// UserInRole数据模型
    /// </summary>
    [Serializable]
    public class UserInRoleListData
    {
        /// <summary>
        /// 总记录数
        /// </summary>
        public int RecordCount { get; set; }

        /// <summary>
        /// 数据列表
        /// </summary>
        public List<UserInRoleListModel> RecordList { get; set; }

        /// <summary>
        /// 导出数据
        /// </summary>
        public string ExportBuffString { get; set; } = string.Empty;
    }
    /// <summary>
    /// UserInRole列表模型
    /// </summary>
    [Serializable]
    public partial class UserInRoleListModel
    {

        /// <summary>
        /// 用户角色关系Id
        /// </summary>
        public string UserInRoleId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserInfoId { get; set; }

        /// <summary>
        /// 用户账号
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        public string RoleInfoId { get; set; }

        /// <summary>
        /// 角色编码
        /// </summary>
        public string RoleCode { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreatedOn { get; set; }

        /// <summary>
        /// 创建人账号
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public string ModifiedOn { get; set; }

        /// <summary>
        /// 更改人账号
        /// </summary>
        public string ModifiedBy { get; set; }
    }
    #endregion

    #region DreamInfo
    /// <summary>
    /// DreamInfo模型
    /// </summary>
    [Serializable]
    public class DreamInfo : BaseModel
    {
        /// <summary>
        /// 表名
        /// </summary>
        public static readonly string TableName = "DreamInfo";

        /// <summary>
        /// 构造函数
        /// </summary>
        public DreamInfo() : base(TableName) { }
        private Guid Id = Guid.Empty;

        /// <summary>
        /// 彩票Id
        /// </summary>
        public Guid DreamInfoId
        {
            get { return Id; }
            set
            {
                Id = value;
                if (value != null)
                {
                    base.BaseId = value;
                }
            }
        }

        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserInfoId { get; set; } 

        /// <summary>
        /// 彩票号码
        /// </summary>
        public string DreamCode { get; set; } 

        /// <summary>
        /// 起始期数
        /// </summary>
        public int StartStage { get; set; } 

        /// <summary>
        /// 截止期数
        /// </summary>
        public int EndStage { get; set; } 

        /// <summary>
        /// 彩票类型
        /// </summary>
        public int Type { get; set; } 

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; } 

        /// <summary>
        /// 创建人账号
        /// </summary>
        public string CreatedBy { get; set; } 

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifiedOn { get; set; } 

        /// <summary>
        /// 更改人账号
        /// </summary>
        public string ModifiedBy { get; set; } 

        /// <summary>
        /// 是否删除：0-否；1-是；
        /// </summary>
        public int IsDeleted { get; set; } = 0;
    }
    /// <summary>
    /// DreamInfo数据模型
    /// </summary>
    [Serializable]
    public class DreamInfoListData
    {
        /// <summary>
        /// 总记录数
        /// </summary>
        public int RecordCount { get; set; }

        /// <summary>
        /// 数据列表
        /// </summary>
        public List<DreamInfoListModel> RecordList { get; set; }

        /// <summary>
        /// 导出数据
        /// </summary>
        public string ExportBuffString { get; set; } = string.Empty;
    }
    /// <summary>
    /// DreamInfo列表模型
    /// </summary>
    [Serializable]
    public partial class DreamInfoListModel
    {

        /// <summary>
        /// 彩票Id
        /// </summary>
        public string DreamInfoId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserInfoId { get; set; }

        /// <summary>
        /// 彩票号码
        /// </summary>
        public string DreamCode { get; set; }

        /// <summary>
        /// 起始期数
        /// </summary>
        public int StartStage { get; set; }

        /// <summary>
        /// 截止期数
        /// </summary>
        public int EndStage { get; set; }

        /// <summary>
        /// 彩票类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreatedOn { get; set; }

        /// <summary>
        /// 创建人账号
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public string ModifiedOn { get; set; }

        /// <summary>
        /// 更改人账号
        /// </summary>
        public string ModifiedBy { get; set; }
    }
    #endregion

    #region MenuInfo
    /// <summary>
    /// MenuInfo模型
    /// </summary>
    [Serializable]
    public class MenuInfo : BaseModel
    {
        /// <summary>
        /// 表名
        /// </summary>
        public static readonly string TableName = "MenuInfo";

        /// <summary>
        /// 构造函数
        /// </summary>
        public MenuInfo() : base(TableName) { }
        private Guid Id = Guid.Empty;

        /// <summary>
        /// 菜单Id
        /// </summary>
        public Guid MenuInfoId
        {
            get { return Id; }
            set
            {
                Id = value;
                if (value != null)
                {
                    base.BaseId = value;
                }
            }
        }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName { get; set; } = string.Empty;

        /// <summary>
        /// 菜单编码
        /// </summary>
        public string MenuCode { get; set; } = string.Empty;

        /// <summary>
        /// 菜单路径
        /// </summary>
        public string MenuPath { get; set; } = string.Empty;

        /// <summary>
        /// 菜单排序
        /// </summary>
        public int MenuSeq { get; set; } = 0;

        /// <summary>
        /// 上级菜单Id
        /// </summary>
        public Guid PMenuId { get; set; } = Guid.Empty;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; } 

        /// <summary>
        /// 创建人账号
        /// </summary>
        public string CreatedBy { get; set; } 

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifiedOn { get; set; } 

        /// <summary>
        /// 更改人账号
        /// </summary>
        public string ModifiedBy { get; set; } 

        /// <summary>
        /// 是否删除：0-否；1-是；
        /// </summary>
        public int IsDeleted { get; set; } = 0;
    }
    /// <summary>
    /// MenuInfo数据模型
    /// </summary>
    [Serializable]
    public class MenuInfoListData
    {
        /// <summary>
        /// 总记录数
        /// </summary>
        public int RecordCount { get; set; }

        /// <summary>
        /// 数据列表
        /// </summary>
        public List<MenuInfoListModel> RecordList { get; set; }

        /// <summary>
        /// 导出数据
        /// </summary>
        public string ExportBuffString { get; set; } = string.Empty;
    }
    /// <summary>
    /// MenuInfo列表模型
    /// </summary>
    [Serializable]
    public partial class MenuInfoListModel
    {

        /// <summary>
        /// 菜单Id
        /// </summary>
        public string MenuInfoId { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName { get; set; }

        /// <summary>
        /// 菜单编码
        /// </summary>
        public string MenuCode { get; set; }

        /// <summary>
        /// 菜单路径
        /// </summary>
        public string MenuPath { get; set; }

        /// <summary>
        /// 菜单排序
        /// </summary>
        public int MenuSeq { get; set; }

        /// <summary>
        /// 上级菜单Id
        /// </summary>
        public string PMenuId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreatedOn { get; set; }

        /// <summary>
        /// 创建人账号
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public string ModifiedOn { get; set; }

        /// <summary>
        /// 更改人账号
        /// </summary>
        public string ModifiedBy { get; set; }
    }
    #endregion
}
using System;
using System.Reflection;

namespace Blog.Core.Common
{
    /// <summary>
    /// Constants
    /// </summary>
    public static class Constants
    {
        #region 公共模块
        /// <summary>
        /// 服务根目录
        /// </summary>
        public static string ServerMapPath()
        {
            return Environment.CurrentDirectory + "\\";
        }

        /// <summary>
        /// 上传附件路径
        /// </summary>
        public const string AttachmentPath = "app_data\\files\\attachment\\";

        /// <summary>
        /// 附件压缩路径
        /// </summary>
        public const string ZipPath = "app_data\\files\\zip\\";

        /// <summary>
        /// 上传头像路径
        /// </summary>
        public const string AvatarPath = "app_data\\files\\avatar\\";

        /// <summary>
        /// 上传视频路径
        /// </summary>
        public const string VedioPath = "wwwroot\\vedio\\";

        /// <summary>
        /// 日志记录Repository名称
        /// </summary>
        public const string LogRepositoryName = "NETCoreRepository";

        /// <summary>
        /// 日志记录配置文件名称
        /// </summary>
        public const string LogConfigFileName = "log4net.config";

        /// <summary>
        /// 表示该列需要加密
        /// </summary>
        public const string EncryptColoumn = "aesencrypt";

        /// <summary>
        /// 表示该列需要解密
        /// </summary>
        public const string DecryptColoumn = "aesdecrypt";

        /// <summary>
        /// Session-登录信息
        /// </summary>
        public const string Session_Login_Info = "login_info";

        /// <summary>
        /// 密码盐
        /// </summary>
        public const string PasswordSalt = "jl*i12!2w4c1k@dj";

        /// <summary>
        /// 阿里支付配置文件名
        /// </summary>
        public const string AlipayCfgPath = "Alipay.xml";

        /// <summary>
        /// Security配置文件名
        /// </summary>
        public const string SecurityCfgPath = "Security.xml";

        /// <summary>
        /// 是否为是
        /// </summary>
        public const int Boolean_Yes = 1;

        /// <summary>
        /// 是否为否
        /// </summary>
        public const int Boolean_No = 0;

        /// <summary>
        /// 返回结果成功
        /// </summary>
        public const int Result_Success = 1;

        /// <summary>
        /// 返回结果失败
        /// </summary>
        public const int Result_Failure = 0;

        /// <summary>
        /// 查询条件类型-等于
        /// </summary>
        public const int COMMON_SEARCHCONDITIONTYPE_EQUAL = 1;

        /// <summary>
        /// 查询条件类型-LIKE
        /// </summary>
        public const int COMMON_SEARCHCONDITIONTYPE_LIKE = 2;

        /// <summary>
        /// 查询条件类型-IN
        /// </summary>
        public const int COMMON_SEARCHCONDITIONTYPE_IN = 3;

        /// <summary>
        /// 查询条件类型-范围
        /// </summary>
        public const int COMMON_SEARCHCONDITIONTYPE_RANGE = 4;

        /// <summary>
        /// 查询条件类型-自定义
        /// </summary>
        public const int COMMON_SEARCHCONDITIONTYPE_CUSTOMER = 5;
        #endregion

        #region HTTP Contenttype
        /// <summary>
        /// application/x-www-form-urlencoded：数据被编码为名称/值对
        /// </summary>
        public const string ContentType1 = "application/x-www-form-urlencoded";

        /// <summary>
        /// application/json：用来告诉服务端消息主体是序列化后的 JSON 字符串
        /// </summary>
        public const string ContentType2 = "application/json";

        /// <summary>
        /// multipart/form-data：数据被编码为一条消息，页上的每个控件对应消息中的一个部分
        /// </summary>
        public const string ContentType3 = "multipart/form-data";

        /// <summary>
        /// text/xml：它是一种使用 HTTP 作为传输协议，XML 作为编码方式的远程调用规范
        /// </summary>
        public const string ContentType4 = "text/xml";
        #endregion

        #region 用户信息
        public const string SystemAccount = "System";

        /// <summary>
        /// 换成中Chat Id前缀
        /// </summary>
        public const string Redis_Chat_Prefix = "CHAT_";
        #endregion

        #region 返回结果
        /// <summary>
        /// 创建成功
        /// </summary>
        public const string CreateSuccessMssg = "创建成功";

        /// <summary>
        /// 删除成功
        /// </summary>
        public const string DeleteSuccessMssg = "删除成功";

        /// <summary>
        /// 更新成功
        /// </summary>
        public const string UpdateSuccessMssg = "更新成功";

        /// <summary>
        /// 操作成功
        /// </summary>
        public const string OperateSuccessMssg = "操作成功";

        /// <summary>
        /// 保存成功
        /// </summary>
        public const string SaveSuccessMssg = "保存成功";

        /// <summary>
        /// 权限不足
        /// </summary>
        public const string GrantFiledMssg = "权限不足";

        /// <summary>
        /// 上传成功
        /// </summary>
        public const string UploadSuccessMssg = "上传成功";
        #endregion

        #region 定时任务
        /// <summary>
        /// 暂停Job
        /// </summary>
        public const string JobPause = "pause";

        /// <summary>
        /// 恢复Job
        /// </summary>
        public const string JobResume = "resume";

        /// <summary>
        /// 停止Job
        /// </summary>
        public const string JobStop = "stop";

        /// <summary>
        /// 启动Job
        /// </summary>
        public const string JobStart = "start";

        /// <summary>
        /// 执行Job
        /// </summary>
        public const string JobExcute = "excute";

        /// <summary>
        /// 正常运行Trigger状态
        /// </summary>
        public const string TriggerStateWaitting = "WAITING";

        /// <summary>
        /// 暂停运行Trigger状态
        /// </summary>
        public const string TriggerStatePaused = "PAUSED";

        /// <summary>
        /// Job状态-运行
        /// </summary>
        public const string JobStatusRunning = "运行";

        /// <summary>
        /// Job状态-暂停
        /// </summary>
        public const string JobStatusPause = "暂停";

        /// <summary>
        /// Job状态-停止
        /// </summary>
        public const string JobStatusStop = "停止";
        #endregion

        #region 验证码
        /// <summary>
        /// 接收人类型-Email
        /// </summary>
        public const string ReceiveTypeEmail = "Email";

        /// <summary>
        /// 接收人类型-Phone
        /// </summary>
        public const string ReceiveTypePhone = "Phone";

        /// <summary>
        /// 验证码类型-Register
        /// </summary>
        public const string CodeTypeRegister = "Register";

        /// <summary>
        /// 验证码类型-ForgetPwd
        /// </summary>
        public const string CodeTypeForgetPwd = "ForgetPwd";

        /// <summary>
        /// 验证码类型-ChangeEmail
        /// </summary>
        public const string CodeTypeChangeEmail = "ChangeEmail";
        #endregion
    }
}

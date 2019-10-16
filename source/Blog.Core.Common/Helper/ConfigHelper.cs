using System.Xml;

namespace Blog.Core.Common
{
    /// <summary>
    /// 配置文件读取Helper
    /// </summary>
    public class ConfigHelper
    {
        #region 构造函数
        /// <summary>
        /// 配置文件名
        /// </summary>
        private string configfile = Constants.ServerMapPath() + "\\config\\Common.xml";
        /// <summary>
        /// 默认读取Common配置文件
        /// </summary>
        public ConfigHelper()
        {
            LoadXml();
        }
        /// <summary>
        /// 读取其他配置文件
        /// </summary>
        /// <param name="configfile"></param>
        public ConfigHelper(string configfile)
        {
            this.configfile = Constants.ServerMapPath() + "\\config\\" + configfile;
            LoadXml();
        }
        #endregion

        #region Common配置项名称
        /// <summary>
        /// Redis服务器连接字符串
        /// </summary>
        public string RS_ConnectString { get; set; }

        /// <summary>
        /// AES Key
        /// </summary>
        public string AES_Key { get; set; }
        /// <summary>
        /// AES Iv
        /// </summary>
        public string AES_Iv { get; set; }

        /// <summary>
        /// MicroSoft数据库连接字符串
        /// </summary>
        public string MSSQL { get; set; }
        /// <summary>
        /// MicroSoft只读数据库连接字符串
        /// </summary>
        public string MSSQL_Readonly { get; set; }
        /// <summary>
        /// MySql数据库连接字符串
        /// </summary>
        public string MYSQL { get; set; }

        /// <summary>
        /// 系统账号
        /// </summary>
        public string SystemAccount { get; set; }
        /// <summary>
        /// 跨域代理
        /// </summary>
        public string OriginServer { get; set; }

        /// <summary>
        /// 密码加密公钥
        /// </summary>
        public string Public_Key { get; set; }
        /// <summary>
        /// 密码加密密钥
        /// </summary>
        public string Private_Key { get; set; }

        /// <summary>
        /// QQ邮箱Host
        /// </summary>
        public string Email_QQ_Host { get; set; }
        /// <summary>
        /// QQ邮箱账号
        /// </summary>
        public string Email_QQ_Account { get; set; }
        /// <summary>
        /// QQ邮箱密码
        /// </summary>
        public string Email_QQ_Password { get; set; }

        /// <summary>
        /// 定时任务命名空间
        /// </summary>
        public string Job_Assembly { get; set; }

        /// <summary>
        /// 附件上传 请求地址
        /// </summary>
        public string Upload_Url { get; set; } = "";
        /// <summary>
        /// 附件上传 类型
        /// </summary>
        public string Upload_Type { get; set; } = "*";
        /// <summary>
        /// 附件上传 每次最多上传数量
        /// </summary>
        public int Upload_MaxCount { get; set; } = 5;
        /// <summary>
        /// 附件上传 每个文件最大（M）
        /// </summary>
        public int Upload_MaxLength { get; set; } = 50;
        #endregion

        #region ALIPAY配置项名称
        /// <summary>
        /// 支付宝网关（固定）
        /// </summary>
        public string ALIPAY_URL { get; set; }
        /// <summary>
        /// APPID 创建应用后生成
        /// </summary>
        public string ALIPAY_APPID { get; set; }
        /// <summary>
        /// Rsa256私钥
        /// </summary>
        public string ALIPAY_APP_PRIVATE_KEY { get; set; }
        /// <summary>
        /// Rsa256公钥
        /// </summary>
        public string ALIPAY_APP_PUBLIC_KEY { get; set; }
        /// <summary>
        /// 商户生成签名字符串所使用的签名算法类型
        /// </summary>
        public string ALIPAY_SIGN_TYPE { get; set; }
        /// <summary>
        /// 参数返回格式，只支持json
        /// </summary>
        public string ALIPAY_FORMAT { get; set; }
        /// <summary>
        /// 编码集，支持GBK/UTF-8
        /// </summary>
        public string ALIPAY_CHARSET { get; set; }
        #endregion

        #region Security配置项名称
        /// <summary>
        /// 生成Token的Key
        /// </summary>
        public string Token_Key { get; set; }
        /// <summary>
        /// 密码规则：是否开启验证
        /// </summary>
        public bool Password_Rule_IsOn { get; set; }
        /// <summary>
        /// 密码规则：最小长度
        /// </summary>
        public int Password_Rule_MinLength { get; set; }
        /// <summary>
        /// 密码规则：最大长度
        /// </summary>
        public int Password_Rule_MaxLength { get; set; }
        /// <summary>
        /// 密码规则：是否需要大小写
        /// </summary>
        public bool Password_Rule_UpperAndLower { get; set; }
        #endregion

        #region 读取配置文件
        private void LoadXml()
        {
            XmlDocument document = new XmlDocument();
            document.Load(configfile);
            XmlNodeList list = document.SelectNodes("//set");
            if (list == null || list.Count <= 0)
                return;
            foreach (XmlNode node in list)
            {
                if (node.Attributes == null)
                    continue;
                XmlAttribute nameAttribute = node.Attributes["name"];
                XmlAttribute valueAttribute = node.Attributes["value"];
                if (nameAttribute != null)
                {
                    switch (nameAttribute.Value)
                    {
                        #region Common
                        case "RS_ConnectString":
                            RS_ConnectString = valueAttribute.InnerText;
                            break;
                        case "AES_Key":
                            AES_Key = valueAttribute.Value;
                            break;
                        case "AES_Iv":
                            AES_Iv = valueAttribute.Value;
                            break;
                        case "MSSQL":
                            MSSQL = valueAttribute.Value;
                            break;
                        case "MSSQL_Readonly":
                            MSSQL_Readonly = valueAttribute.Value;
                            break;
                        case "MYSQL":
                            MYSQL = valueAttribute.Value;
                            break;
                        case "SystemAccount":
                            SystemAccount = valueAttribute.Value;
                            break;
                        case "OriginServer":
                            OriginServer = valueAttribute.Value;
                            break;
                        case "Public_Key":
                            Public_Key = valueAttribute.Value;
                            break;
                        case "Private_Key":
                            Private_Key = valueAttribute.Value;
                            break;
                        case "Email_QQ_Host":
                            Email_QQ_Host = valueAttribute.Value;
                            break;
                        case "Email_QQ_Account":
                            Email_QQ_Account = valueAttribute.Value;
                            break;
                        case "Email_QQ_Password":
                            Email_QQ_Password = valueAttribute.Value;
                            break;
                        case "Job_Assembly":
                            Job_Assembly = valueAttribute.Value; 
                            break;
                        case "Upload_Type":
                            Upload_Type = valueAttribute.Value;
                            break;
                        case "Upload_Url":
                            Upload_Url = valueAttribute.Value;
                            break;
                        case "Upload_MaxCount":
                            Upload_MaxCount = Cast.ConToInt(valueAttribute.Value);
                            break;
                        case "Upload_MaxLength":
                            Upload_MaxLength = Cast.ConToInt(valueAttribute.Value);
                            break;
                        #endregion

                        #region ALIPAY
                        case "ALIPAY_URL":
                            ALIPAY_URL = node.InnerText;
                            break;
                        case "ALIPAY_APPID":
                            ALIPAY_APPID = valueAttribute.Value;
                            break;
                        case "ALIPAY_APP_PRIVATE_KEY":
                            ALIPAY_APP_PRIVATE_KEY = valueAttribute.Value;
                            break;
                        case "ALIPAY_APP_PUBLIC_KEY":
                            ALIPAY_APP_PUBLIC_KEY = valueAttribute.Value;
                            break;
                        case "ALIPAY_SIGN_TYPE":
                            ALIPAY_SIGN_TYPE = valueAttribute.Value;
                            break;
                        case "ALIPAY_FORMAT":
                            ALIPAY_FORMAT = valueAttribute.Value;
                            break;
                        case "ALIPAY_CHARSET":
                            ALIPAY_CHARSET = valueAttribute.Value;
                            break;
                        #endregion

                        #region Security
                        case "Token_Key":
                            Token_Key = valueAttribute.Value;
                            break;
                        case "Password_Rule_IsOn":
                            Password_Rule_IsOn = Cast.ConToBoolean(valueAttribute.Value);
                            break;
                        case "Password_Rule_MinLength":
                            Password_Rule_MinLength = Cast.ConToInt(valueAttribute.Value);
                            break;
                        case "Password_Rule_MaxLength":
                            Password_Rule_MaxLength = Cast.ConToInt(valueAttribute.Value);
                            break;
                        case "Password_Rule_UpperAndLower":
                            Password_Rule_UpperAndLower = Cast.ConToBoolean(valueAttribute.Value);
                            break;
                            #endregion
                    }
                }
            }
        }
        #endregion
    }
}

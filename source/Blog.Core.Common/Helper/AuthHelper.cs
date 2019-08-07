using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Blog.Core.Model;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Blog.Core.Common
{
    /// <summary>
    /// 身份授权、认证
    /// </summary>
    public class AuthHelper
    {
        /// <summary>
        /// 客户端Ip
        /// </summary>
        private string _clientIp = "";
        /// <summary>
        /// Token过期时间（小时）
        /// </summary>
        private const int _token_expire_in = 2;
        /// <summary>
        /// 日志
        /// </summary>
        private LogHelper _log;
        /// <summary>
        /// 数据库
        /// </summary>
        private SqlHelper _sql;

        /// <summary>
        /// 无参构造函数
        /// </summary>
        public AuthHelper()
        {
            _log = new LogHelper("AuthHelper");
            _sql = new SqlHelper(_log);
        }

        /// <summary>
        /// 有参构造函数
        /// </summary>
        /// <param name="clientIp"></param>
        public AuthHelper(string clientIp)
        {
            _clientIp = clientIp;
            _log = new LogHelper("AuthHelper");
            _sql = new SqlHelper(_log);
        }

        /// <summary>
        /// 获取密码加密公钥
        /// </summary>
        /// <returns></returns>
        public string GetPublicKey()
        {
            ConfigHelper config = new ConfigHelper();
            return config.Public_Key;
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns></returns>
        public VerifyCode GetVerifyCode()
        {
            VerifyCode result = new VerifyCode();
            var vcHelper = new VerifyCodeHelper();
            vcHelper.CreateVerifyCode();
            result.VerifyCodeString = vcHelper.Text;
            result.VerifyCodeBaseString = vcHelper.ImageString;
            return result;
        }

        /// <summary>
        /// 获取授权Token
        /// </summary>
        /// <param name="credit">登录信息</param>
        /// <param name="clientIp">客户端Ip</param>
        /// <returns></returns>
        public AuthToken GetAuthToken(LoginCredit credit)
        {
            try
            {
                if (credit.grant_type == "password")
                {
                    string clientId = ValidVerifyCode(credit.verifycode1, credit.verifycode2);
                    Login(clientId, credit.username, credit.password);
                    string userId = GetUserIdByAccount(credit.username);
                    AuthToken token = new AuthToken();
                    token.expires_in = 60 * 60 * 1;
                    token.access_token = WuYao.RsaEncrypt("grant_type=password" + credit.verifycode1 + "$" + userId + "$" + DateTime.UtcNow.AddSeconds(token.expires_in).Ticks + "$" + _clientIp);
                    token.refresh_token = WuYao.RsaEncrypt("grant_type=refresh$" + userId + "$" + DateTime.UtcNow.AddDays(1).Ticks + "$" + DateTime.UtcNow.Ticks);
                    token.token_type = "Bearer";
                    return token;
                }
                else if (credit.grant_type == "refresh")
                {
                    string[] rtoken = WuYao.RsaDecrypt(credit.refresh_token.Replace(' ', '+')).Split('$');
                    if (long.Parse(rtoken[2]) < DateTime.UtcNow.Ticks)
                    {
                        if (CacheHelper.Exists(rtoken[1].ToUpper()))
                        {
                            CacheHelper.Remove(rtoken[1].ToUpper());
                        }
                        throw new UnauthorizedAccessException("当前账号已过期，请重新登录！");
                    }
                    UserIdentity identity = GetUserIdentity(rtoken[1]);
                    AuthToken token = new AuthToken();
                    token.expires_in = 60 * 60 * 1;
                    token.access_token = WuYao.RsaEncrypt("grant_type=password$" + identity.UserId + "$" + DateTime.UtcNow.AddSeconds(token.expires_in).Ticks + "$" + identity.IpAddress);
                    token.refresh_token = WuYao.RsaEncrypt("grant_type=refresh$" + identity.UserId + "$" + rtoken[2] + "$" + DateTime.UtcNow.Ticks);
                    token.token_type = "Bearer";
                    return token;
                }
                else
                {
                    throw new Exception("Invalid grant_type !");
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                throw new Exception(ex.Message);
            }
        }

        public AuthToken GetAuthTokenJWT(LoginCredit credit)
        {
            try
            {
                AuthToken result = new AuthToken();
                if (credit.grant_type == "password")
                {
                    ConfigHelper _s_config = new ConfigHelper(Constants.SecurityCfgPath);
                    string clientId = ValidVerifyCode(credit.verifycode1, credit.verifycode2);
                    Login(clientId, credit.username, credit.password);
                    string userId = GetUserIdByAccount(credit.username);
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Issuer = "Blog",
                        Audience = "API",
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.NameIdentifier, userId),
                            new Claim(ClaimTypes.Name, credit.username),
                            new Claim(ClaimTypes.Role, GetUserRoles(userId)),
                        }),
                        Expires = DateTime.UtcNow.AddDays(1),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_s_config.Token_Key)), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    result.access_token = tokenHandler.WriteToken(token);
                    result.expires_in = 60 * 60 * 24;
                    result.token_type = "Bearer";
                }
                //else if (credit.grant_type == "refresh")
                //{
                //    string[] rtoken = WuYao.RsaDecrypt(credit.refresh_token.Replace(' ', '+')).Split('$');
                //    if (long.Parse(rtoken[2]) < DateTime.UtcNow.Ticks)
                //    {
                //        if (CacheHelper.Exists(rtoken[1].ToUpper()))
                //        {
                //            CacheHelper.Remove(rtoken[1].ToUpper());
                //        }
                //        throw new UnauthorizedAccessException("当前账号已过期，请重新登录！");
                //    }
                //    UserIdentity identity = GetUserIdentity(rtoken[1]);
                //    AuthToken token = new AuthToken();
                //    token.expires_in = 60 * 60 * 1;
                //    token.access_token = WuYao.RsaEncrypt("grant_type=password$" + identity.UserId + "$" + DateTime.UtcNow.AddSeconds(token.expires_in).Ticks + "$" + identity.IpAddress);
                //    token.refresh_token = WuYao.RsaEncrypt("grant_type=refresh$" + identity.UserId + "$" + rtoken[2] + "$" + DateTime.UtcNow.Ticks);
                //    token.token_type = "Bearer";
                //    return token;
                //}
                else
                {
                    throw new Exception("Invalid grant_type !");
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string Logout(string userId)
        {
            try
            {
                _sql.OpenDb();
                _sql.Execute("UPDATE UserInfo SET IsLogin = 0 WHERE UserInfoId = @Id", new Dictionary<string, object> { { "@Id", userId } });
                CacheHelper.Remove(userId.ToUpper());
                CacheHelper.Remove("CHAT_" + userId.ToUpper());
                return "登出成功";
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
        /// 验证验证码，返回ClientId
        /// </summary>
        /// <param name="verifycode1"></param>
        /// <param name="verifycode2"></param>
        /// <returns></returns>
        private string ValidVerifyCode(string verifycode1, string verifycode2)
        {
            var vcHelper = new VerifyCodeHelper();
            return vcHelper.CheckVerifyCode(verifycode1, verifycode2);
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="account"></param>
        /// <param name="password"></param>
        private void Login(string clientId, string account, string password)
        {
            try
            {
                var instance = WuYao.GetSubClass(typeof(IAuthHelper));
                if (instance != null)
                {
                    var authInstance = instance as IAuthHelper;
                    authInstance.ValidUser(account, WuYao.RsaDecrypt(password));
                }
                else
                {
                    ValidUser(account, WuYao.GetPasswordCipher(password));
                }
            }
            catch (Exception ex)
            {
                _sql.OpenDb();
                //删除登陆验证码
                _sql.Execute("DELETE FROM tbl_loginverifycode WHERE ClientId = @id", new Dictionary<string, object> { { "@id", clientId } });
                _sql.CloseDb();
                throw ex;
            }
            
        }

        /// <summary>
        /// 验证用户
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="password">密码（与数据库中一致）</param>
        /// <returns></returns>
        private void ValidUser(string account, string password)
        {
            try
            {
                string sql = account.Contains("@") ? "SELECT * FROM UserInfo WHERE Email = @account" : "SELECT * FROM UserInfo WHERE Account = @account";
                List<UserInfo> userList = _sql.Search<UserInfo>(sql, new Dictionary<string, object> { { "@account", account } });
                if (userList == null || userList.Count == 0)
                {
                    throw new Exception("账号或密码错误！");
                }
                var user = userList[0];
                if (user.LoginFiledTimes >= 5)
                {
                    throw new Exception("账号密码输入错误次数:5,请修改密码后重新登录！");
                }
                if (user.IsDeleted == Constants.Boolean_Yes)
                {
                    throw new Exception("当前账号已禁用，请联系系统管理员！");
                }
                _sql.OpenDb();
                if (user.Password != password)
                {
                    user.LoginFiledOn = DateTimeUtils.NowBeijing();
                    user.LoginFiledTimes += 1;
                    if (user.LoginFiledTimes >= 5)
                    {
                        user.IsDeleted = Constants.Boolean_Yes;
                    }
                    _sql.Update(user);
                    throw new Exception("账号或密码错误！");
                }
                else
                {
                    user.LoginFiledTimes = 0;
                    user.IsLogin = 1;
                    _sql.Update(user);
                }
                _sql.Execute(string.Format("INSERT INTO tbl_loginlog (LoginLogId, UserInfoId, UserAccount, LoginOn, ClientIp) Values('{0}', '{1}', '{2}', @loginon, '{3}')",
                    Guid.NewGuid(), user.UserInfoId, user.Account, _clientIp),new Dictionary<string, object> { { "@loginon", DateTimeUtils.NowBeijing() } });
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _sql.CloseDb();
            }
        }

        /// <summary>
        /// 获取当前用户身份信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserIdentity GetUserIdentity(string userId)
        {
            UserIdentity identity = null;
            try
            {
                if (CacheHelper.Exists(userId.ToUpper()))
                {
                    identity = CacheHelper.Get<UserIdentity>(userId.ToUpper());
                }
                else
                {
                    DataTable dtUser = _sql.Query(@"SELECT UserInfoId AS UserId,
                                                           Account    AS UserAccount,
                                                           Name       AS UserName,
                                                           Email      AS UserEmail,
                                                           Avatar     AS UserAvatar
                                                    FROM   UserInfo WITH(nolock)
                                                    WHERE  IsDeleted = 0
                                                           AND UserInfoId = @userId 
                                                    ", new Dictionary<string, object> { { "@userId", userId } });
                    if (dtUser != null && dtUser.Rows.Count > 0)
                    {
                        identity = dtUser.ToModelList<UserIdentity>()[0];
                    }
                    else
                    {
                        throw new UnauthorizedAccessException("Disabled Account !");
                    }
                    identity.UserRoles = GetUserRoles(userId).Split(',');
                    identity.IpAddress = _clientIp;
                    CacheHelper.Insert(identity.UserId.ToUpper(), identity, 3660);
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                throw ex;
            }
            return identity;
        }

        /// <summary>
        /// 获取用户角色
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <returns></returns>
        private string GetUserRoles(string id)
        {
            string result = string.Empty;
            DataTable dt = _sql.Query(@"SELECT RoleCode FROM UserInRole WHERE UserInfoId = @Id", new Dictionary<string, object> { { "@Id", id } });
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    result += Cast.ConToString(row["RoleCode"]) + ",";
                }
                result = result.Remove(result.Length - 1);
            }
            return result;
        }

        /// <summary>
        /// 根据用户账号获取用户Id
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        private string GetUserIdByAccount(string account)
        {
            try
            {
                string sql = account.Contains("@") ? "SELECT UserInfoId FROM UserInfo WHERE Email = @account" : "SELECT UserInfoId FROM UserInfo WHERE Account = @account";
                DataTable dt = _sql.Query(sql, new Dictionary<string, object> { { "@account", account } });
                if (dt == null || dt.Rows.Count == 0)
                    throw new Exception("当前登录账号在系统不存在");
                return Cast.ConToString(dt.Rows[0]["UserInfoId"]);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex);
                throw ex;
            }
        }

        /// <summary>
        /// 获取用户验证码
        /// </summary>
        /// <param name="email"></param>
        public string GetReceiveVerifyCode(string receive, string type)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(receive))
                {
                    throw new Exception("请输入邮箱账号！");
                }
                string code = Rand.Number(6);
                if (receive.Contains("@"))
                {// 邮箱接收
                    if (type == Constants.CodeTypeRegister)
                    {// 注册账号
                        DataTable dt = _sql.Query("SELECT Account FROM UserInfo WHERE Email = @email", new Dictionary<string, object> { { "@email", receive } });
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            return "该邮箱已注册，如忘记密码请找回！";
                        }
                        EmailHelper.SendEmailByQQ(receive, "淮安市三轮车开黑网站-注册账号", "本次验证码为【" + code + "】,有效期30分钟！", type, code);
                        return "验证码已发送，请检查邮箱尽快注册账号！";
                    }
                    else if (type == Constants.CodeTypeForgetPwd)
                    {// 忘记密码
                        EmailHelper.SendEmailByQQ(receive, "淮安市三路车开黑网站-重置密码", "本次验证码为【" + code + "】,有效期30分钟！", type, code);
                        return "验证码已发送，请检查邮箱尽快修改密码！";
                    }
                    else if (type == Constants.CodeTypeChangeEmail)
                    {// 更换邮箱
                        DataTable dt = _sql.Query("SELECT UserInfoId FROM UserInfo WHERE Email = @email", new Dictionary<string, object> { { "@email", receive } });
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            throw new Exception("当前邮箱已存在账号！");
                        }
                        EmailHelper.SendEmailByQQ(receive, "淮安市三路车开黑网站-更换邮箱", "本次验证码为【" + code + "】,有效期30分钟！", type, code);
                        return "验证码已发送，请检查邮箱尽快修改邮箱！";
                    }
                }
                else
                {// 短信接收
                    // to-do...
                }
                return "获取失败";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 验证用户接收验证码
        /// </summary>
        /// <param name="receive">接收者</param>
        /// <param name="type">验证码类型</param>
        /// <param name="verifycode">验证码</param>
        /// <param name="period">验证码有效时间（分钟）</param>
        /// <returns></returns>
        public string ValidReceiveVerifyCode(string receive, string type, string verifycode, int period = 30)
        {
            try
            {
                DataTable dt = _sql.Query("SELECT VerifyCodeLogId,Code FROM tbl_verifycodelog WHERE Receive = @receive AND CodeType = @type AND CreatedOn >= @time AND IsSuccess = 1",
                    new Dictionary<string, object> { { "@receive", receive }, { "@type", type }, { "@time", DateTimeUtils.NowBeijing().AddMinutes(-period) } });
                if (dt == null || dt.Rows.Count == 0)
                    return "验证码不存在或已过期，请重新获取验证码！";
                if (verifycode != Cast.ConToString(dt.Rows[0]["Code"]))
                    return "验证码不存在或已过期，请重新获取验证码！";
                return string.Empty;
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// 创建账号
        /// </summary>
        /// <param name="receive"></param>
        /// <param name="verifycode"></param>
        /// <returns></returns>
        public string CreateUser(string receive, string verifycode)
        {
            try
            {
                string valid = ValidReceiveVerifyCode(receive, Constants.CodeTypeRegister, verifycode);
                if (!string.IsNullOrEmpty(valid))
                {
                    return valid;
                }
                string account = string.Empty;
                string password = Rand.Str(8);
                DataTable dtEmail = _sql.Query("SELECT UserInfoId FROM UserInfo WHERE Email = @email", new Dictionary<string, object> { { "@email", receive } });
                if (dtEmail != null && dtEmail.Rows.Count > 0)
                {
                    return "当前邮箱账号密码已发送，请检查邮箱！";
                }
                DataTable dtAccount = null;
                do
                {
                    account = Rand.Number(8);
                    dtAccount = _sql.Query("SELECT UserInfoId FROM UserInfo WHERE Account = @account", new Dictionary<string, object> { { "@account", account } });
                } while (dtAccount != null && dtAccount.Rows.Count > 0);
                _sql.OpenDb();
                UserInfo user = new UserInfo();
                user.Account = account;
                user.Password = WuYao.GetMd5(password + Constants.PasswordSalt);
                user.Email = receive;
                Guid userId = _sql.Create(user);
                DataTable dtRole = _sql.Query("SELECT RoleInfoId FROM RoleInfo WHERE RoleCode = @code", new Dictionary<string, object> { { "@code", RoleKey.JCQX } });
                if (dtRole != null && dtRole.Rows.Count > 0)
                {
                    UserInRole ur = new UserInRole();
                    ur.UserInfoId = userId;
                    ur.UserCode = account;
                    ur.RoleCode = RoleKey.JCQX;
                    ur.RoleInfoId = Guid.Parse(Cast.ConToString(dtRole.Rows[0]["RoleInfoId"]));
                    _sql.Create(ur);
                }
                if (receive.Contains("@"))
                {
                    EmailHelper.SendEmailByQQ(receive, "淮安市三轮车开黑网站-注册账号", string.Format("账号：{0} \n 密码：{1}", account, password), Constants.CodeTypeRegister);
                    return "账号密码已发送至注册邮箱！";
                }
                else
                {
                    return "";
                }
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
        /// 重置密码
        /// </summary>
        /// <param name="receive"></param>
        /// <param name="verifycode"></param>
        /// <returns></returns>
        public string ResetPwd(string receive, string verifycode)
        {
            try
            {
                string valid = ValidReceiveVerifyCode(receive, Constants.CodeTypeForgetPwd, verifycode);
                if (!string.IsNullOrEmpty(valid))
                {
                    return valid;
                }
                string password = Rand.Str(8);
                _sql.OpenDb();
                _sql.Execute("UPDATE UserInfo SET Password = @password WHERE Email = @receive", new Dictionary<string, object> { { "@password", WuYao.GetMd5(password + Constants.PasswordSalt) }, { "@receive", receive } });
                if (receive.Contains("@"))
                {
                    EmailHelper.SendEmailByQQ(receive, "淮安市三轮车开黑网站-重置密码", string.Format("重置密码：{0}；请尽快登录并修改密码！", password), Constants.CodeTypeForgetPwd);
                    return "重置密码已发送至注册邮箱！";
                }
                else
                {
                    return "";
                }
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
        /// 获取头像附件信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Attachment GetAvatar(string id)
        {
            return _sql.Search<Attachment>(id);
        }
    }
}

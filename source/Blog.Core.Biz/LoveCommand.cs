using System;
using System.Collections.Generic;
using Blog.Core.Common;
using Blog.Core.Model;
using Newtonsoft.Json;
using System.Data;

namespace Blog.Core.Biz
{
    /// <summary>
    /// LoveCommand
    /// </summary>
    public class LoveCommand 
    {
        ///// <summary>
        ///// 构造函数
        ///// </summary>
        ///// <param name="identity"></param>
        //public LoveCommand(UserIdentity identity) : base(identity)
        //{
        //}

        ///// <summary>
        ///// Get Lover By Name
        ///// </summary>
        ///// <returns></returns>
        //public LoveModel GetLoverByName(string name)
        //{
        //    try
        //    {
        //        LoveModel lover = new LoveModel();
        //        lover = CacheHelper.Get<LoveModel>(name);
        //        return lover;
        //    }
        //    catch (Exception ex)
        //    {
        //        _log.Error(ex);
        //        throw ex;
        //    }
        //}

        ///// <summary>
        ///// Add Lover
        ///// </summary>
        ///// <param name="love"></param>
        ///// <returns></returns>
        //public string AddLover(LoveModel lover)
        //{
        //    try
        //    {
        //        CacheHelper.Insert(lover.Name, lover);
        //        _log.WriteLog(string.Format("Id:{0};Name:{1};Age:{2};Phone:{3}", lover.Id, lover.Name, lover.Age, lover.Phone));
        //        _log.Info(string.Format("Id:{0};Name:{1};Age:{2};Phone:{3}", lover.Id, lover.Name, lover.Age, lover.Phone));
        //        return "Success";
        //    }
        //    catch (Exception ex)
        //    {
        //        _log.Error(ex);
        //        throw ex;
        //    }
        //}

        ///// <summary>
        ///// Delete Lover
        ///// </summary>
        ///// <param name="name"></param>
        ///// <returns></returns>
        //public string DelLover(string name)
        //{
        //    try
        //    {
        //        CacheHelper.Remove(name);
        //        return "Success";
        //    }
        //    catch (Exception ex)
        //    {
        //        _log.Error(ex);
        //        throw ex;
        //    }
        //}

        //public string AesEncrypt(string text, string key, string iv)
        //{
        //    return WuYao.AesEncrypt(text, key, iv);
        //}

        //public string AesDecrypt(string text, string key, string iv)
        //{
        //    return WuYao.AesDecrypt(text, key, iv);
        //}

        ///// <summary>
        ///// 获取一些东西
        ///// </summary>
        ///// <returns></returns>
        //public string GetSomethings()
        //{
        //    string result = string.Empty;
        //    List<LoveModel> list1 = new List<LoveModel>();
        //    list1.Add(new LoveModel
        //    {
        //        Id = 1,
        //        Name = "Jack",
        //        Age = 20
        //    });
        //    list1.Add(new LoveModel
        //    {
        //        Id = 2,
        //        Name = "Meiko",
        //        Age = 22
        //    });
        //    DataTable dt = list1.FillDataTable();
        //    List<LoveModel> list = dt.ToModelList<LoveModel>();
        //    return JsonConvert.SerializeObject(list);
        //}

        private LogHelper _log = new LogHelper();

        public void ValidUser(string account, string password)
        {
            _log.WriteLog("登录：" + "账号" + account + "密码" + password);
        }

        public void ChangePassword(string account, string password)
        {
            _log.WriteLog("修改密码：" + "账号" + account + "密码" + password);
        }
    }
}

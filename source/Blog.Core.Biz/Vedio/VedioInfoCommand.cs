using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Blog.Core.Biz.Attachments;
using Blog.Core.Common;
using Blog.Core.Model;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.IO;

namespace Blog.Core.Biz.Vedio
{
    /// <summary>
    /// 视频Command
    /// </summary>
    public class VedioInfoCommand : BaseCommand
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="identity"></param>
        public VedioInfoCommand(UserIdentity identity) : base(identity)
        {
        }

        /// <summary>
        /// 获取视频信息List
        /// </summary>
        /// <param name="searchList"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="type">1-所有；2-最新；3-最热；4-我的；</param>
        /// <returns></returns>
        public VedioInfoListData GetVedioInfoList(List<SearchCondition> searchList, int pageIndex, int pageSize, string orderBy, int type)
        {
            try
            {
                VedioInfoListData result = new VedioInfoListData();

                #region 初始化数据
                string sqlString = @"SELECT * FROM VedioInfo WHERE IsDeleted = 0";
                Dictionary<string, object> paramList = new Dictionary<string, object>();
                switch (type)
                {
                    case 4:
                        sqlString += " AND CreatedBy = @account";
                        paramList.Add("@account", _identity.UserAccount);
                        break;
                    default:
                        sqlString += " AND IsPublic = 1";
                        break;
                }
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
                                    paramList.Add(string.Concat("@", s.Key), Cast.ConToInt(s.Value));
                                }
                                else if (s.Key.ToLower().Contains("before"))
                                {
                                    sqlString += string.Concat(" AND ", s.Table, s.Key.Replace("before", ""), " <= @", s.Key);
                                    paramList.Add(string.Concat("@", s.Key), Cast.ConToInt(s.Value));
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
                int recordCount = 0;
                DataTable dtResult = _sql.Query(sqlString, paramList, orderBy, pageSize, pageIndex, out recordCount);
                #endregion

                #region 构建结果
                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    result.RecordCount = recordCount;
                    result.RecordList = dtResult.ToModelList<VedioInfoListModel>();
                }
                #endregion

                return result;
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// 删除视频信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public string DeleteVedioInfo(string[] ids)
        {
            try
            {
                _sql.OpenDb();
                _sql.BeginTrans();
                if (ids == null || ids.Length == 0)
                {
                    throw new Exception("视频Id不能为空！");
                }
                foreach (string id in ids)
                {
                    _sql.Delete(id, VedioInfo.TableName);
                }
                _sql.Commit();
                return Constants.DeleteSuccessMssg;
            }
            catch (Exception ex)
            {
                _sql.Rollback();
                _log.Error(ex);
                throw;
            }
            finally
            {
                _sql.CloseDb();
            }
        }

        /// <summary>
        /// 上传视频信息
        /// </summary>
        /// <param name="vedioinfo"></param>
        /// <param name="files"></param>
        public void Upload(VedioEditModel vedioinfo, List<IFormFile> files)
        {
            AttachmentCommand attach = new AttachmentCommand(_identity);
            List<Attachment> attachments = new List<Attachment>();
            try
            {
                _sql.OpenDb();
                attachments = attach.UploadFile(VedioInfo.TableName, vedioinfo.VedioInfoId, files, Constants.VedioPath);
                VedioInfo info = new VedioInfo();
                info.VedioInfoId = Guid.Parse(vedioinfo.VedioInfoId);
                foreach (var att in attachments)
                {
                    if (att.MimeType.StartsWith("video"))
                    {
                        info.SourceType = att.MimeType;
                        info.SourceUrl = att.FilePath.Split('\\').Last();
                        info.Size = (att.FileSize / 1024).ToString("0") + "kb";
                    }
                    else if (att.MimeType.StartsWith("image"))
                    {
                        info.Poster = att.FilePath.Split('\\').Last();
                    }
                }
                info.Description = vedioinfo.Description ?? "暂无描述";
                info.IsPublic = vedioinfo.IsPublic ? 1 : 0;
                _sql.Create(info);
            }
            catch (Exception ex)
            {
                string[] ids = attachments.Select(item => item.AttachmentId.ToString()).ToArray();
                attach.DeleteFile(ids);
                _log.Error(ex);
                throw;
            }
        }
    }
}

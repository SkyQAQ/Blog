using Blog.Core.Common;
using Blog.Core.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Blog.Core.Biz.Attachments
{
    /// <summary>
    /// 附件Command
    /// </summary>
    public class AttachmentCommand : BaseCommand
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="identity"></param>
        public AttachmentCommand(UserIdentity identity) : base(identity)
        {
        }

        /// <summary>
        /// 获取附件信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Attachment GetAttachment(string id)
        {
            return _sql.Search<Attachment>(id);
        }

        /// <summary>
        /// 获取附件列表信息
        /// </summary>
        /// <param name="searchList"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public AttachmentListData GetAttachmentList(List<SearchCondition> searchList, int pageIndex, int pageSize)
        {
            try
            {
                AttachmentListData result = new AttachmentListData();

                #region 初始化数据
                string sqlString = @"SELECT * FROM Attachment WHERE IsDeleted = 0";
                Dictionary<string, object> paramList = new Dictionary<string, object>();
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
                                    paramList.Add(string.Concat("@", s.Key), Cast.ConToDateTime(s.Value).ToString("yyyy-MM-dd HH:mm:ss"));
                                }
                                else if (s.Key.ToLower().Contains("before"))
                                {
                                    sqlString += string.Concat(" AND ", s.Table, s.Key.Replace("before", ""), " <= @", s.Key);
                                    paramList.Add(string.Concat("@", s.Key), Cast.ConToDateTime(s.Value).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"));
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
                DataTable dtResult = null;
                int recordCount = 0;
                if (pageSize > 0 && pageIndex > 0)
                {
                    dtResult = _sql.Query(sqlString, paramList, " ORDER BY CreatedOn DESC", pageSize, pageIndex, out recordCount);
                }
                else
                {
                    sqlString += " ORDER BY CreatedOn DESC";
                    dtResult = _sql.Query(sqlString, paramList);
                }
                #endregion

                #region 构建结果
                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    result.RecordCount = recordCount;
                    result.RecordList = dtResult.ToModelList<AttachmentListModel>();
                }
                #endregion

                return result;
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// 上传附件
        /// </summary>
        /// <param name="moduleType">模块类型</param>
        /// <param name="moduleId">模块Id</param>
        /// <param name="file">文件</param>
        /// <param name="fileDir">文件目录</param>
        /// <returns></returns>
        public Attachment UploadFile(string moduleType, string moduleId, IFormFile file, string fileDir = Constants.AttachmentPath)
        {
            return UploadFile(moduleType, moduleId, new List<IFormFile> { { file } }, fileDir)[0];
        }

        /// <summary>
        /// 上传多个附件
        /// </summary>
        /// <param name="moduleType">模块类型</param>
        /// <param name="moduleId">模块Id</param>
        /// <param name="files">文件</param>
        /// <param name="fileDir">文件目录</param>
        /// <returns></returns>
        public List<Attachment> UploadFile(string moduleType, string moduleId, List<IFormFile> files, string fileDir = Constants.AttachmentPath)
        {
            try
            {
                if (files == null || files.Count == 0) throw new FileNotFoundException("文件丢失");
                string[] type = _config.Upload_Type == "*" ? null : _config.Upload_Type.Split(',');
                if (files.Count > Cast.ConToInt(_config.Upload_MaxCount)) throw new FileLoadException("每次最多上传" + _config.Upload_MaxCount + "个附件");
                foreach (IFormFile file in files)
                {
                    if(type !=null && type.Length > 0 && !type.Contains(Path.GetExtension(file.FileName))) throw new FileFormatException("文件" + file.FileName + "类型不允许");
                    if (file.Length / 1024 > Cast.ConToInt(_config.Upload_MaxLength) * 1024) throw new FileLoadException("文件" + file.FileName + "超过" + _config.Upload_MaxLength + "M");
                }
                fileDir = Path.Combine(Constants.ServerMapPath() + fileDir);
                if (!Directory.Exists(fileDir))
                {
                    Directory.CreateDirectory(fileDir);
                }
                _sql.OpenDb();
                List<Attachment> result = new List<Attachment>();
                foreach (IFormFile file in files)
                {
                    string fileName = DateTimeUtils.NowBeijing().ToString("yyyyMMdd") + "_" + moduleType + "_" + Guid.NewGuid();
                    string[] array = file.FileName.Split('.');
                    string fileSufFix = array[array.Length - 1];
                    string filePath = Path.Combine(fileDir, fileName + "." + fileSufFix);
                    using (FileStream fs = File.Create(filePath))
                    {
                        file.CopyTo(fs);
                        fs.Flush();
                    }
                    Attachment attachment = new Attachment();
                    attachment.FilePath = filePath;
                    attachment.FileName = file.FileName;
                    attachment.FileSize = file.Length;
                    attachment.MimeType = file.ContentType;
                    attachment.ModuleType = moduleType;
                    attachment.ModuleId = moduleId;
                    attachment.AttachmentId = _sql.Create(attachment);
                    result.Add(attachment);
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
        /// 删除附件
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public string DeleteFile(string[] ids)
        {
            try
            {
                if (ids == null || ids.Length == 0)
                    throw new Exception("请选择待删除的附件！");
                _sql.OpenDb();
                foreach (string id in ids)
                {
                    Attachment attachment = _sql.Search<Attachment>(id);
                    if (attachment == null)
                        return Constants.DeleteSuccessMssg;
                    if (File.Exists(attachment.FilePath))
                        File.Delete(attachment.FilePath);
                    _sql.Delete(id, Attachment.TableName);
                }
                return Constants.DeleteSuccessMssg;
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
        /// 下载附件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mimeType"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public byte[] DownloadFile(string id, out string mimeType, out string fileName)
        {
            try
            {
                Attachment attachment = _sql.Search<Attachment>(id);
                mimeType = attachment.MimeType;
                fileName = attachment.FileName;
                return File.ReadAllBytes(attachment.FilePath);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// 打包附件下载
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public byte[] DownloadZip(string[] ids)
        {
            try
            {
                if (ids == null || ids.Length == 0)
                {
                    throw new Exception("请选择待下载附件！");
                }
                string sql = "SELECT FilePath,FileName FROM Attachment WHERE AttachmentId IN ($condition$)";
                string conditon = "";
                Dictionary<string, object> paramList = new Dictionary<string, object>();
                int count = 1;
                foreach (string id in ids)
                {
                    conditon += "@id" + count + ",";
                    paramList.Add("@id" + count, id);
                    count++;
                }
                sql = sql.Replace("$condition$", conditon.Remove(conditon.Length - 1));
                DataTable dt = _sql.Query(sql, paramList);
                Dictionary<string, string> list = new Dictionary<string, string>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach(DataRow row in dt.Rows)
                    {
                        list.Add(Cast.ConToString(row["FilePath"]), Cast.ConToString(row["FileName"]));
                    }
                }
                return FileHelper.ZipFile(list);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                throw ex;
            }
        }
    }
}

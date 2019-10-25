using Microsoft.AspNetCore.Mvc;
using Blog.Core.Common;
using Blog.Core.Model;
using Blog.Core.Biz.Attachments;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Blog.Core.Controllers
{
    /// <summary>
    /// 附件Controller
    /// </summary>
    [Route("api/attachment")]
    public class AttachmentController : BaseController
    {
        /// <summary>
        /// 获取附件列表数据
        /// </summary>
        /// <param name="searchList"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet, Route("list")]
        public AttachmentListData GetUserList(string searchList, int pageIndex, int pageSize)
        {
            return new AttachmentCommand(UserIdentity).GetAttachmentList(!string.IsNullOrEmpty(searchList) ? JsonConvert.DeserializeObject<List<SearchCondition>>(searchList) : null, pageIndex, pageSize);
        }

        /// <summary>
        /// 上传附件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost, Route("upload")]
        public string UploadFile(FileInfo file)
        {
            return new AttachmentCommand(UserIdentity).UploadFile(file.ModuleType, file.ModuleId, GetFile()).AttachmentId.ToString();
        }

        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost, Route("delete")]
        public string DeleteFile(string[] ids)
        {
            return new AttachmentCommand(UserIdentity).DeleteFile(ids);
        }

        /// <summary>
        /// 下载附件
        /// </summary>
        /// <param name="id">附件Id</param>
        /// <returns></returns>
        [HttpGet, Route("download")]
        public FileContentResult Download(string id)
        {
            string mimeType;
            string fileName;
            var fileContents = new AttachmentCommand(UserIdentity).DownloadFile(id, out mimeType, out fileName);
            return File(fileContents, mimeType, fileName);
        }

        /// <summary>
        /// 打包附件下载
        /// </summary>
        /// <param name="ids">附件Ids</param>
        /// <returns></returns>
        [HttpPost, Route("downloadzip")]
        public FileContentResult DownloadZip(string[] ids)
        {
            var fileContents = new AttachmentCommand(UserIdentity).DownloadZip(ids);
            return File(fileContents, System.Net.Mime.MediaTypeNames.Application.Zip);
        }        
    }
}
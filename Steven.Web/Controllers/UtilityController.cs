using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Steven.Web.Framework.Controllers;
using Steven.Core.Utilities;
using Steven.Domain.Services;
using Steven.Core.Cache;
using Steven.Domain.Repositories;
using System.IO;

namespace Steven.Web.Controllers
{
    public class UtilityController : WebSiteController
    {
        public ICacheManager Cache { get; set; }
        public IAttachmentSvc AttachmentSvc { get; set; }
        public IAttachmentRepository AttachmentRepository { get; set; }
        public ISysConfigRepository SysConfigRepository { get; set; }
        public ActionResult GetVerifyCode()
        {
            return File(VeryfyCodeUtility.CreateVerifyImage(Session, 5), @"image/jpeg");
        }

        public ActionResult ClearCache()
        {
            Cache.Clear();
            return Content("ok");
        }

        public ActionResult DownloadFile(long id)
        {
            var atta = AttachmentRepository.Get(id);
            if (atta == null)
            {
                return HttpNotFound("不存在该附件");
            }
            var attaPath = Path.Combine(SysConfigRepository.UploadPath,atta.FilePath);
            var fileContent = AttachmentSvc.GetFileContentType(atta.FileExt);
            return File(attaPath, fileContent,atta.Name+atta.FileExt);
        }
    }
}
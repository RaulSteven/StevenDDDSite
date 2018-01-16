using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Steven.Web.Framework.Controllers;
using Steven.Domain.Repositories;
using Steven.Domain.Services;
using System.Threading.Tasks;
using Steven.Domain.Enums;
using System.Collections;
using Steven.Domain.Models;
using Steven.Core.Extensions;
using Steven.Domain.ViewModels;

namespace Steven.Web.Areas.Admin.Controllers
{
    public class AttachmentController : AdminController
    {
        public IAttachmentSvc AttachmentSvc { get; set; }
        public IAttachmentRepository AttachmentRepository { get; set; }
        
        [HttpPost]
        public ActionResult Index()
        {
            return Json("ok");
            //return View();
        }

        [HttpPost]
        public JsonResult BatchUpload(TableSource src, long srcId = 0, int type = 0, int width = 100, int height = 100)
        {
            var postedFile = Request.Files["Filedata"];
            var table = new Hashtable();
            if (null != postedFile && postedFile.ContentLength > 0)
            {
                var oAtt = new Attachment
                {
                    Descript = "",
                    Source = src,
                    SourceId = 0,
                    CreateUserId = User.UserModel.UserId,
                    CreateUserName = User.UserModel.UserName,
                    FilePath = "",
                };
                if ( AttachmentSvc.Save(postedFile, oAtt))
                {
                     LogRepository.Insert(TableSource.Attachments, OperationType.Insert,oAtt.Id);
                }
                table.Add("id", oAtt.Id);
                table.Add("imgPath", AttachmentSvc.GetPicUrl(oAtt.Id, width, height));
                table.Add("imgPathOriginal", AttachmentSvc.GetPicUrl(oAtt.Id));
                table.Add("name", oAtt.Name);
                table.Add("time", oAtt.UpdateTime.ToDisplayDateTime());
                table.Add("size", oAtt.FileSizeStr);
                table.Add("type", type);
                table.Add("statusCode", JsonModelCode.Succ);
            }
            else
            {
                table.Add("statusCode", JsonModelCode.Error);
                table.Add("message", "上传失败");
            }
            return Json(table);
        }
    }
}
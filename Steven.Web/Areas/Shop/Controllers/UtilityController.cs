using Steven.Core.Extensions;
using Steven.Domain.Enums;
using Steven.Domain.Models;
using Steven.Domain.Services;
using Steven.Domain.ViewModels;
using Steven.Web.Framework.Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Steven.Web.Areas.Shop.Controllers
{
    public class UtilityController : ShopController
    {
        public IAttachmentSvc AttachmentSvc { get; set; }

        [HttpPost]
        public JsonResult BatchUpload(TableSource src,string fileId, long srcId = 0, int type = 0, int width = 100, int height = 100)
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
                if (AttachmentSvc.Save(postedFile, oAtt))
                {
                    LogRepository.Insert(TableSource.Attachments, OperationType.Insert, oAtt.Id);
                }
                table.Add("id", oAtt.Id);
                table.Add("imgPath", AttachmentSvc.GetPicUrl(oAtt.Id, width, height));
                table.Add("imgPathOriginal", AttachmentSvc.GetPicUrl(oAtt.Id));
                table.Add("name", oAtt.Name);
                table.Add("time", oAtt.UpdateTime.ToDisplayDateTime());
                table.Add("size", oAtt.FileSizeStr);
                table.Add("type", type);
                table.Add("statusCode", JsonModelCode.Succ);
                table.Add("fileId",postedFile.FileName);
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
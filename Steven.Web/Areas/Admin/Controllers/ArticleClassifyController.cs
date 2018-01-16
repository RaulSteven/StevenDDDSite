using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Steven.Domain.Enums;
using Steven.Domain.Models;
using Steven.Domain.Repositories;
using Steven.Domain.ViewModels;
using Steven.Web.Framework.Controllers;
using Steven.Web.Framework.Security;
using Newtonsoft.Json;
using Steven.Domain.Services;
using Steven.Web.Areas.Admin.Models;

namespace Steven.Web.Areas.Admin.Controllers
{
    public class ArticleClassifyController : AdminController
    {
        public IArticleClassifyRepository ArticleClassifyRepository { get; set; }
        public IArticleRepository ArticleRepository { get; set; }
        public IAttachmentSvc AttachmentSvc { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetZTreeJson()
        {
            var list =  ArticleClassifyRepository.GetListByZTree();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetClassify(long id)
        {
            var result = new JsonModel();
            var articleClz = ArticleClassifyRepository.Get(id);
            if (articleClz == null)
            {
                result.msg = "找不到记录！";
                return Json(result);
            }
            articleClz.PicUrl = AttachmentSvc.GetPicUrl(articleClz.PicAttaId, 100, 100);
            result.data = articleClz;
            result.code = JsonModelCode.Succ;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Save(ArticleClassifyModel model)
        {
            var result = new JsonModel();           
            ArticleClassify parent = null;
            if (model.PId != 0)
            {
                parent = ArticleClassifyRepository.Get(model.PId);
                if (parent == null)
                {
                    result.msg = "找不到id为" + model.PId + "的主分类！";
                    return Json(result);
                }
            }

            var operationType = OperationType.Insert;
            ArticleClassify articleClassify = null;
            if (model.Id > 0)
            {
                articleClassify = ArticleClassifyRepository.Get(model.Id);
                if (articleClassify == null)
                {
                    result.msg = "记录不存在了";
                    return Json(result);
                }
                operationType = OperationType.Update;
            }
            else
            {
                articleClassify = new ArticleClassify();
            }
            Mapper.Map(model, articleClassify);
            ArticleClassifyRepository.Save(articleClassify, parent);
            LogRepository.Insert(TableSource.ArticleClassify, operationType, articleClassify.Id);
            result.msg = "保存成功！";
            result.code = JsonModelCode.Succ;
            result.data = articleClassify;
            return Json(result);
        }

        [HttpPost]
        public ActionResult Delete(long id)
        {
            var result = ArticleClassifyRepository.Delete(id);
            if (result.code == JsonModelCode.Succ)
            {
                LogRepository.Insert(TableSource.ArticleClassify, OperationType.Delete, result.data.ToString());
            }
            return Json(result);
        }
    }
}
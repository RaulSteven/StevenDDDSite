using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Steven.Web.Framework.Controllers;
using Steven.Domain.ViewModels;
using Steven.Domain.Repositories;
using Steven.Web.Areas.Admin.Models;
using Steven.Domain.Enums;
using AutoMapper;
using Newtonsoft.Json;
using Steven.Domain.Models;
using Steven.Core.Utilities;
using Steven.Web.Framework.Security;

namespace Steven.Web.Areas.Admin.Controllers
{
    public class ArticleController : AdminController
    {
        public IArticleClassifyRepository ArticleClassifyRepository { get; set; }
        public IArticleRepository ArticleRepository { get; set; }

        [ValidatePage]
        // GET: Admin/Article
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetList(string keyWord, long? classifyId)
        {
            var search = GetSearchModel();
            var list = ArticleRepository.GetPager(keyWord,classifyId, search);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [ValidateButton(ActionName ="Index",Buttons = SysButton.Edit)]
        public ActionResult Edit(long id,string reUrl)
        {
            ViewBag.ReUrl = reUrl ?? Url.Action("Index");
            var model = new ArticleModel()
            {
                ArticleDateTime = DateTime.Now,
                ExecutiveEditor = User.UserModel.UserName,
                CommonStatus = CommonStatus.Enabled,
                ArticleTarget = ArticleTarget.ThisPage,
                PartialViewCode = ArticleDetailType.Article
            };
            if (id == 0)
            {
                return View(model);
            }
            var article = ArticleRepository.Get(id);
            if (article == null)
            {
                ShowErrorMsg();
                return Redirect(ViewBag.ReUrl);
            }
            Mapper.Map(article, model);
            var clz = ArticleClassifyRepository.Get(model.ClassifyId);
            if (clz != null)
            {
                model.ClassifyName = clz.Name;
            }
            if (!string.IsNullOrEmpty(model.FocusMap))
            {
                model.FocusMapList = JsonConvert.DeserializeObject<List<ArticleFocusMapModel>>(model.FocusMap);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [ValidateButton(ActionName ="Index",Buttons = SysButton.Edit)]
        public ActionResult Edit(ArticleModel model)
        {
            var result = new JsonModel();
            var opType = OperationType.Insert;
            Article article = null;
            if (model.Id>0)
            {
                article = ArticleRepository.Get(model.Id);
                if (article == null)
                {
                    result.msg = "找不到记录！";
                    return Json(result);
                }
                opType = OperationType.Update;
            }
            else
            {
                article = new Article();
            }
            Mapper.Map(model, article);
            if (string.IsNullOrEmpty(article.ArticleIndex))
            {
                article.ArticleIndex = Math.Abs(article.GetHashCode()).ToString();
            }
            if (article.PartialViewCode == ArticleDetailType.Image)
            {
                var lstFocusMap = JsonConvert.DeserializeObject<List<ArticleFocusMapModel>>(model.FocusMap);
                article.ImageCount = (lstFocusMap != null && lstFocusMap.Any()) ? lstFocusMap.Count : 0;
            }
            else
            {
                var lstImg = StringUtility.GetImgUrl(article.ArticleContent);
                article.ImageCount = (lstImg != null && lstImg.Any()) ? lstImg.Count : 0;
            }
            ArticleRepository.Save(article);
            LogRepository.Insert(TableSource.Article, opType, article.Id);
            result.code = JsonModelCode.Succ;
            ShowSuccMsg("保存成功！");
            return Json(result);
        }
    }
}
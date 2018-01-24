using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Steven.Domain.Repositories;
using Steven.Web.Framework.Controllers;
using Steven.Web.Framework.Extensions;

namespace Steven.Web.Controllers
{
    public class HomeController : WebSiteController
    {
        public IArticleRepository ArticleRepository { get; set; }

        public ActionResult Index()
        {
            var list = ArticleRepository.GetAll();
            return View(list);
        }

        public ActionResult Detail(string id)
        {
            var article = ArticleRepository.GetByIndex(id);
            if (article == null)
            {
                return Redirect(Url.Home());
            }
            //update view count
            ArticleRepository.UpdateViewCount(article.Id);
            return View(article);
        }
    }
}
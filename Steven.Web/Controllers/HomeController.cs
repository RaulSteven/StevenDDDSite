using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Steven.Domain.Repositories;
using Steven.Web.Framework.Controllers;

namespace Steven.Web.Controllers
{
    public class HomeController : WebSiteController
    {
        public ISysConfigRepository SysConfigRepository { get; set; }
        public ISysCaseRepository SysCaseRepository { get; set; }
        public ISysPartnerRepository SysPartnerRepository { get; set; }
        // GET: Main
        public ActionResult Index()
        {
            var list = SysPartnerRepository.GetAll();
            return View(list);
        }

        public ActionResult Case()
        {
            var list = SysCaseRepository.GetAll();
            return View(list);
        }

        public ActionResult ShowCase(long id)
        {
            var sysCase = SysCaseRepository.Get(id);
            if (sysCase == null)
            {
                return RedirectToAction("Case");
            }
            return View(sysCase);
        }

        public ActionResult MiniApp()
        {
            return View();
        }
    }
}
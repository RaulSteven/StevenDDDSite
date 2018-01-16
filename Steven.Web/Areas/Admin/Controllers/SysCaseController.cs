using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Steven.Web.Framework.Controllers;
using Steven.Domain.Models;
using Steven.Domain.Repositories;
using Steven.Web.Areas.Admin.Models;
using AutoMapper;
using Steven.Domain.ViewModels;
using Steven.Domain.Enums;

namespace Steven.Web.Areas.Admin.Controllers
{
    public class SysCaseController : AdminController
    {
        public ISysCaseRepository SysCaseRepository { get; set; }
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetList(string name)
        {
            var search = GetSearchModel();
            var list = SysCaseRepository.GetPager(name, search);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(long id, string reUrl)
        {
            ViewBag.ReUrl = reUrl ?? Url.Action("Index");

            var model = new SysCaseModel();
            if (id == 0)
            {
                return View(model);
            }

            var sysCase = SysCaseRepository.Get(id);
            if (sysCase == null)
            {
                ShowErrorMsg();
                return Redirect(ViewBag.ReUrl);
            }
            Mapper.Map(sysCase, model);
            
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SysCaseModel model)
        {
            var result = new JsonModel();

            SysCase sysCase = null;
            OperationType opType = OperationType.Insert;
            if (model.Id == 0)
            {
                sysCase = new SysCase();
            }
            else
            {
                sysCase = SysCaseRepository.Get(model.Id);
                if (sysCase == null)
                {
                    result.msg = $"找不到id为{model.Id}的记录";
                    return Json(result);
                }
            }
            Mapper.Map(model, sysCase);
            SysCaseRepository.Save(sysCase);
            LogRepository.Insert(TableSource.SysCase, opType, sysCase.Id);
            result.code = JsonModelCode.Succ;
            ShowSuccMsg("保存成功！");
            return Json(result);
        }
    }
}
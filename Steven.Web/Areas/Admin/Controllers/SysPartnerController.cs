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
    public class SysPartnerController : AdminController
    {
        public ISysPartnerRepository SysPartnerRepository { get; set; }
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetList(string name)
        {
            var search = GetSearchModel();
            var list = SysPartnerRepository.GetPager(name, search);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(long id, string reUrl)
        {
            ViewBag.ReUrl = reUrl ?? Url.Action("Index");

            var model = new SysPartnerModel();
            if (id == 0)
            {
                return View(model);
            }

            var partner = SysPartnerRepository.Get(id);
            if (partner == null)
            {
                ShowErrorMsg();
                return Redirect(ViewBag.ReUrl);
            }
            Mapper.Map(partner, model);
            
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SysPartnerModel model)
        {
            var result = new JsonModel();

            SysPartner partner = null;
            OperationType opType = OperationType.Insert;
            if (model.Id == 0)
            {
                partner = new SysPartner();
            }
            else
            {
                partner = SysPartnerRepository.Get(model.Id);
                if (partner == null)
                {
                    result.msg = $"找不到id为{model.Id}的记录";
                    return Json(result);
                }
            }
            Mapper.Map(model, partner);
            SysPartnerRepository.Save(partner);
            LogRepository.Insert(TableSource.SysPartner, opType, partner.Id);
            result.code = JsonModelCode.Succ;
            ShowSuccMsg("保存成功！");
            return Json(result);
        }
    }
}
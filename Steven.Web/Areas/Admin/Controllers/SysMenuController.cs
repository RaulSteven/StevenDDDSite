using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Steven.Web.Framework.Controllers;
using Steven.Domain.Models;
using Steven.Domain.Repositories;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Steven.Domain.ViewModels;
using Steven.Domain.Enums;
using AutoMapper;
using Steven.Web.Framework.Security;

namespace Steven.Web.Areas.Admin.Controllers
{
    public class SysMenuController : AdminController
    {
        public ISysMenuRepository SysMenuRepository { get; set; }

        [ValidatePage]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetJsonList()
        {
            var list =  SysMenuRepository.GetJsonList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(SysMenuModel model)
        {
            var result = new JsonModel();
            SysMenu menu = null;
            var opType = OperationType.Insert;
            if (model.Id > 0)
            {
                opType = OperationType.Update;
                menu =  SysMenuRepository.Get(model.Id);
                if (menu == null)
                {
                    result.msg = "找不到记录！";
                    return Json(result);
                }
            }
            else
            {
                menu = new SysMenu();
            }
            Mapper.Map(model, menu);
            if (model.ButtonArray != null)
            {
                foreach (var btn in model.ButtonArray)
                {
                    menu.Buttons = menu.Buttons | btn;
                }
            }
            else
            {
                menu.Buttons = SysButton.None;
            }
             SysMenuRepository.Save(menu);
             LogRepository.Insert(TableSource.SysMenu, opType, menu.Id);
            result.data = menu;
            result.code = JsonModelCode.Succ;
            result.msg = "保存成功！";
            return Json(result);
        }

        public ActionResult GetMenu(long id)
        {
            var result = new JsonModel();
            var apart =  SysMenuRepository.Get(id);
            if (apart == null)
            {
                result.msg = "找不到记录！";
                return Json(result);
            }
            result.data = apart;
            result.code = JsonModelCode.Succ;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(long id)
        {
            var result = new JsonModel();
           var deleIds =   SysMenuRepository.Delete(id);
             LogRepository.Insert(TableSource.SysMenu, OperationType.Delete, deleIds);
            result.msg = "删除成功！";
            result.code = JsonModelCode.Succ;
            return Json(result);
        }
    }
}
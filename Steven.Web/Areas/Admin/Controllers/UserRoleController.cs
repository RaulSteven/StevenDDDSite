using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Steven.Domain.Repositories;
using Steven.Web.Framework.Controllers;
using System.Threading.Tasks;
using Steven.Domain.Models;
using Steven.Web.Areas.Admin.Models;
using AutoMapper;
using Steven.Core.Extensions;
using Steven.Domain.ViewModels;
using Steven.Domain.Enums;
using Newtonsoft.Json;
using Steven.Domain.Services;
using Steven.Core.Utilities;
using Steven.Web.Framework.Security;

namespace Steven.Web.Areas.Admin.Controllers
{
    public class UserRoleController : AdminController
    {
        public IUserRoleRepository UserRoleRepository { get; set; }
        public ISysMenuRepository SysMenuRepository { get; set; }
        public IUserRoleSvc UserRoleSvc { get; set; }
        public IUserRole2MenuRepository UserRole2MenuRepository { get; set; }
        public ISysMenuSvc SysMenuSvc { get; set; }

        // GET: Admin/User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetList(string keyWord)
        {
            var search = GetSearchModel();
            var list = UserRoleRepository.GetList(keyWord, search);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(long id, string reUrl)
        {
            ViewBag.ReUrl = reUrl ?? Url.Action("Index");
            var model = new UserRoleModel();
            if (id != 0)
            {
                var role = UserRoleRepository.Get(id);
                if (role == null)
                {
                    ShowErrorMsg();
                    return Redirect(ViewBag.ReUrl);
                }
                Mapper.Map(role, model);
                var lstMenuId = UserRole2MenuRepository.GetLstMenuId(role.Id);
                model.MenuIds = JsonConvert.SerializeObject(lstMenuId);

            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserRoleModel model)
        {
            var result = new JsonModel();
            var opType = OperationType.Insert;
            UserRole role = null;
            if (model.Id > 0)
            {
                role = UserRoleRepository.Get(model.Id);
                if (role == null)
                {
                    result.msg = $"找不到id为{0}的角色";
                    return Json(result);
                }
                opType = OperationType.Update;
            }
            else
            {
                role = new UserRole();
            }
            Mapper.Map(model, role);
            UserRoleSvc.SaveList(role, model.MenuIds);
            LogRepository.Insert(TableSource.UserRole, opType, role.Id);
            result.code = JsonModelCode.Succ;
            ShowSuccMsg("保存成功！");
            return Json(result);
        }

        public ActionResult SetButtons(long id,string reUrl)
        {
            ViewBag.ReUrl = reUrl ?? Url.Action("Index");
            ViewBag.RoleId = id;
            var model = SysMenuSvc.GetRole2MenuList(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult SaveButtons(long id, string btnIds)
        {
            var result = new JsonModel();
            if (string.IsNullOrEmpty(btnIds))
            {
                result.msg = "请选择按钮！";
                return Json(result);
            }
            UserRole2MenuRepository.SaveRole2MenuButtons(id, btnIds);
            UserRoleSvc.ClearRoleUserCache(id);
            result.msg = "保存成功！";
            result.code = JsonModelCode.Succ;
            return Json(result);
        }
    }
}
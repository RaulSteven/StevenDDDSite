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
        public IUserRole2FilterRepository UserRole2FilterRepository { get; set; }

        #region 角色列表、编辑
        // GET: Admin/User
        [ValidatePage]
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

        [ValidateButton(ActionName = "Index", Buttons = SysButton.Edit)]
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
        [ValidateButton(ActionName = "Index", Buttons = SysButton.Edit)]
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
        #endregion


        #region 按钮设置

        [ValidateButton(ActionName = "Index", Buttons = SysButton.Grant)]
        public ActionResult SetButtons(long id, string reUrl)
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

        #endregion

        #region 数据权限

        /// <summary>
        /// 设置角色的数据权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [ValidateButton(Buttons = SysButton.Grant, ActionName = "Index")]
        public ActionResult SetFilterRule(long roleId)
        {
            //Source、FilterGroup
            var list = UserRoleSvc.GetFilterList(roleId);
            ViewBag.roleId = roleId;
            return PartialView(list);
        }
        [ValidateButton(Buttons = SysButton.Grant, ActionName = "Index")]
        public ActionResult DeleteFilterRule(long id)
        {
            var result = new JsonModel();
            if (id < 1)
            {
                result.msg = "请选择要删除的数据源";
                return Json(result);
            }
            var model = UserRole2FilterRepository.Get(id);
            if (model == null)
            {
                return Json(result);
            }
            UserRole2FilterRepository.Delete(model);
            UserRoleSvc.ClearRoleUserCache(model.RoleId);
            LogRepository.Insert(TableSource.UserRole2Filter, OperationType.Delete,id);
            result.code = JsonModelCode.Succ;
            result.msg = "删除成功！";
            return Json(result);
        }

        [HttpPost]
        [ValidateButton(Buttons = SysButton.Grant, ActionName = "Index")]
        public ActionResult SetFilterRule(long id, string filterGroup, string name, string source, long roleId)
        {
            var result = new JsonModel();
            var exist = UserRole2FilterRepository.ExistSource(id, source);
            if (exist)
            {
                result.msg = $"已存在资源为{source}的角色数据规则！";
                return Json(result);
            }
            UserRole2Filter userFilter = null;
            if (id > 0)
            {
                userFilter = UserRole2FilterRepository.Get(id);
                if (userFilter == null)
                {
                    result.msg = $"找不到Id为{id}的角色数据规则关联数据！";
                    return Json(result);
                }
            }
            else
            {
                userFilter = new UserRole2Filter();
            }
            userFilter.RoleId = roleId;
            userFilter.Name = name;
            userFilter.Source = source;
            userFilter.FilterGroups = filterGroup;
            UserRole2FilterRepository.Save(userFilter);
            UserRoleSvc.ClearRoleUserCache(userFilter.RoleId);
            result.msg = "保存成功！";
            result.code = JsonModelCode.Succ;
            var propModelList = GetPropertyList(source);
            userFilter.SourceProperties = propModelList;
            result.data = userFilter;
            return Json(result);
        }

        [HttpPost]
        [ValidateButton(Buttons = SysButton.Grant, ActionName = "Index")]
        public ActionResult GetCurrResources(FilterCurrent curr)
        {
            var list = UserRoleSvc.GetResList(curr);
            return Json(list);
        }

        [HttpPost]
        public ActionResult GetSource(string source)
        {
            var propModelList = GetPropertyList(source);
            return Json(propModelList);
        }

        private List<PropertyModel> GetPropertyList(string source)
        {
            var type = Assembly.Load("BeiLin.Domain").GetType("BeiLin.Domain.Models." + source);
            if (type == null)
            {
                return null;
            }
            var propModelList = new List<PropertyModel>();
            var propertyInfoList = type.GetProperties();
            foreach (var property in propertyInfoList)
            {
                var proModel = new PropertyModel()
                {
                    Name = property.Name,
                    TypeName = property.PropertyType.IsEnum ? "Enum" : property.PropertyType.Name,
                };
                var nameAttr = property.GetCustomAttribute(typeof(DisplayNameAttribute));
                if (nameAttr == null)
                {
                    proModel.DisplayName = property.Name;
                }
                else
                {
                    proModel.DisplayName = ((DisplayNameAttribute)nameAttr).DisplayName;
                }
                proModel.DisplayName = $"{proModel.DisplayName}({proModel.TypeName})";
                propModelList.Add(proModel);
            }
            foreach (var curr in FilterCurrent.CurrentDeptId.GetSList())
            {
                propModelList.Add(new PropertyModel()
                {
                    Name = curr.Value,
                    DisplayName = curr.Text,
                    TypeName = "Enum"
                });
            }
            return propModelList;
        }

        #endregion
    }
}
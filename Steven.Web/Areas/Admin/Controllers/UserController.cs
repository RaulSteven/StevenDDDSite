using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Steven.Web.Framework.Controllers;
using Steven.Domain.Models;
using Steven.Domain.Repositories;
using Steven.Domain.Enums;
using System.Threading.Tasks;
using AutoMapper;
using Steven.Web.Areas.Admin.Models;
using Steven.Domain.ViewModels;
using Steven.Core.Utilities;
using Steven.Web.Framework.Security;

namespace Steven.Web.Areas.Admin.Controllers
{
    public class UserController : AdminController
    {
        public IUsersRepository UsersRepository { get; set; }
        public IUserRole2MenuRepository UserRole2MenuRepository { get; set; }
        public IUser2RoleRepository User2RoleRepository { get; set; }
        public IUserRoleRepository UserRoleRepository { get; set; }
        public IUser2ApartmentRepository User2ApartRepository { get; set; }

        // GET: Admin/User
        [ValidatePage]
        public ActionResult Index()
        {
            var model = new UserIndexModel()
            {
                UserRoleList = UserRoleRepository.GetList()
            };
            return View(model);
        }

        public ActionResult GetList(string keyWord, UserGroup group)
        {
            var search = GetSearchModel();
            var list =  UsersRepository.GetPager(group, keyWord, search);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [ValidateButton(ActionName ="Index",Buttons = SysButton.Edit)]
        public ActionResult Edit(long id, string reUrl)
        {
            ViewBag.ReUrl = reUrl ?? Url.Action("Index");
            var model = new AdminUserModel();
            if (id != 0)
            {
                var user =  UsersRepository.Get(id);
                if (user == null)
                {
                    ShowErrorMsg();
                    return Redirect(ViewBag.ReUrl);
                }
                Mapper.Map(user,model);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateButton(ActionName ="Index",Buttons = SysButton.Edit)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AdminUserModel model)
        {
            var result = new JsonModel();
            #region check params
            var existLoginName =  UsersRepository.ExistLoginName(model.Id, model.LoginName);
            if (existLoginName)
            {
                result.msg = "登录名已存在";
                return Json(result);
            }
            #endregion

            var opType = OperationType.Insert;
            Users user = null;
            if (model.Id > 0)
            {
                user =  UsersRepository.Get(model.Id);
                if (user == null)
                {
                    result.msg = $"找不到id为{0}的用户";
                    return Json(result);
                }
                opType = OperationType.Update;
            }
            else
            {
                user = new Users();
            }
            Mapper.Map(model, user);
            if (!string.IsNullOrEmpty(model.Password))
            {
                user.PasswordSalt = HashUtils.GenerateSalt();
                user.Password = HashUtils.HashPasswordWithSalt(model.Password, user.PasswordSalt);
            }
             UsersRepository.Save(user);
             LogRepository.Insert(TableSource.Users, opType, user.Id);
            result.code = JsonModelCode.Succ;
            ShowSuccMsg("保存成功！");
            return Json(result);
        }

        #region 设置角色

        [HttpPost]
        public ActionResult SetRole(SetRoleModel model)
        {
            var result = new JsonModel();
            User2RoleRepository.SaveList(model.UserIds, model.RoleIds);
            result.msg = "保存成功！";
            result.code = JsonModelCode.Succ;
            return Json(result);
        }
        #endregion

        #region 设置部门

        [HttpPost]
        public ActionResult SetApart(string userIds, string apartIds)
        {
            var result = new JsonModel();
            User2ApartRepository.SaveList(userIds, apartIds);
            result.msg = "保存成功！";
            result.code = JsonModelCode.Succ;
            return Json(result);
        }
        #endregion
    }
}
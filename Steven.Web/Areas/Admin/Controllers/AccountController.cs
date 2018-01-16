using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Steven.Web.Framework.Controllers;
using Steven.Domain.Models;
using Steven.Domain.Repositories;
using Steven.Domain.Services;
using Steven.Domain.Enums;
using System.Threading.Tasks;
using Steven.Web.Framework.Extensions;
using Steven.Core.Utilities;
using Steven.Core.Extensions;
using Steven.Domain.ViewModels;
using Steven.Web.Areas.Admin.Models;

namespace Steven.Web.Areas.Admin.Controllers
{
    public class AccountController : WebSiteController
    {
        public IUsersRepository UsersRepository { get; set; }
        public IFormsAuthenticationSvc FormsAuthSvc { get; set; }
        public ISysOperationLogRepository SysOperationLogRepository { get; set; }
        public ActionResult NoPermission()
        {
            return View();
        }

        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect(Url.AdminHome());
            }
            var model = new LoginModel();
            model.UserName = CookieUtils.GetCookie(FormsAuthSvc.GetUserNameCookieKey(), "");
            if (!string.IsNullOrEmpty(model.UserName))
            {
                model.IsRemember = true;
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            var result = new JsonModel();
            #region check params
            if (model == null)
            {
                result.msg = "请输入数据！";
                return Json(result);
            }
            if (string.IsNullOrEmpty(model.UserName))
            {
                result.msg = "请输入用户名";
                return Json(result);
            }
            if (string.IsNullOrEmpty(model.Password)|| model.Password.Length < 6)
            {
                result.msg = "请输入正确的密码";
                return Json(result);
            }
            if (string.IsNullOrEmpty(model.ValidateCode))
            {
                result.msg = "请输入验证码";
                return Json(result);
            }
            #endregion

            if (!VeryfyCodeUtility.IsVerifyCodeMatch(Session, model.ValidateCode))
            {
                result.msg = "验证码错误！";
                return Json(result);
            }

            var loginResult = UsersRepository.AdminLogin(model.UserName, model.Password);
            if (loginResult.Status == SigninStatus.Succ)
            {
                FormsAuthSvc.CreateAuthenticationTicket(loginResult.UserInfo, Response, HttpContext, model.IsRemember);
                //添加到Fom
                if (model.IsRemember)
                {
                    CookieUtils.SetCookie(FormsAuthSvc.GetUserNameCookieKey(), model.UserName, true);
                }
                else
                {
                    CookieUtils.RemoveCookie(FormsAuthSvc.GetUserNameCookieKey());
                }
                //添加登录日志
                 SysOperationLogRepository.Insert(TableSource.Users, OperationType.UserLogin, loginResult.UserInfo.Id);
                result.code = JsonModelCode.Succ;
                return Json(result);
            }
            result.msg = "登录失败！" + loginResult.Status.GetDescriotion();
            return Json(result);
        }

        public ActionResult Logout()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect(Url.Action("Login"));
            }
            FormsAuthSvc.LogOut(User);
            return Redirect(Url.Action("Login"));
        }
    }
}
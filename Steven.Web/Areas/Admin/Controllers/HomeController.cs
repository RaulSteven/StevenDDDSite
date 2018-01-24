using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Steven.Web.Framework.Controllers;
using Steven.Domain.Enums;
using System.Threading.Tasks;
using Steven.Domain.Repositories;
using Steven.Web.Areas.Admin.Models;
using AutoMapper;
using Steven.Core.Utilities;
using Newtonsoft.Json;
using Steven.Domain.ViewModels;
using Steven.Domain.Models;
using Steven.Core.Cache;
using Steven.Web.Framework.Security;

namespace Steven.Web.Areas.Admin.Controllers
{
    public class HomeController : AdminController
    {
        public ISysConfigRepository SysConfigRepository { get; set; }
        public IUsersRepository UsersRepository { get; set; }
        public ICacheManager Cache { get; set; }
        // GET: Admin/Home
        [ValidatePage]
        public ActionResult Index()
        {
            var model = new HomeDataModel();
            model.TotalNewOrderPercent = PercentConvert(model.TotalNewOrderCount, model.TotalOrderCount);

            model.TotalUserCount = UsersRepository.GetCount();
            model.TotalMonthUserCount = UsersRepository.GetCount(true);
            model.TotalMonthUserPercent = PercentConvert(model.TotalMonthUserCount, model.TotalUserCount);
            return View(model);
        }

        public ActionResult _HomeData(AdminHomeDataType t = AdminHomeDataType.Today)
        {
            var model = new HomeStatisticsDataModel();
            return View(model);
        }



        private string PercentConvert(int n, int total)
        {
            return ((float)n / (float)total * 100).ToString("0.00 ") + "% ";
        }
        private float PercentInt(int n, int total)
        {
            return float.Parse(((float)n / (float)total * 100).ToString("0.00 "));
        }
        public ActionResult SkinConfig()
        {
            return PartialView();
        }

        [ValidatePage]
        public ActionResult SysConfigList()
        {
            return View();
        }

        public ActionResult GetSysConfigList(string keyword, SysConfigClassify? clz)
        {
            var search = GetSearchModel();
            var list = SysConfigRepository.GetPager(keyword, clz, search);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [ValidateButton(ActionName = "GetSysConfigList", Buttons = SysButton.Edit)]
        public ActionResult SysConfigEdit(long id, string reUrl)
        {
            ViewBag.ReUrl = reUrl ?? Url.Action("SysConfigList");
            var model = new SysConfigModel();
            if (id == 0)
            {
                return View(model);
            }
            var config = SysConfigRepository.Get(id);
            if (config == null)
            {
                ShowErrorMsg();
                return Redirect(ViewBag.ReUrl);
            }
            Mapper.Map(config, model);
            switch (model.ConfigType)
            {
                case SysConfigType.String:
                    model.StringValue = JsonConvert.DeserializeObject<string>(model.ConValue);
                    break;
                case SysConfigType.Bool:
                    model.BoolValue = JsonConvert.DeserializeObject<bool>(model.ConValue);
                    break;
                case SysConfigType.TextArea:
                    model.TextAreaValue = JsonConvert.DeserializeObject<string>(model.ConValue);
                    break;
                case SysConfigType.Int:
                    model.IntValue = JsonConvert.DeserializeObject<int>(model.ConValue);
                    break;
                case SysConfigType.Long:
                    model.LongValue = JsonConvert.DeserializeObject<long>(model.ConValue);
                    break;
                case SysConfigType.StringArray:
                    model.StringArrayValue = string.Join(",", JsonConvert.DeserializeObject<string[]>(model.ConValue));
                    break;
                default:
                    break;
            }
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateButton(ActionName = "GetSysConfigList", Buttons = SysButton.Edit)]
        [ValidateAntiForgeryToken]
        public ActionResult SysConfigEdit(SysConfigModel model)
        {
            var result = new JsonModel();
            var opType = OperationType.Update;
            SysConfig config = null;
            if (model.Id == 0)
            {
                config = new SysConfig();
                opType = OperationType.Insert;
            }
            else
            {
                config = SysConfigRepository.Get(model.Id);
                if (config == null)
                {
                    result.msg = "记录不存在！";
                    return Json(result);
                }
            }
            Mapper.Map(model, config);
            switch (config.ConfigType)
            {
                case SysConfigType.String:
                    config.ConValue = JsonConvert.SerializeObject(model.StringValue);
                    break;
                case SysConfigType.Bool:
                    config.ConValue = JsonConvert.SerializeObject(model.BoolValue);
                    break;
                case SysConfigType.TextArea:
                    config.ConValue = JsonConvert.SerializeObject(model.TextAreaValue);
                    break;
                case SysConfigType.Int:
                    config.ConValue = JsonConvert.SerializeObject(model.IntValue);
                    break;
                case SysConfigType.Long:
                    config.ConValue = JsonConvert.SerializeObject(model.LongValue);
                    break;
                case SysConfigType.StringArray:
                    config.ConValue = JsonConvert.SerializeObject(model.StringArrayValue.Split(','));
                    break;
                default:
                    break;
            }
            SysConfigRepository.Save(config);
            LogRepository.Insert(TableSource.SysConfig, opType, config.Id);
            result.code = JsonModelCode.Succ;
            result.msg = "保存成功！";
            return Json(result);
        }


        public ActionResult ClearCache()
        {
            var reUrl = Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : Url.Action("Index");
            Cache.Clear();
            ShowSuccMsg("清除缓存成功！");
            return Redirect(reUrl);
        }

        [ValidatePage]
        public ActionResult UserProfile()
        {
            var user = UsersRepository.Get(User.UserModel.UserId);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModifyPwd(string password,string newPassword)
        {
            var result = new JsonModel();
            #region check params
            if (string.IsNullOrEmpty(password))
            {
                result.msg = "旧密码不能为空!";
                return Json(result);
            }

            if (string.IsNullOrEmpty(newPassword))
            {
                result.msg = "新密码不能为空!";
                return Json(result);
            }
            #endregion

            var user = UsersRepository.Get(User.UserModel.UserId);
            if (!user.Password.Equals(HashUtils.HashPasswordWithSalt(password, user.PasswordSalt)))
            {
                result.msg = "旧密码不正确！";
                return Json(result);
            }
            user.Password = HashUtils.HashPasswordWithSalt(newPassword, user.PasswordSalt);
            UsersRepository.Save(user);
            LogRepository.Insert(TableSource.Users, OperationType.Update, user.Id);
            result.code = JsonModelCode.Succ;
            ShowSuccMsg("修改密码成功!");
            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModifyInfo(AdminUserModel model)
        {
            var result = new JsonModel();
            #region check params
            var existLoginName = UsersRepository.ExistLoginName(model.Id, model.LoginName);
            if (existLoginName)
            {
                result.msg = "登录名已存在";
                return Json(result);
            }
            #endregion

            var opType = OperationType.Update;
            Users user = UsersRepository.Get(model.Id);
            if (user == null)
            {
                result.msg = $"找不到id为{0}的用户";
                return Json(result);
            }
           
            Mapper.Map(model, user);
            UsersRepository.Save(user);
            LogRepository.Insert(TableSource.Users, opType, user.Id);
            result.code = JsonModelCode.Succ;
            ShowSuccMsg("保存成功！");
            return Json(result);
        }
    }
}
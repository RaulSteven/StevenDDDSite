using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Steven.Domain.Repositories;
using AutoMapper;
using Steven.Domain.Models;
using Steven.Web.Framework.Controllers;
using Steven.Domain.Enums;
using Steven.Domain.ViewModels;
using Steven.Core.Utilities;
using Steven.Domain.Services;

namespace Steven.Web.Areas.Admin.Controllers
{
    public class AgentController : AdminController
    {
        public IUsersRepository UsersRepository { get; set; }
        public IAgentRepository AgentRepository { get; set; }
        public IAgentSvc AgentSvc { get; set; }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetList(string keyWord)
        {
            var search = GetSearchModel();
            var list = AgentRepository.GetPager(keyWord, search);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(long id, string reUrl)
        {
            ViewBag.ReUrl = reUrl ?? Url.Action("Index");
            var model = new AgentModel();
            if (id != 0)
            {
                var agent = AgentRepository.GetIncludeUser(id);
                if (agent == null)
                {
                    ShowErrorMsg();
                    return Redirect(ViewBag.ReUrl);
                }
                Mapper.Map(agent, model);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AgentModel model)
        {
            var result = new JsonModel();
            #region check params
            var existLoginName = UsersRepository.ExistLoginName(model.UserId, model.LoginName);
            if (existLoginName)
            {
                result.msg = "登录名已存在";
                return Json(result);
            }
            #endregion
            result = AgentSvc.Save(model);
            if (result.code == JsonModelCode.Succ)
            {
                ShowSuccMsg("保存成功！");
            }
            return Json(result);
        }

        [HttpPost]
        public ActionResult BatchDele(string ids)
        {
            var result = new JsonModel();
            var dele = AgentRepository.BatchDele(ids);
            if (dele > 0)
            {
                result.msg = "删除成功！";
                result.code = JsonModelCode.Succ;
                LogRepository.Insert(TableSource.Agent, OperationType.Delete, ids);
            }
            else
            {
                result.msg = "删除失败！";
            }
            return Json(result);

        }
    }
}
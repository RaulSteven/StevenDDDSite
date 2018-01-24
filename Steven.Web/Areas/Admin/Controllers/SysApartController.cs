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
using Steven.Domain.Services;
using Steven.Web.Framework.Security;

namespace Steven.Web.Areas.Admin.Controllers
{
    public class SysApartController : AdminController
    {
        public ISysApartmentRepository SysApartRepository { get; set; }
        public IUserRoleRepository UserRoleRepository { get; set; }
        public IUserRole2ApartmentRepository UserRole2ApartRepository { get; set; }

        public ISysApartmenSvc SysApartmentSvc { get; set; }

        // GET: Admin/SysApart
        [ValidatePage]
        public ActionResult Index()
        {
            var lstRole = UserRoleRepository.GetList();
            var selectList = new SelectList(lstRole, "Id", "Name");
            ViewBag.RoleSList = selectList;
            return View();
        }

        public ActionResult GetJsonList()
        {
            var list =  SysApartRepository.GetJsonList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(SysApartModel model)
        {
            var result = new JsonModel();
            SysApartment apart = null;
            var opType = OperationType.Insert;
            if (model.Id > 0)
            {
                opType = OperationType.Update;
                apart =  SysApartRepository.Get(model.Id);
                if (apart == null)
                {
                    result.msg = "找不到记录！";
                    return Json(result);
                }
            }
            else
            {
                apart = new SysApartment();
            }
            Mapper.Map(model, apart);
            SysApartmentSvc.Save(apart, model.RoleIds);
             LogRepository.Insert(TableSource.SysApartment, opType, apart.Id);
            result.data = apart;
            result.code = JsonModelCode.Succ;
            result.msg = "保存成功！";
            return Json(result);
        }

        public ActionResult GetApart(long id)
        {
            var result = new JsonModel();
            var apart =  SysApartRepository.Get(id);
            if (apart == null)
            {
                result.msg = "找不到记录！";
                return Json(result);
            }
            apart.LstRoleIds = UserRole2ApartRepository.GetLstRoleId(apart.Id);
            result.data = apart;
            result.code = JsonModelCode.Succ;
            return Json(result,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(long id)
        {
            var result = new JsonModel();
            var deleIds =  SysApartRepository.Delete(id);
             LogRepository.Insert(TableSource.SysApartment, OperationType.Delete, deleIds);
            result.msg = "删除成功！";
            result.code = JsonModelCode.Succ;
            return Json(result);
        }
    }
}
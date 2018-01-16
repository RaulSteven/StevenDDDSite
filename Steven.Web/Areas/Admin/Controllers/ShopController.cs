using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Steven.Web.Framework.Controllers;
using Steven.Domain.Models;
using Steven.Domain.Repositories;
using Steven.Domain.ViewModels;
using Steven.Domain.Enums;
using Steven.Domain.Services;
using AutoMapper;
using Steven.Web.Areas.Admin.Models;
using Newtonsoft.Json;

namespace Steven.Web.Areas.Admin.Controllers
{
    public class ShopController : AdminController
    {
        public IShopRepository ShopRepository { get; set; }
        public IUsersRepository UsersRepository { get; set; }
        public IAgentRepository AgentRepository { get; set; }
        public IShopAppInfoRepository ShopAppInfoRepository { get; set; }
        public IProductClassifyRepository ProductClassifyRepository { get; set; }
        public IAttachmentSvc AttachmentSvc { get; set; }
        public IShopSvc ShopSvc { get; set; }
        public IShopTemplateRepository ShopTemplateRepository { get; set; }
        

        #region Index
        // GET: Admin/Shop
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetList(string keyWord, long? agentId)
        {
            var search = GetSearchModel();
            var list = ShopRepository.GetPager(keyWord, agentId, search);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region  GetAgentList

        public ActionResult GetAgentList()
        {
            var lst = AgentRepository.GetList();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Edit

        public ActionResult Edit(long id, string reUrl)
        {
            ViewBag.ReUrl = reUrl ?? Url.Action("Index");
            var model = new ShopModel();
            if (id != 0)
            {
                var shop = ShopRepository.GetIncludeUser(id);
                if (shop == null)
                {
                    ShowErrorMsg();
                    return Redirect(ViewBag.ReUrl);
                }
                Mapper.Map(shop, model);

                if (shop.AgentId != 9)
                {
                    var agent = AgentRepository.Get(shop.AgentId);
                    if (agent != null)
                    {
                        model.AgentName = agent.Name;
                    }
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ShopModel model)
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
            result = ShopSvc.Save(model);
            if (result.code == JsonModelCode.Succ)
            {
                ShowSuccMsg("保存成功！");
            }
            return Json(result);
        }

        #endregion

        #region batchDele
        [HttpPost]
        public ActionResult BatchDele(string ids)
        {
            var result = new JsonModel();
            var dele = ShopRepository.BatchDele(ids);
            if (dele > 0)
            {
                result.msg = "删除成功！";
                result.code = JsonModelCode.Succ;
                LogRepository.Insert(TableSource.Shop, OperationType.Delete, ids);
            }
            else
            {
                result.msg = "删除失败！";
            }
            return Json(result);

        }
        #endregion

        #region APP Info 
        public ActionResult AppInfoEdit(long shopId, string reUrl)
        {
            ViewBag.ReUrl = reUrl ?? Url.Action("Index");
            var model = new ShopAppInfoModel();
            var appInfo = ShopAppInfoRepository.GetByShopId(shopId);
            if (appInfo == null)
            {
                ShowErrorMsg();
                return Redirect(ViewBag.ReUrl);
            }
            Mapper.Map(appInfo, model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AppInfoEdit(ShopAppInfoModel model)
        {
            var result = new JsonModel();
            var appInfo = ShopAppInfoRepository.Get(model.Id);
            if (appInfo == null)
            {
                result.msg = "记录不存在！";
                return Json(result);
            }
            Mapper.Map(model, appInfo);
            ShopAppInfoRepository.Save(appInfo);
            LogRepository.Insert(TableSource.ShopAppInfo, OperationType.Update, appInfo.Id);
            result.code = JsonModelCode.Succ;
            ShowSuccMsg("保存成功！");
            return Json(result);
        }

        #endregion

        #region ProductClassify
        public ActionResult ProductClassifyList(long shopId, string reUrl)
        {
            ViewBag.ReUrl = reUrl ?? Url.Action("Index");
            var shop = ShopRepository.Get(shopId);
            if (shop == null)
            {
                ShowErrorMsg();
                return Redirect(ViewBag.ReUrl);
            }
            ViewBag.Shop = shop;
            return View();
        }

        public ActionResult GetProductClassifyList(long shopId)
        {
            var search = GetSearchModel();
            var list = ProductClassifyRepository.GetPager(shopId, search);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetProductClassify(long id)
        {
            var proClz = ProductClassifyRepository.Get(id);
            var model = Mapper.Map<ProductClassify, ProductClassifyModel>(proClz);
            if (model.IconAttaId != 0)
            {
                model.IconUrl = AttachmentSvc.GetPicUrl(model.IconAttaId, 100, 100);
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProductClassifyEdit(ProductClassifyModel model)
        {
            var result = new JsonModel();
            var shop = ShopRepository.Get(model.ShopId);
            if (shop == null)
            {
                result.msg = $"找不到id为{model.ShopId}的商铺！";
                return Json(result);
            }
            int limitNum = 0, num = 0;
            ProductClassify productClz = null;
            var opType = OperationType.Insert;
            if (model.Id == 0)
            {
                if (!ShopSvc.IsLimitShopCls(model.ShopId, ref limitNum, ref num))
                {
                    result.msg = $"您已上传了{num}个分组，不能超过限制数{limitNum}";
                    return Json(result);
                }
                productClz = new ProductClassify();
            }
            else
            {
                opType = OperationType.Update;
                productClz = ProductClassifyRepository.Get(model.Id);
                if (productClz == null)
                {
                    result.msg = $"找不到id为{model.Id}的商品分类！";
                    return Json(result);
                }
            }
            Mapper.Map(model, productClz);
            ProductClassifyRepository.Save(productClz);
            LogRepository.Insert(TableSource.ProductClassify, opType, productClz.Id);
            result.code = JsonModelCode.Succ;
            result.msg = "保存成功！" + (model.Id == 0 ? $"您还能上传{limitNum - num}个分组" : "");
            return Json(result);
        }

        [HttpPost]
        public ActionResult ProductClassifyBatchDele(string ids)
        {
            var result = ProductClassifyRepository.BatchDele(ids);
            if (result.code == JsonModelCode.Succ)
            {
                LogRepository.Insert(TableSource.ProductClassify, OperationType.Delete, ids);
            }
            return Json(result);

        }
        #endregion


        #region Template

        public ActionResult Template(long shopId, string reUrl)
        {
            ViewBag.ReUrl = reUrl ?? Url.Action("Index");
            ViewBag.shopId = shopId;
            var shop = ShopRepository.GetIncludeUser(shopId);
            if (shop == null)
            {
                ShowErrorMsg();
                return Redirect(ViewBag.ReUrl);
            }
            var list = ShopTemplateRepository.GetList(shopId);
            return View(list);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTemplate(EditTemplateModel model)
        {
            ViewBag.shopId = model.ShopId;
            var result = ShopTemplateRepository.Save(model.TemplateIds, model.ShopId);
            if (result.code == JsonModelCode.Succ)
            {
                ShowSuccMsg("保存成功！");
            }
            return Json(result);
        }

        
        #endregion
    }
}
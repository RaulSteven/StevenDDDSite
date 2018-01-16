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
using Steven.Domain.ViewModels;
using AutoMapper;
using Steven.Web.Areas.Admin.Models;
using Newtonsoft.Json;

namespace Steven.Web.Areas.Admin.Controllers
{
    public class ProductController : AdminController
    {
        public IProductRepository ProductRepository { get; set; }
        public IShopRepository ShopRepository { get; set; }
        public IProductSpecsRepository ProductSpecsRepository { get; set; }
        public ISysSpecsRepository SysSpecesRepository { get; set; }
        public IProductClassifyRepository ProductClassifyRepository { get; set; }

        public ISysSpecsRepository SysSpecsRepository { get; set; }

        public IProductSvc ProductSvc { get; set; }
        public IShopSvc ShopSvc { get; set; }
        public ISysUnitRepository SysUnitRepository { get; set; }

        #region 商品列表、编辑、删除
        //public 
        // GET: Admin/Product
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetList(string keyword, long? shopId, ProductStatus? status)
        {
            var search = GetSearchModel();
            var list = ProductRepository.GetPager(keyword, shopId, status, search);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetShopList()
        {
            var lst = ShopRepository.GetList();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetProductClassifyList(long shopId)
        {
            var lst = ProductClassifyRepository.GetList(shopId);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSysSpecsList()
        {
            var lst = SysSpecesRepository.GetLstName();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(long id, string reUrl)
        {
            ViewBag.ReUrl = reUrl ?? Url.Action("Index");

            var lst = SysSpecesRepository.GetLstName();
            var lstUnit = SysUnitRepository.GetLstName();
            var model = new ProductModel()
            {
                LstSysSpecs = JsonConvert.SerializeObject(lst),
                LstUnit = JsonConvert.SerializeObject(lstUnit),
            };
            if (id == 0)
            {
                model.Status = ProductStatus.OnSale;
                return View(model);
            }

            var product = ProductRepository.Get(id);
            if (product == null)
            {
                ShowErrorMsg();
                return Redirect(ViewBag.ReUrl);
            }
            Mapper.Map(product, model);
            model.LstSpecs = ProductSpecsRepository.GetList(product.Id);
            if (model.ProductClassifyId != 0)
            {
                var cls = ProductClassifyRepository.Get(model.ProductClassifyId);
                if (cls!= null)
                {
                    model.ProductClassifyName = cls.Name;
                }
            }
            if (model.ShopId!= 0)
            {
                var shop = ShopRepository.Get(model.ShopId);
                if (shop != null)
                {
                    model.ShopName = shop.Name;
                }
            }
            return View(model);
        }


        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductModel model)
        {
            var result = ProductSvc.Save(model);
            if (result.code == JsonModelCode.Succ)
            {
                if (model.Id <= 0)
                {
                    ShowSuccMsg($"保存成功！您还能上传{result.data}个商品");
                }
                else
                {
                    ShowSuccMsg("保存成功！");
                }
            }
            return Json(result);
        }
        #endregion

        #region 规格
        public ActionResult SpecsList()
        {
            return View();
        }

        public ActionResult GetSpecsList(string name)
        {
            var search = GetSearchModel();
            var list = SysSpecesRepository.GetPager(name, search);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSpecs(long id)
        {
            var result = new JsonModel();
            if (id == 0)
            {
                result.msg = $"找不到id为{id}的记录";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            var specs = SysSpecesRepository.Get(id);
            if (specs == null)
            {
                result.msg = $"找不到id为{id}的记录";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            result.data = specs;
            result.code = JsonModelCode.Succ;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveSpecs(SysSpecsModel model)
        {
            var result = new JsonModel();

            SysSpecs specs = null;
            OperationType opType = OperationType.Insert;
            if (model.Id == 0)
            {
                specs = new SysSpecs();
            }
            else
            {
                specs = SysSpecesRepository.Get(model.Id);
                if (specs == null)
                {
                    result.msg = $"找不到id为{model.Id}的记录";
                    return Json(result);
                }
            }
            Mapper.Map(model, specs);
            SysSpecesRepository.Save(specs);
            LogRepository.Insert(TableSource.SysSpecs, opType, specs.Id);
            result.code = JsonModelCode.Succ;
            result.msg = "保存成功！";
            return Json(result);
        }
        #endregion

        #region 单位
        public ActionResult UnitList()
        {
            return View();
        }

        public ActionResult GetUnitList(string name)
        {
            var search = GetSearchModel();
            var list = SysUnitRepository.GetPager(name, search);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUnit(long id)
        {
            var result = new JsonModel();
            if (id == 0)
            {
                result.msg = $"找不到id为{id}的记录";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            var m = SysUnitRepository.Get(id);
            if (m == null)
            {
                result.msg = $"找不到id为{id}的记录";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            result.data = m;
            result.code = JsonModelCode.Succ;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveUnit(SysUnitModel model)
        {
            var result = new JsonModel();

            SysUnit oModel = null;
            OperationType opType = OperationType.Insert;
            if (model.Id == 0)
            {
                oModel = new SysUnit();
            }
            else
            {
                oModel = SysUnitRepository.Get(model.Id);
                if (oModel == null)
                {
                    result.msg = $"找不到id为{model.Id}的记录";
                    return Json(result);
                }
            }
            Mapper.Map(model, oModel);
            SysUnitRepository.Save(oModel);
            LogRepository.Insert(TableSource.SysUnit, opType, oModel.Id);
            result.code = JsonModelCode.Succ;
            result.msg = "保存成功！";
            return Json(result);
        }
        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Steven.Web.Framework.Controllers;
using Steven.Domain.ViewModels;
using Steven.Domain.Repositories;
using Steven.Domain.Services;
using Steven.Domain.Enums;
using AutoMapper;
using Steven.Domain.Models;
using Steven.Web.Areas.Admin.Models;
using Steven.Web.Areas.Shop.Models;
using Steven.Core.Utilities;
using Steven.Domain.Infrastructure;
using Newtonsoft.Json;

namespace Steven.Web.Areas.Shop.Controllers
{
    public class ProductController : ShopController
    {
        public IProductClassifyRepository ProductClassifyRepository { get; set; }
        public IAttachmentSvc AttachmentSvc { get; set; }
        public IProductRepository ProductRepository { get; set; }
        public IProductSvc ProductSvc { get; set; }
        public ISysSpecsRepository SysSpecesRepository { get; set; }
        public IProductSpecsRepository ProductSpecsRepository { get; set; }
        public ISysUnitRepository SysUnitRepository { get; set; }
        public IShopSvc ShopSvc { get; set; }

        #region 商品管理
        // GET: Shop/Product
        public ActionResult Index(long? clzId)
        {
            var model = new ProductIndexModel();
            model.LstClassify = ProductClassifyRepository.GetListByShopId(User.UserModel.ShopId);
            model.CurrClzId = clzId ?? 0;
            return View(model);
        }

        public ActionResult _ProductList(long? clzId)
        {
            var search = new PageSearchSortModel()
            {
                Sort = "Status asc, Sort desc, UpdateTime desc",
                Offset = StringUtility.ConvertToInt(Request.QueryString["offset"], 0),
                Limit = StringUtility.ConvertToInt(Request.QueryString["limit"], 10)
            };
            var lst = ProductRepository.GetPager(clzId, User.UserModel.ShopId, search);
            return PartialView(lst);
        }
        
        public ActionResult Edit(long id, string reUrl)
        {
            ViewBag.ReUrl = reUrl ?? Url.Action("Index");

            var lst = SysSpecesRepository.GetLstName();
            var lstUnit = SysUnitRepository.GetLstName();
            var model = new ProductModel()
            {
                LstSysSpecs = JsonConvert.SerializeObject(lst),
                LstClassify = ProductClassifyRepository.GetList(User.UserModel.ShopId),
                LstUnit = JsonConvert.SerializeObject(lstUnit)
            };
            if (id == 0)
            {
                int limitNum = 0, num = 0;
                if (!ShopSvc.IsLimitShopPro(User.UserModel.ShopId, ref limitNum, ref num))
                {
                    ShowErrorMsg($"您已上传了{num}个商品，不能超过限制数{limitNum}");
                    return Redirect(reUrl);
                }
                model.Status = ProductStatus.OnSale;
                if (model.LstClassify!=null&&model.LstClassify.Any())
                {
                    model.ProductClassifyId = model.LstClassify.First().id;
                }
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
                if (cls != null)
                {
                    model.ProductClassifyName = cls.Name;
                }
            }
            return View(model);
        }


        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductModel model)
        {
            model.ShopId = User.UserModel.ShopId;
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

        #region 商品批量操作


        [HttpPost]
        public ActionResult BatchStatus(string ids, ProductStatus status)
        {
            var result = new JsonModel();
            var idArr = StringUtility.ConvertToBigIntArray(ids, ',');
            if (idArr == null || idArr.Length == 0)
            {
                result.msg = "请选择需要上下架的商品！";
                return Json(result);
            }
            var count = ProductRepository.BatchStatus(idArr, status, User.UserModel.ShopId);
            LogRepository.Insert(TableSource.Product, OperationType.Update, ids);
            result.code = JsonModelCode.Succ;
            result.msg = "保存成功！";
            return Json(result);
        }

        [HttpPost]
        public ActionResult BatchDele(string ids)
        {
            var result = new JsonModel();
            var count = ProductRepository.BatchDele(ids);
            if (count == 0)
            {
                result.msg = "删除失败！请刷新页面";
                return Json(result);
            }
            LogRepository.Insert(TableSource.Product, OperationType.Delete, ids);
            result.msg = "删除成功！";
            result.code = JsonModelCode.Succ;
            return Json(result);
        }

        [HttpPost]
        public ActionResult BatchClassify(string ids, long clzId)
        {
            var result = new JsonModel();
            var idArr = StringUtility.ConvertToBigIntArray(ids, ',');
            if (idArr == null || idArr.Length == 0)
            {
                result.msg = "请选择需要修改的商品！";
                return Json(result);
            }
            var count = ProductRepository.BatchClassify(idArr, clzId, User.UserModel.ShopId);
            LogRepository.Insert(TableSource.Product, OperationType.Update, ids);
            result.code = JsonModelCode.Succ;
            result.msg = "保存成功！";
            return Json(result);
        }
        #endregion


        #region 分组管理
        public ActionResult ClassifyList(string op)
        {
            ViewBag.Op = op;
            return View();
        }

        public ActionResult _ClassifyList()
        {
            var list = ProductClassifyRepository.GetListByShopId(User.UserModel.ShopId);
            return PartialView(list);
        }

        public ActionResult GetClassify(long id)
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
        public ActionResult ClassifyEdit(ProductClassifyModel model)
        {
            var result = new JsonModel();
            int limitNum = 0, num = 0;
            ProductClassify productClz = null;
            var opType = OperationType.Insert;
            if (model.Id == 0)
            {
                if (!ShopSvc.IsLimitShopCls(User.UserModel.ShopId, ref limitNum, ref num))
                {
                    result.msg = $"您已上传了{num}个分组，不能超过限制数{limitNum}";
                    return Json(result);
                }
                productClz = new ProductClassify();
                num++;
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
            productClz.ShopId = User.UserModel.ShopId;
            ProductClassifyRepository.Save(productClz);
            LogRepository.Insert(TableSource.ProductClassify, opType, productClz.Id);
            result.code = JsonModelCode.Succ;
            result.msg = "保存成功！" + (model.Id == 0 ? $"您还能上传{limitNum-num}个分组" : "");
            return Json(result);
        }

        [HttpPost]
        public ActionResult ClassifyDele(long id)
        {
            var result = ProductClassifyRepository.Delete(id, User.UserModel.ShopId);
            if (result.code == JsonModelCode.Succ)
            {
                LogRepository.Insert(TableSource.ProductClassify, OperationType.Delete, id);
            }
            return Json(result);
        }
        #endregion

    }
}
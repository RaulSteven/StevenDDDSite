using Steven.Web.Framework.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Steven.Core.Extensions;
using Steven.Domain.Models;
using Steven.Domain.Repositories;
using Steven.Domain.Services;
using Steven.Web.Areas.Shop.Models;
using Steven.Domain.Enums;
using Steven.Domain.ViewModels;
using Newtonsoft.Json;
using Steven.Core.Utilities;

namespace Steven.Web.Areas.Shop.Controllers
{
    public class ShopFittingController : ShopController
    {
        public IShopRepository ShopRepository { get; set; }
        public IShopFittingRepository ShopFittingRepository { get; set; }
        public IProductRepository ProductRepository { get; set; }
        public IProductClassifyRepository ProductClassifyRepository { get; set; }
        public IAttachmentSvc AttachmentSvc { get; set; }


        // GET: Shop/ShopFitting
        public ActionResult Index()
        {
            var model = new ShopFittingModel();
            var shopId = User.UserModel.ShopId;
            model.Shop = ShopRepository.Get(shopId);

            #region 主推

            var mainFitting = ShopFittingRepository.Get(shopId, ShopFittingType.MainPush);
            if (mainFitting != null)
            {
                model.MainPushHasSelected = mainFitting.HasSelected;
                model.MainPushSubTitle = mainFitting.SubTitle;
                model.MainPushTitle = mainFitting.Title;
                model.MainPushProductIds = mainFitting.JsonData;
            }
            else
            {
                model.MainPushHasSelected = false;
                model.MainPushSubTitle = "每日好物、限时福利、精选指南";
                model.MainPushTitle = "今日主推";
            }


            #endregion

            #region 分组
            var classifyFitting = ShopFittingRepository.Get(shopId, ShopFittingType.ProductClassify);
            if (classifyFitting != null)
            {
                model.ClassifyHasSelected = classifyFitting.HasSelected;
                model.ClassifyTitle = classifyFitting.Title;
            }
            else
            {
                model.ClassifyHasSelected = false;
                model.ClassifyTitle = "懂吃、会选、有格调";
            }
            model.LstClassify = ProductClassifyRepository.GetHomeClassify(shopId);
            #endregion

            #region 热卖
            var hotFitting = ShopFittingRepository.Get(shopId, ShopFittingType.HotSale);
            if (hotFitting != null)
            {
                model.HotSaleHasSelected = hotFitting.HasSelected;
                model.HotSaleSubTitle = hotFitting.SubTitle;
                model.HotSaleTitle = hotFitting.Title;
                model.HotSaleProductIds = hotFitting.JsonData;
            }
            else
            {
                model.HotSaleTitle = "24小时热卖";
                model.HotSaleSubTitle = "每日8点更新";
                model.HotSaleHasSelected = false;
            }

            #endregion

            #region 新品
            var newFitting = ShopFittingRepository.Get(shopId, ShopFittingType.TodayNews);
            if (newFitting != null)
            {
                model.NewHasSelected = newFitting.HasSelected;
                model.NewSubTitle = newFitting.SubTitle;
                model.NewTitle = newFitting.Title;
                model.NewProductIds = newFitting.JsonData;
            }
            else
            {
                model.NewHasSelected = false;
                model.NewSubTitle = "每日0点更新";
                model.NewTitle = "今日新品";
            }
            #endregion

            #region 店铺展示
            var show = ShopFittingRepository.Get(shopId, ShopFittingType.Show);
            model.ShowHasSelected = false;
            model.ShowAddress = false;
            model.ShowContact = false;
            model.ShowOthers = false;
            model.Others = model.Shop.Others;
            if (show != null)
            {
                model.ShowHasSelected = show.HasSelected;
                if (!string.IsNullOrEmpty(show.JsonData))
                {
                    var showJsonData = JsonConvert.DeserializeObject<ShowJsonData>(show.JsonData);
                    model.ShowAddress = showJsonData.ShowAddress;
                    model.ShowContact = showJsonData.ShowContact;
                    model.ShowOthers = showJsonData.ShowOthers;
                }     
            }           
            #endregion

            #region 商品列表
            var lstProduct = ProductRepository.GetList(User.UserModel.ShopId);
            foreach (var item in lstProduct)
            {
                item.PicUrl = AttachmentSvc.GetPicUrl(ProductImg.GetCoverImgId(item.PicAttIds), 181, 181);
                item.MainPushPicUrl = AttachmentSvc.GetPicUrl(ProductImg.GetCoverImgId(item.PicAttIds), 670, 350);
            }
            model.JsonProducts = JsonConvert.SerializeObject(lstProduct);
            #endregion

            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveMain(ShopFittingModel model)
        {
            var result = new JsonModel();
            SaveFitting(model.MainPushTitle, model.MainPushSubTitle, model.MainPushHasSelected, ShopFittingType.MainPush, model.MainPushProductIds);
            result.msg = "保存成功！";
            result.code = JsonModelCode.Succ;
            return Json(result);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveClassify(ShopFittingModel model)
        {
            var result = new JsonModel();
            SaveFitting(model.ClassifyTitle, "", model.ClassifyHasSelected, ShopFittingType.ProductClassify);
            result.msg = "保存成功！";
            result.code = JsonModelCode.Succ;
            return Json(result);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveHot(ShopFittingModel model)
        {
            var result = new JsonModel();
            SaveFitting(model.HotSaleTitle, model.HotSaleSubTitle, model.HotSaleHasSelected, ShopFittingType.HotSale, model.HotSaleProductIds);
            result.msg = "保存成功！";
            result.code = JsonModelCode.Succ;
            return Json(result);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveNew(ShopFittingModel model)
        {
            var result = new JsonModel();
            SaveFitting(model.NewTitle, model.NewSubTitle, model.NewHasSelected, ShopFittingType.TodayNews, model.NewProductIds);
            result.msg = "保存成功！";
            result.code = JsonModelCode.Succ;
            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveShow(ShopFittingModel model)
        {
            var result = new JsonModel();
            ShowJsonData json = new ShowJsonData
            {
                ShowAddress = model.ShowAddress,
                ShowContact = model.ShowContact,
                ShowOthers = model.ShowOthers
            };
            var showJsonData = JsonConvert.SerializeObject(json);
            SaveFitting(ShopFittingType.Show.GetDescriotion(), "", model.ShowHasSelected, ShopFittingType.Show, showJsonData);
            var shopId = User.UserModel.ShopId;
            var shop = ShopRepository.Get(shopId);
            if (shop != null)
            {
                shop.Others = model.Others;
                ShopRepository.Save(shop);
            }
            result.msg = "保存成功！";
            result.code = JsonModelCode.Succ;
            return Json(result);
        }

        [NonAction]
        private void SaveFitting(string title, string subTitle, bool hasSelected, ShopFittingType fittingType, string jsonData = "")
        {
            var shopId = User.UserModel.ShopId;
            var fitting = ShopFittingRepository.Save(shopId, title, subTitle, hasSelected, fittingType, jsonData);
            LogRepository.Insert(TableSource.ShopFitting, OperationType.Update, fitting.Id);
        }

        [HttpPost]
        public ActionResult SaveSelect(ShopFittingType type)
        {
            var result = new JsonModel();
            var shopId = User.UserModel.ShopId;
            var fitting = ShopFittingRepository.Get(shopId, type);
            if (fitting == null)
            {
                fitting = new ShopFitting();
                fitting.Title = type.GetDescriotion();
                fitting.HasSelected = true;
                fitting.FittingType = type;
                fitting.ShopId = shopId;
            }
            else
            {
                if (fitting.HasSelected)
                {
                    fitting.HasSelected = false;
                }
                else
                {
                    fitting.HasSelected = true;
                }
            }
            ShopFittingRepository.Save(fitting);
            result.code = JsonModelCode.Succ;
            result.data = fitting.HasSelected;
            return Json(result);
        }
    }
}
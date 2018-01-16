using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Steven.Web.Framework.Controllers;
using Steven.Domain.Models;
using Steven.Domain.Repositories;
using Steven.Web.Areas.Shop.Models;
using Steven.Domain.Services;
using Steven.Domain.Enums;
using Steven.Core.Extensions;
using Newtonsoft.Json;
using AutoMapper;
using Steven.Domain.ViewModels;
using Steven.Core.Cache;

namespace Steven.Web.Areas.Shop.Controllers
{
    public class HomeController : ShopController
    {
        public IShopOrderRepository OrderRepository { get; set; }
        public IShopViewRecordRepository ShopViewRecordRepository { get; set; }
        public ISysConfigRepository SysConfigRepository { get; set; }
        public IShopRepository ShopRepository { get; set; }
        public IArticleSvc ArticleSvc { get; set; }
        public IArticleRepository ArticleRepository { get; set; }
        public IUsersRepository UserRepository { get; set; }
        public ICacheManager Cache { get; set; }
        public IArticleClassifyRepository ArticleClassifyRepository { get; set; }
        public IShopBuyWayRepository ShopBuyWayRepository { get; set; }
        // GET: Shop/Home
        public ActionResult Index()
        {
            var model = new HomeIndexModel();
            model.LstArticle = ArticleSvc.GetList(SysConfigRepository.ArticleProductDynamicId, 10);
            model.TotalPayedOrderCount = OrderRepository.GetTodayPayedCount(User.UserModel.ShopId);
            model.ShipmentPendingCount = OrderRepository.GetCount(User.UserModel.ShopId, OrderStatus.ShipmentPending);
            model.WaitingCount = OrderRepository.GetCount(User.UserModel.ShopId, OrderStatus.Waiting);
            model.TodayTotalOrderPrice = OrderRepository.GetTodayTotalPrice(User.UserModel.ShopId);
            var shop = ShopRepository.Get(User.UserModel.ShopId);
            model.Status = shop.Status;
            model.TodayViewCount = ShopViewRecordRepository.GetTodayCount(User.UserModel.ShopId);

            var lstTime = new List<string>();
            var lstCreateOrder = new List<int>();
            var lstPayedOrder = new List<int>();
            var today = DateTime.Now;
            for (int day = 6; day >= 0; day--)
            {
                lstTime.Add(today.AddDays(-day).ToDisplayDate());
                lstCreateOrder.Add(OrderRepository.GetCreateCount(User.UserModel.ShopId, day));
                lstPayedOrder.Add(OrderRepository.GetPayedCount(User.UserModel.ShopId, day));
            }

            model.JsonTime = JsonConvert.SerializeObject(lstTime);
            model.JsonCreateOrderCount = JsonConvert.SerializeObject(lstCreateOrder);
            model.JsonPayedOrderCount = JsonConvert.SerializeObject(lstPayedOrder);

            return View(model);
        }

        [HttpPost]
        public ActionResult ChangeStatus(bool status)
        {
            var result = new JsonModel();
            var shop = ShopRepository.Get(User.UserModel.ShopId);
            shop.Status = status ? ShopStatus.Open : ShopStatus.Closed;
            ShopRepository.Save(shop);

            result.data = shop.Status.GetDescriotion();
            result.code = JsonModelCode.Succ;
            return Json(result);
        }

        #region 文章
        public ActionResult ArticleDetail(long id)
        {
            var article = ArticleRepository.GetEnable(id);
            if (article == null)
            {
                ShowErrorMsg();
                return RedirectToAction("Index");
            }
            var clz = ArticleClassifyRepository.Get(article.ClassifyId);
            if (clz == null)
            {
                ShowErrorMsg();
                return RedirectToAction("Index");
            }
            ViewBag.Classify = clz;
            return View(article);
        }

        public ActionResult ArticleList(long clzId = 0)
        {
            if (clzId == 0)
            {
                clzId = SysConfigRepository.ArticleProductDynamicId;
            }
            var clz = ArticleClassifyRepository.Get(clzId);
            if (clz == null)
            {
                ShowErrorMsg();
                return RedirectToAction("Index");
            }
            return View(clz);
        }

        public ActionResult _ArticleList(long clzId)
        {
            var search = GetSearchModel();
            var lst = ArticleSvc.GetPager(clzId, search);
            return PartialView(lst);
        }
        #endregion

        #region 设置
        public ActionResult Setting()
        {
            var shop = ShopRepository.Get(User.UserModel.ShopId);
            var model = Mapper.Map<Steven.Domain.Models.Shop, SettingModel>(shop);
            model.IsDelivery = ShopBuyWayRepository.GetByShopBuyType(User.UserModel.ShopId, BuyType.Delivery) != null;
            model.IsArrival = ShopBuyWayRepository.GetByShopBuyType(User.UserModel.ShopId, BuyType.Arrival) != null;
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Setting(SettingModel model)
        {
            var result = new JsonModel();
            var shop = ShopRepository.Get(User.UserModel.ShopId);
            Mapper.Map(model, shop);
            ShopRepository.Save(shop);
            result.msg = "保存成功！";
            result.code = JsonModelCode.Succ;
            Cache.Remove(User.UserModel.GId);

            return Json(result);
        }
        [HttpPost]
        public ActionResult ChangeBuyType(BuyType type)
        {
            var result = new JsonModel();
            var shopId = User.UserModel.ShopId;
            var model = ShopBuyWayRepository.GetByShopBuyType(shopId, type);
            if (model == null)
            {
                model = new ShopBuyWay
                {
                    ShopId = shopId,
                    Type = type
                };
                ShopBuyWayRepository.Save(model);
                result.data = true;
            }
            else
            {
                if (ShopBuyWayRepository.IsHaveOneByShop(shopId))
                {
                    ShopBuyWayRepository.Delete(model);
                    result.data = false;
                }
                else
                {
                    result.code = JsonModelCode.Error;
                    result.msg = "购买方式请开通至少一项！";
                    return Json(result);
                }
            }           
            result.code = JsonModelCode.Succ;           
            return Json(result);
        }
        #endregion

        public ActionResult GetOrderCount(OrderStatus status)
        {
            var result = new JsonModel();
            result.data = OrderRepository.GetCount(User.UserModel.ShopId, status);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetShopName()
        {
            var result = new JsonModel();
            var name = string.Empty;
            var shop = ShopRepository.Get(User.UserModel.ShopId);
            if (shop != null)
            {
                name = shop.Name;
            }
            result.data = name;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
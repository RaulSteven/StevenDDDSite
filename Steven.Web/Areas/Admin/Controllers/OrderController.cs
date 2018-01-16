using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Steven.Core.Extensions;
using Steven.Domain.Enums;
using Steven.Domain.Models;
using Steven.Domain.Repositories;
using Steven.Domain.ViewModels;
using Steven.Web.Areas.Admin.Models;
using Steven.Web.Framework.Controllers;

namespace Steven.Web.Areas.Admin.Controllers
{
    public class OrderController : AdminController
    {
        public IShopOrderRepository OrderRepository { get; set; }
        public IShopRepository ShopRepository { get; set; }
        public IShopOrderProductRepository ShopOrderProductRepository { get; set; }
        public IUsersRepository UsersRepository { get; set; }
        public ISysExpressRepository ExpressRepository { get; set; }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetList(long? shopId = null, string keyword = null, DateTime? startTime = null, DateTime? endTime = null, OrderStatus? status = null, BuyType? buyType = null)
        {
            var search = GetSearchModel();
            keyword = keyword?.Trim();
            var list = OrderRepository.GetAdminOrderPager(shopId, keyword, startTime, endTime, status, buyType,
                search);
            foreach (var item in list.rows)
            {
                var shop = ShopRepository.Get(item.ShopId);
                if (shop != null)
                {
                    item.ShopName = shop.Name;
                }
                var user = UsersRepository.Get(item.UserId);
                if (user != null)
                {
                    item.UserName = user.RealName;
                }
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetShopList()
        {
            var lst = ShopRepository.GetList();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Detail(long id, string reUrl)
        {
            var order = OrderRepository.Get(id);
            if (order == null)
            {
                ShowErrorMsg();
                return Redirect(reUrl);
            }
            order.Products = ShopOrderProductRepository.GetShopOrderProList(id);

            return View(order);
        }
        public List<SelectListItem> GetStatusSelectList(BuyType? buyType = null)
        {
            List<SelectListItem> selectListItems = new List<SelectListItem>
            {
                new SelectListItem {Text = "全部"}
            };
            if (!buyType.HasValue)
            {
                return selectListItems;
            }
            var orderStatusList = OrderStatus.Completed.GetSList(OrderStatus.UnPay
        , OrderStatus.Waiting
        , OrderStatus.ShipmentPending
        , OrderStatus.Consign
        , OrderStatus.Completed
        , OrderStatus.Closed);
            if (buyType == BuyType.Arrival)
            {
                orderStatusList = OrderStatus.Completed.GetSList(OrderStatus.UnPay
                , OrderStatus.Paid
                , OrderStatus.Completed
                , OrderStatus.Closed);
            }

            foreach (var item in orderStatusList)
            {
                selectListItems.Add(new SelectListItem
                {
                    Text = item.Text,
                    Value = item.Value
                });
            }
            return selectListItems;
        }
        public ActionResult GetStatus(BuyType? buyType = null)
        {
            string option = "<option value=\"\">全部</option>";
            if (!buyType.HasValue)
            {
                return Json(option, JsonRequestBehavior.AllowGet);
            }
            var orderStatusList = OrderStatus.Completed.GetSList(OrderStatus.UnPay
        , OrderStatus.Waiting
        , OrderStatus.ShipmentPending
        , OrderStatus.Consign
        , OrderStatus.Completed
        , OrderStatus.Closed);
            if (buyType == BuyType.Arrival)
            {
                orderStatusList = OrderStatus.Completed.GetSList(OrderStatus.UnPay
                , OrderStatus.Paid
                , OrderStatus.Completed
                , OrderStatus.Closed);
            }

            foreach (var item in orderStatusList)
            {
                option += "<option value=" + item.Value + ">" + item.Text + "</option>";
            }
            return Json(option, JsonRequestBehavior.AllowGet);
        }
        #region 快递管理
        public ActionResult ExpressList()
        {
            return View();
        }

        public ActionResult GetExpressList(string name)
        {
            var search = GetSearchModel();
            var list = ExpressRepository.GetPager(name, search);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetExpress(long id)
        {
            var result = new JsonModel();
            if (id == 0)
            {
                result.msg = $"找不到id为{id}的记录";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            var m = ExpressRepository.Get(id);
            if (m == null)
            {
                result.msg = $"找不到id为{id}的记录";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            result.data = m;
            result.code = JsonModelCode.Succ;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveExpress(SysExpressModel model)
        {
            var result = new JsonModel();

            SysExpress oModel = null;
            OperationType opType = OperationType.Insert;
            if (model.Id == 0)
            {
                oModel = new SysExpress();
            }
            else
            {
                oModel = ExpressRepository.Get(model.Id);
                if (oModel == null)
                {
                    result.msg = $"找不到id为{model.Id}的记录";
                    return Json(result);
                }
            }
            Mapper.Map(model, oModel);
            ExpressRepository.Save(oModel);
            LogRepository.Insert(TableSource.SysExpress, opType, oModel.Id);
            result.code = JsonModelCode.Succ;
            result.msg = "保存成功！";
            return Json(result);
        }
        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Steven.Core.Extensions;
using Steven.Core.Utilities;
using Steven.Domain.APIModels;
using Steven.Domain.Enums;
using Steven.Domain.Infrastructure;
using Steven.Domain.Models;
using Steven.Domain.Repositories;
using Steven.Domain.Services;
using Steven.Domain.ViewModels;
using Steven.Web.Areas.Shop.Models;
using Steven.Web.Framework.Controllers;
using Newtonsoft.Json;

namespace Steven.Web.Areas.Shop.Controllers
{
    public class OrderController : ShopController
    {
        public IShopOrderRepository ShopOrderRepository { get; set; }
        public IShopOrderProductRepository ShopOrderProductRepository { get; set; }
        public IAttachmentSvc AttachmentSvc { get; set; }
        public IProductRepository ProductRepository { get; set; }
        public IShopOrderProductRepository OrderProductRepository { get; set; }
        public IUsersRepository UsersRepository { get; set; }
        public IShopBuyWayRepository ShopBuyWayRepository { get; set; }
        public ISysExpressRepository SysExpressRepository { get; set; }
        public ActionResult Index(BuyType? t = null, string keyword = null, DateTime? startTime = null, DateTime? endTime = null,
            OrderStatus? status = null)
        {
            var model = new OrderIndexModel
            {
                CurrStatus = status,
                KeyWord = keyword,
                CurrStartTime = startTime,
                CurrEndTime = endTime
            };
            model.IsHaveOrder = ShopOrderRepository.IsShopHaveOrder(User.UserModel.ShopId, status);
            model.TypeList = ShopBuyWayRepository.GetListByShop(User.UserModel.ShopId);
            model.ExpressList = SysExpressRepository.GetSelectList();
            var tModel = model.TypeList.FirstOrDefault();
            if (t.HasValue)
            {
                model.Type = t.Value;
            }
            else if (tModel != null)
            {
                model.Type = tModel.Type;
            }
            else
            {
                model.Type = BuyType.Delivery;
            }
            return View(model);
        }
        public ActionResult _OrderList(BuyType? t = null, string keyword = null, DateTime? startTime = null, DateTime? endTime = null,
            OrderStatus? status = null)
        {
            var search = GetSearchModel();
            keyword = keyword?.Trim();
            var lst = ShopOrderRepository.GetShopOrderPager(User.UserModel.ShopId, t, keyword, startTime, endTime, status,
                search);
            foreach (var item in lst.rows)
            {
                item.Products = ShopOrderProductRepository.GetShopOrderProList(item.Id);
                foreach (var pro in item.Products)
                {
                    pro.ProductPic = AttachmentSvc.GetPicUrl(pro.ProductPicAttId, 100);
                }
                var user = UsersRepository.Get(item.UserId);
                if (user != null)
                {
                    item.UserName = user.RealName;
                }
            }
            return PartialView(lst);
        }

        /// <summary>
        /// 操作
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Action(long orderId, ShopOrder.OrderShopAction action)
        {
            var result = new JsonModel();
            var order = ShopOrderRepository.GetByShopOrderId(orderId, User.UserModel.ShopId);
            if (order == null)
            {
                result.msg = "订单不存在！";
                return Json(result);
            }
            if (!order.HasShopAction(action))
            {
                result.msg = "该订单不允许执行此操作！";
                return Json(result);
            }
            switch (action)
            {
                case ShopOrder.OrderShopAction.Pay:
                    order.PaymentStatus = PaymentStatus.Payed;//订单为支付
                    order.PayTime = DateTime.Now;
                    order.OrderStatus = OrderStatus.Waiting;
                    if (order.BuyType == BuyType.Arrival)
                    {
                        order.OrderStatus = OrderStatus.Paid;
                    }
                    order.PayType = PayType.Gathering;
                    ShopOrderRepository.Save(order);
                    break;
                case ShopOrder.OrderShopAction.Take:
                    order.OrderStatus = OrderStatus.ShipmentPending;
                    ShopOrderRepository.Save(order);
                    break;
                case ShopOrder.OrderShopAction.Sended:
                    order.OrderStatus = OrderStatus.Consign;
                    ShopOrderRepository.Save(order);
                    break;
                case ShopOrder.OrderShopAction.Signed:
                    order.OrderStatus = OrderStatus.Completed;
                    order.DeliveredTime = DateTime.Now;
                    ShopOrderRepository.Save(order);
                    break;
                case ShopOrder.OrderShopAction.Closed:
                    order.OrderStatus = OrderStatus.Closed;
                    ShopOrderRepository.Save(order);
                    var proList = OrderProductRepository.GetShopOrderProList(orderId);
                    foreach (var item in proList)
                    {
                        var product = ProductRepository.Get(item.ProductId);
                        if (product != null)
                        {
                            var stock = product.Stock + item.Number;
                            ProductRepository.UpdateStock(item.ProductId, stock);
                        }
                    }
                    break;
                case ShopOrder.OrderShopAction.DeliveryType:
                    var info = new DeliveryTypeInfo
                    {
                        DeliveryType = order.DeliveryType,
                        DeliveryTypeName = order.DeliveryType.GetDescriotion(),
                        ExpressName = order.ExpressName,
                        ExpressCode = order.ExpressCode
                    };
                    result.data = info;
                    break;
            }

            result.code = JsonModelCode.Succ;
            result.msg = action.GetDescriotion() + "成功！";
            return Json(result);
        }
        [HttpPost]
        public ActionResult ValidCode(long validCodeOrderId, string validCode)
        {
            var result = new JsonModel();
            var order = ShopOrderRepository.GetByShopOrderId(validCodeOrderId, User.UserModel.ShopId);
            if (order == null)
            {
                result.msg = "订单不存在！";
                return Json(result);
            }
            if (!order.HasShopAction(ShopOrder.OrderShopAction.Valid))
            {
                result.msg = "该订单不允许执行此操作！";
                return Json(result);
            }
            if (string.IsNullOrEmpty(validCode) || validCode != order.ValidCode)
            {
                result.msg = "用户验证码错误！";
                return Json(result);
            }
            order.OrderStatus = OrderStatus.Completed;
            ShopOrderRepository.Save(order);
            result.msg = "验证成功！";
            result.code = JsonModelCode.Succ;
            return Json(result);
        }

        public ActionResult _ValidOrder(long orderId)
        {
            var model = ShopOrderRepository.GetByShopOrderId(orderId, User.UserModel.ShopId);
            model.Products = ShopOrderProductRepository.GetShopOrderProList(model.Id);
            foreach (var pro in model.Products)
            {
                pro.ProductPic = AttachmentSvc.GetPicUrl(pro.ProductPicAttId, 100);
            }
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ValidOrder(string validCode)
        {
            var result = new JsonModel();
            var order = ShopOrderRepository.GetByOrderValidCode(User.UserModel.ShopId, validCode);
            if (order == null)
            {
                result.msg = "验证码错误！";
                return Json(result);
            }
            if (!order.HasShopAction(ShopOrder.OrderShopAction.Valid))
            {
                result.msg = "该订单不允许执行此操作！";
                return Json(result);
            }
            result.code = JsonModelCode.Succ;
            result.data = order.Id;
            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OrderSended(long deliveryTypeOrderId, DeliveryType deliveryType, long expressId = 0, string expressName = null, string expressCode = null)
        {
            var result = new JsonModel();
            var order = ShopOrderRepository.GetByShopOrderId(deliveryTypeOrderId, User.UserModel.ShopId);
            if (order == null)
            {
                result.msg = "订单不存在！";
                return Json(result);
            }
            if (!order.HasShopAction(ShopOrder.OrderShopAction.Sended))
            {
                result.msg = "该订单不允许执行此操作！";
                return Json(result);
            }
            if (deliveryType == DeliveryType.Express && string.IsNullOrEmpty(expressName))
            {
                result.msg = "请选择快递公司！";
                return Json(result);
            }
            if (deliveryType == DeliveryType.Express && string.IsNullOrEmpty(expressCode))
            {
                result.msg = "请输入快递单号！";
                return Json(result);
            }
            order.DeliveryType = deliveryType;
            order.ExpressId = expressId;
            order.ExpressName = expressName;
            order.ExpressCode = expressCode;
            order.OrderStatus = OrderStatus.Consign;
            ShopOrderRepository.Save(order);
            result.code = JsonModelCode.Succ;
            result.msg = ShopOrder.OrderShopAction.Sended.GetDescriotion() + "成功！";
            return Json(result);
        }
        public class DeliveryTypeInfo
        {
            public DeliveryType DeliveryType { get; set; }
            public string DeliveryTypeName { get; set; }
            public string ExpressName { get; set; }
            public string ExpressCode { get; set; }
        }
    }
}
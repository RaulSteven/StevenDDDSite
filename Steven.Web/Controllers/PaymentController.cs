using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Steven.Domain.Enums;
using Steven.Domain.Repositories;
using Steven.Domain.Services;
using Steven.Domain.ViewModels;
using Steven.Web.Framework.Controllers;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.TenPayLibV3;

namespace Steven.Web.Controllers
{
    public class PaymentController : WebSiteController
    {
        public IShopOrderRepository OrderRepository { get; set; }
        public IShopAppInfoRepository ShopAppInfoRepository { get; set; }
        public IShopOrderSvc ShopOrderSvc { get; set; }
        #region 微信浏览器内支付回调
        // GET: Payment
        public ActionResult WxPayCallBack(string appId)
        {
            WxPayData data = null;
            if (string.IsNullOrEmpty(appId))
            {
                data = new WxPayData();
                data.SetValue("return_code", "FAIL");
                data.SetValue("return_msg", "参数appId为空！");
                return Content(data.ToXml(), "text/xml");
            }

            var shopAppInfo = ShopAppInfoRepository.GetShopIdByAppId(appId);
            if (shopAppInfo == null)
            {
                data = new WxPayData();
                data.SetValue("return_code", "FAIL");
                data.SetValue("return_msg", "商户不存在！");
                return Content(data.ToXml(), "text/xml");
            }
            ResponseHandler resHandler = new ResponseHandler(null);

            string return_code = resHandler.GetParameter("return_code");
            string return_msg = resHandler.GetParameter("return_msg");

            resHandler.SetKey(shopAppInfo.WxKey);
            //验证请求是否从微信发过来（安全）
            if (resHandler.IsTenpaySign() && return_code.ToUpper() == "SUCCESS")
            {
                var orderCode = resHandler.GetParameter("out_trade_no");
                var order = OrderRepository.GetByOrderCode(orderCode);
                if (order == null)
                {
                    data = new WxPayData();
                    data.SetValue("return_code", "FAIL");
                    data.SetValue("return_msg", "订单不存在！");
                    return Content(data.ToXml(), "text/xml");
                }
                if (order.PaymentStatus == PaymentStatus.Payed)
                {
                    data = new WxPayData();
                    data.SetValue("return_code", "FAIL");
                    data.SetValue("return_msg", "订单已支付！");
                    return Content(data.ToXml(), "text/xml");
                }
                order.PaymentStatus = PaymentStatus.Payed;//订单为支付
                order.PayTime = DateTime.Now;
                order.OrderStatus = OrderStatus.Waiting;
                if (order.BuyType == BuyType.Arrival)
                {
                    order.OrderStatus = OrderStatus.Paid;
                }
                OrderRepository.Save(order);
                //if (!string.IsNullOrEmpty(order.PrePayId))
                //{
                //    ShopOrderSvc.SendTemplate(order.Id, order.ShopId, order.UserId, TemplateType.Payed, order.PrePayId);
                //}
                ShopOrderSvc.ShopNotifyOrderSave(order.ShopId, order, TemplateType.UserPay);
                data = new WxPayData();
                data.SetValue("return_code", "SUCCESS");
                data.SetValue("return_msg", "OK");
                return Content(data.ToXml(), "text/xml");
            }
            data = new WxPayData();
            data.SetValue("return_code", return_code);
            data.SetValue("return_msg", return_msg);
            return Content(data.ToXml(), "text/xml");
        }
        #endregion
    }
}
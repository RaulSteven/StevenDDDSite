using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using Steven.Domain.Models;
using Steven.Domain.Repositories;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.TenPayLibV3;

namespace Steven.Domain.Services
{
    public class WeixinPaySvc : BaseSvc, IWeixinPaySvc
    {
        public ISysConfigRepository ConfigRepository { get; set; }

        /// <summary>
        /// 获取预支付Id
        /// </summary>
        public bool GetPrepayId(string openId, ShopAppInfo shopAppInfo, ShopOrder order, ref string prePayId, ref string nonceStr)
        {
            RequestHandler requestData = PrePayIdInit(shopAppInfo, order,openId);
            //用户的openId
            requestData.SetParameter("openid", openId);
            return PrePayIdResolve(requestData, shopAppInfo.WxKey, ref prePayId, ref nonceStr);
        }

        private bool PrePayIdResolve(RequestHandler requestData, string key, ref string prePayId, ref string nonceStr)
        {
            string sign = requestData.CreateMd5Sign("key", key);
            requestData.SetParameter("sign", sign);
            string data = requestData.ParseXML();
            var result = TenPayV3.Unifiedorder(data);
            Log.Debug("预支付结果：" + result);
            var res = XDocument.Parse(result);
            var resXml = res.Element("xml");
            var returnCode = resXml.Element("return_code").Value;
            if (returnCode.Equals("FAIL"))
            {
                return false;
            }
            if (res.Element("xml").Element("prepay_id") != null)
            {
                prePayId = res.Element("xml").Element("prepay_id").Value;
            }
            if (res.Element("xml").Element("nonce_str") != null)
            {
                nonceStr = res.Element("xml").Element("nonce_str").Value;
            }
            return true;

            //var result = TenPayV3.Unifiedorder(requestData);
            //Log.Debug("预支付结果：" + result);
            //if (result.return_code.Equals("FAIL"))
            //{
            //    return false;
            //}
            //prePayId = result.prepay_id;
            //nonceStr = result.nonce_str;
            //return true;
        }

        private RequestHandler PrePayIdInit(ShopAppInfo shopAppInfo, ShopOrder order, string openId)
        {
            var notifyUrl = $"{ConfigRepository.WebSiteUrl}{"/WxPayBack/" + shopAppInfo.BeiLinAppId}";
            Log.Debug("notifyUrl:" + notifyUrl);
            //var totalPrice = (int)order.TotalPrice * 100;
            //var xmlDataInfo = new TenPayV3UnifiedorderRequestData(shopAppInfo.WxAppId, shopAppInfo.WxMchId, "北琳支付",order.Code, totalPrice, HttpContext.Current.Request.UserHostAddress, notifyUrl, TenPayV3Type.JSAPI, openId, shopAppInfo.WxKey, GetNoncestr());
            //return xmlDataInfo;
            var reqHandler = new RequestHandler(HttpContext.Current);
            reqHandler.Init();
            //设置package订单参数
            reqHandler.SetParameter("appid", shopAppInfo.WxAppId);          //公众账号ID
            reqHandler.SetParameter("mch_id", shopAppInfo.WxMchId);          //商户号
            reqHandler.SetParameter("nonce_str", GetNoncestr());                    //随机字符串
            reqHandler.SetParameter("body", "beilin pay");  //商品描述
            reqHandler.SetParameter("attach", ""); // 附属信息
            reqHandler.SetParameter("out_trade_no", order.Code);        //商家订单号
            reqHandler.SetParameter("total_fee", ((int)(order.TotalPrice * 100)).ToString());                    //商品金额,以分为单位(money * 100).ToString()
            reqHandler.SetParameter("spbill_create_ip", HttpContext.Current.Request.UserHostAddress);   //用户的公网ip，不是商户服务器IP
            reqHandler.SetParameter("notify_url", notifyUrl);            //接收财付通通知的URL
            reqHandler.SetParameter("trade_type", TenPayV3Type.JSAPI.ToString());//交易类型
            return reqHandler;
        }

        public string GetPaySign(string nonceStr, string timeStamp, string prePayId, ShopAppInfo shopAppInfo)
        {
            var package = $"prepay_id={prePayId}";
            var paySign = TenPayV3.GetJsPaySign(shopAppInfo.WxAppId, timeStamp, nonceStr, package, shopAppInfo.WxKey);
            return paySign;
        }

        public string GetNoncestr()
        {
            return TenPayV3Util.GetNoncestr();
        }

        public string GetTimestamp()
        {
            return TenPayV3Util.GetTimestamp();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Steven.Core.Extensions;
using Steven.Core.Utilities;
using Steven.Domain.Enums;
using Steven.Domain.Models;
using Steven.Domain.Repositories;
using Steven.Domain.ViewModels;
using Steven.Web.Framework.Controllers;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;

namespace Steven.Web.Areas.Shop.Controllers
{
    public class CommonController : ShopController
    {
        public IUsersMediaRepository UsersMediaRepository { get; set; }
        public ISysConfigRepository SysConfigRepository { get; set; }
        public IShopTemplateRepository ShopTemplateRepository { get; set; }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BindWx()
        {
            var usersMedia = UsersMediaRepository.GetByUserId(User.UserModel.UserId);
            ViewBag.isOrderDownNotify = ShopTemplateRepository.IsHaveNotify(User.UserModel.ShopId,
                TemplateType.UserOrderDown);
            ViewBag.isPayNotify = ShopTemplateRepository.IsHaveNotify(User.UserModel.ShopId,
                TemplateType.UserPay);
            ViewBag.isTakeNotify = ShopTemplateRepository.IsHaveNotify(User.UserModel.ShopId,
                TemplateType.UserTake);
            return View(usersMedia);
        }
        public ActionResult BindingWx()
        {
            bool isWx = BrowserUtility.IsWxBrowser();
            if (!isWx)
            {
                ShowErrorMsg("对不起，请您在微信端操作！");
                return Redirect(Url.Action("BindWx"));
            }
            var usersMedia = UsersMediaRepository.GetByUserId(User.UserModel.UserId);
            ViewBag.isOrderDownNotify = ShopTemplateRepository.IsHaveNotify(User.UserModel.ShopId,
                TemplateType.UserOrderDown);
            ViewBag.isPayNotify = ShopTemplateRepository.IsHaveNotify(User.UserModel.ShopId,
                TemplateType.UserPay);
            ViewBag.isTakeNotify = ShopTemplateRepository.IsHaveNotify(User.UserModel.ShopId,
                TemplateType.UserTake);           
            return View(usersMedia);
        }
        #region // 绑定微信
        /// <summary>
        /// 微信绑定
        /// </summary>
        /// <returns></returns>
        public ActionResult WxBind()
        {
            var returnUrl = Url.Action("BindingWx");

            var website = SysConfigRepository.WebSiteUrl;

            var callbackUrl = $"{website}{Url.Action("WxBindCallback", new { returnUrl })}";
            bool isWx = BrowserUtility.IsWxBrowser();

            if (!isWx)
            {
                ShowErrorMsg("对不起，请您在微信端操作！");
                return Redirect(Url.Action("BindWx"));
            }
            // 如果是在微信端打开此页面，则弹出授权页面获取用户信息（通过微信公众号平台）
            var authUrl = GetWxAuthorizeUrl(SysConfigRepository.WxAppId, callbackUrl, BrowserType_Wechat);
            return Redirect(authUrl);
        }

        /// <summary>
        /// 微信授权回调地址
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public ActionResult WxBindCallback(string code, string state, string returnUrl = null)
        {
            bool isWx = BrowserUtility.IsWxBrowser();
            if (!isWx)
            {
                ShowErrorMsg("对不起，请您在微信端操作！");
                return Redirect(Url.Action("BindWx"));
            }
            var userId = User.UserModel.UserId;
            var uMedia = UsersMediaRepository.GetByUserId(userId);
            if (uMedia != null)
            {
                ShowErrorMsg("您已绑定了微信！");
                return Redirect(returnUrl);
            }

            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(state)) return Redirect(returnUrl);

            if (!state.Equals(BrowserType_Wechat))
            {
                return Redirect(returnUrl);
            }
            var appId = SysConfigRepository.WxAppId;
            var appSecret = SysConfigRepository.WxAppSecret;

            var accessToken = "";
            var userOpenId = "";// 用户在微信开放平台或微信公众号上的OpenId

            try
            {
                // 获取访问授权
                GetWxAccessToken(appId, appSecret, code, ref accessToken, ref userOpenId);

                // 获取微信用户基本信息
                var userInfo = OAuthApi.GetUserInfo(accessToken, userOpenId);
                if (userInfo == null) return Redirect(returnUrl);

                UsersMediaRepository.Save(userId, userInfo.nickname,
                        userInfo.openid, userInfo.unionid, userInfo.headimgurl);
                ShopTemplateRepository.ShopNotifyAdd(User.UserModel.ShopId,SysConfigRepository);
                ShowSuccMsg("绑定成功！");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                ShowErrorMsg("绑定失败！");
            }

            return Redirect(returnUrl);
        }
        #region // const
        public const string BrowserType_Wechat = "wx";
        public const string BrowserType_Other = "other";

        private const string COOKIES_WxLogin = "WxLogin";
        private const string COOKIES_TOKEN = "WxAccessToken";
        private const string COOKIES_OPENID = "WxUserOpenId";
        #endregion
        /// <summary>
        /// 获取访问授权，只有通过访问授权，才能获取用户详细信息
        /// </summary>
        /// <param name="appId">公众账号的appId或开发平台的appId</param>
        /// <param name="appSecret">公众账号的appSecret或开发平台的appSecret</param>
        /// <param name="code">微信授权回调返回的code</param>
        /// <param name="accessToken">返回的授权</param>
        /// <param name="userOpenId">返回的用户OpenId</param>
        public void GetWxAccessToken(string appId, string appSecret, string code, ref string accessToken,
            ref string userOpenId)
        {
            // 没有过期，则直接从cookie取授权和openid
            HttpCookie cookie = Request.Cookies[COOKIES_WxLogin];
            if (cookie != null && cookie.Expires >= DateTime.Now)
            {
                accessToken = cookie[COOKIES_TOKEN];
                userOpenId = cookie[COOKIES_OPENID];
            }
            else
            {
                // 通过code获取网页访问授权
                var accessTokenInfo = OAuthApi.GetAccessToken(appId, appSecret, code);
                if (accessTokenInfo == null) return;

                // 存储在cookie里
                var cookieToken = new HttpCookie(COOKIES_WxLogin);
                cookieToken.Expires = DateTime.Now.AddSeconds(accessTokenInfo.expires_in);
                cookieToken.Values.Add(COOKIES_TOKEN, accessTokenInfo.access_token);
                cookieToken.Values.Add(COOKIES_OPENID, accessTokenInfo.openid);
                Response.Cookies.Add(cookieToken);

                accessToken = accessTokenInfo.access_token;
                userOpenId = accessTokenInfo.openid;
            }
        }
        /// <summary>
        /// 获取授权重定向的Url
        /// </summary>
        /// <param name="appId">公众账号的appid</param>
        /// <param name="callbackUrl">回调页面</param>
        /// <param name="state"></param>
        /// <returns></returns>
        public string GetWxAuthorizeUrl(string appId, string callbackUrl, string state)
        {
            HttpCookie cookie = Request.Cookies[COOKIES_WxLogin];
            string url = OAuthApi.GetAuthorizeUrl(appId, callbackUrl, state, cookie != null
                ? OAuthScope.snsapi_base
                : OAuthScope.snsapi_userinfo);

            return url;
        }

        /// <summary>
        /// 微信解绑
        /// </summary>
        /// <returns></returns>
        public ActionResult WxUnBind()
        {
            var returnUrl = Url.Action("BindWx");
            bool isWx = BrowserUtility.IsWxBrowser();
            if (isWx)
            {
                returnUrl = Url.Action("BindingWx");
            }
            var uMedia = UsersMediaRepository.GetByUserId(User.UserModel.UserId);
            if (uMedia != null)
            {
                UsersMediaRepository.Delete(uMedia);
            }
            ShopTemplateRepository.ShopNotifyDelete(User.UserModel.ShopId);
            ShowSuccMsg("取消绑定成功！");
            return Redirect(returnUrl);
        }

        [HttpPost]
        public ActionResult ChangeNotify(TemplateType type)
        {
            var result = new JsonModel();
            var shopId = User.UserModel.ShopId;
            var model = ShopTemplateRepository.GetByShopTemplateType(shopId, type);
            if (model == null)
            {
                model = new ShopTemplate
                {
                    Name = type.GetDescriotion(),
                    ShopId = shopId,
                    TemplateType = type,
                    IsUsed = true
                };
                model.TemplateId = ShopTemplateRepository.GetTemplateId(type, SysConfigRepository);
            }
            else
            {
                if (model.IsUsed)
                {
                    model.IsUsed = false;
                }
                else
                {
                    model.IsUsed = true;
                }
            }
            ShopTemplateRepository.Save(model);
            result.code = JsonModelCode.Succ;
            return Json(result);
        }
        #endregion
    }
}
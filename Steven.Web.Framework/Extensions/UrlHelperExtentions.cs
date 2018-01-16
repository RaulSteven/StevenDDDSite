using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Steven.Core.Utilities;
using Steven.Domain.Enums;
using Steven.Domain.Models;
using Steven.Domain.Repositories;
using Steven.Domain.ViewModels;
using Steven.Domain.Services;

namespace Steven.Web.Framework.Extensions
{
    public static class UrlHelperExtentions
    {
        public const string ControllerDefault = "Default";
        public const string AdminDefault = "Admin_default";
        public const string ShopDefault = "Shop_default";

        //首页
        public const string HomeRoute = "HomeRoute";
        //#region // Helpers

        public static string GenerateUrl(this UrlHelper urlHelper, string routeName, string actionName, string controllerName, object values)
        {
            return UrlHelper.GenerateUrl(
                routeName,
                actionName,
                controllerName,
                new RouteValueDictionary(values),
                RouteTable.Routes,
                urlHelper.RequestContext,
                true);
        }

        //#endregion

        #region // 管理页

        public static string AdminHome(this UrlHelper urlHelper)
        {
            return urlHelper.GenerateUrl(AdminDefault, "Index", "Home", null);
        }

        /// <summary>
        /// 无权限
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="reUrl"></param>
        /// <returns></returns>
        public static string AdminNoPermission(this UrlHelper urlHelper, string reUrl = "")
        {
            if (String.IsNullOrEmpty(reUrl))
                return urlHelper.GenerateUrl(AdminDefault, "NoPermission", "Account", null);
            return urlHelper.GenerateUrl(AdminDefault, "NoPermission", "Account", new { reUrl });
        }
        public static string AdminLogin(this UrlHelper urlHelper, string returnUrl = "")
        {
            if (String.IsNullOrEmpty(returnUrl)) return urlHelper.GenerateUrl(AdminDefault, "Login", "Account", null);
            return urlHelper.GenerateUrl(AdminDefault, "Login", "Account", new { returnUrl });
        }

        public static string AdminLogout(this UrlHelper urlHelper, string returnUrl = "")
        {
            if (String.IsNullOrEmpty(returnUrl)) return urlHelper.GenerateUrl(AdminDefault, "Logout", "Account", null);
            return urlHelper.GenerateUrl(AdminDefault, "Logout", "Account", new { returnUrl });
        }
        #endregion

        #region // 公共

        public static string DownUrl(this UrlHelper urlHelper,long id)
        {
            return urlHelper.GenerateUrl(ControllerDefault, "DownloadFile", "Utility", new { id});
        }

        public static string FileUrl(this UrlHelper urlHelper, long attaId)
        {
            if (attaId == 0)
            {
                return "";
            }
            var attaSvc = DependencyResolver.Current.GetService<IAttachmentSvc>();
            return attaSvc.GetAttachmentUrl(attaId);
        }
        

        public static string ThumbUrl(this UrlHelper urlHelper, long picId, int width, int height, List<string> defUrl)
        {
            var imageUrl = "";
            var url = defUrl.Select(d => new { guid = Guid.NewGuid().ToString(), d}).OrderBy(d => d.guid).FirstOrDefault();
            if (url != null)
            {
                imageUrl = url.d;
            }
            return ThumbUrl(urlHelper, picId, width, height, ThumMode.Crop, 100, imageUrl);
        }

        public static string ThumbUrl(this UrlHelper urlHelper, long picId, int width, int height,string defUrl)
        {
            return ThumbUrl(urlHelper,picId, width, height, ThumMode.Crop, 100, defUrl);
        }

        public static string ThumbUrl(this UrlHelper urlHelper, long picId,int width = 100,int height = 100, ThumMode mode = ThumMode.Crop, int q = 100, string defUrl = "")
        {
            if (picId == 0)
            {
                return defUrl;
            }
            var attaSvc = DependencyResolver.Current.GetService<IAttachmentSvc>();
            return attaSvc.GetPicUrl(picId, width, height, mode, q);
        }

        public static string ThumbUrl(this UrlHelper urlHelper, long picId, int width, int height, WaterMarkingPosition position, ThumMode mode = ThumMode.Crop, string defUrl = "")
        {
            if (picId == 0)
            {
                return defUrl;
            }
            var attaSvc = DependencyResolver.Current.GetService<IAttachmentSvc>();
            return attaSvc.GetPicUrl(picId, width, height, mode, 100, position);
        } 

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <returns></returns>
        public static string GetVerifyCode(this UrlHelper urlHelper)
        {
            return urlHelper.GenerateUrl(ControllerDefault, "GetVerifyCode", "Utility", null);
        }
        /// <summary>
        /// 图片管理
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <returns></returns>
        public static string GetFileMana(this UrlHelper urlHelper)
        {
            return urlHelper.GenerateUrl(AdminDefault, "FileMana", "Attachment", null);
        }


        #endregion

        #region 首页
        public static string Home(this UrlHelper urlHelper)
        {
            return urlHelper.GenerateUrl(HomeRoute, "Index", "Home", null);
        }

        #endregion

    }
}

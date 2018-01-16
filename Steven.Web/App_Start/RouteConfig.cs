using Steven.Web.Framework.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Steven.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            var namespaces = new[] { "Steven.Web.Controllers" };
            #region 首页
            routes.MapRoute(
                name: UrlHelperExtentions.HomeRoute,
                url: "",
                defaults: new
                {
                    controller = "Home",
                    action = "Index",
                },
                namespaces: namespaces
           );
            #endregion

            #region 工具类，如：404、500

            routes.MapRoute(
                name: "NotFound",
                url: "404.html",
                defaults: new
                {
                    controller = "Home",
                    action = "NotFind"
                },
                namespaces: namespaces);
            routes.MapRoute(
                name: "Error",
                url: "Error.html",
                defaults: new
                {
                    controller = "Home",
                    action = "Error"
                },
                namespaces: namespaces);
            #endregion


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                namespaces:namespaces,
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

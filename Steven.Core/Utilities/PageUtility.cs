using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace Steven.Core.Utilities
{
    public static class PageUtility
    { 
        private static string GenerateUrl(HtmlHelper html,string pageIndexParameterName,int pageIndex,string routeName)
        { 
            var actionName = html.ViewContext.RouteData.Values["action"].ToString();
            var controllerName = html.ViewContext.RouteData.Values["controller"].ToString();
            var routeValues = GetCurrentRouteValues(html.ViewContext, actionName, controllerName, pageIndexParameterName);
            routeValues[pageIndexParameterName] = pageIndex; 
            var urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            if (!string.IsNullOrEmpty(routeName))
                return urlHelper.RouteUrl(routeName, routeValues);
            return urlHelper.RouteUrl(routeValues);
        }

        private static RouteValueDictionary GetCurrentRouteValues(ViewContext viewContext, string actionName, string controllerName, string pageIndexParameterName)
        {
            var routeValues = new RouteValueDictionary();
            var rq = viewContext.HttpContext.Request.QueryString;
            if (rq != null && rq.Count > 0)
            {
                var invalidParams = new[] { "x-requested-with", "xmlhttprequest", pageIndexParameterName.ToLower() };
                foreach (string key in rq.Keys)
                {
                    if (!string.IsNullOrEmpty(key) && Array.IndexOf(invalidParams, key.ToLower()) < 0)
                    {
                        routeValues[key] = rq[key];
                    }
                }
            } 
            routeValues["action"] = actionName; 
            routeValues["controller"] = controllerName;
            return routeValues;
        }

        public static string GetUrl(this HtmlHelper helper, string pageParameterName, int currentPage,int lastPage)
        { 
            
            if (currentPage >= lastPage)
            {
                currentPage = lastPage;
            }
            if (currentPage < 1)
            {
                currentPage = 1;
            }
            var url = GenerateUrl(helper, pageParameterName, currentPage, null); 
            return url;
        }
    }
}

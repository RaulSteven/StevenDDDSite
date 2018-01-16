using Steven.Domain.Enums;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;

namespace Steven.Domain.APIModels
{
    public class HeadUtility
    {
        public const string ShowHeader = "showheader";
        public const string ShowFooter = "showfooter";
        public const string ETagKey = "ETag";
        public const string UserAgentKey = "ua";
        public const string Lang = "lang";
        public const string BeiLinShopKey = "beilinshopkey";
        public static void AddHead(string headKey, string value)
        {
            HttpContext.Current.Response.AppendHeader(headKey, value);
        }

        public static string GetHead(string headKey, string defvalue)
        {
            var value = string.Empty;
            if (HttpContext.Current.Request.Headers.AllKeys.Contains(headKey))
            {
                value = HttpContext.Current.Request.Headers[headKey];
            }

            return string.IsNullOrEmpty(value) ? defvalue : value.Trim();
        }


        public static string GetMobilePlatform()
        {
            var ua = GetHead("ua", "");
            var source = "Other";
            if (!string.IsNullOrEmpty(ua))
            {
                var arrUa = ua.Split('|');
                if (arrUa.Length > 3)
                {
                    source = arrUa[2];
                }
            }
            return source;
        }
    }
}

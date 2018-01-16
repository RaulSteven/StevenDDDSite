using System;
using System.Linq;
using System.Web;

namespace Steven.Core.Utilities
{
    public class CookieUtils
    {
        /// <summary>
        /// 删除指定的Cookie
        /// </summary>
        /// <param name="cookiename"></param>
        public static void RemoveCookie(string cookiename)
        {
            if (null != HttpContext.Current.Response.Cookies[cookiename])
            {
                HttpContext.Current.Response.Cookies.Remove(cookiename);
            }
            if (null != HttpContext.Current.Request.Cookies[cookiename])
            {
                HttpContext.Current.Request.Cookies.Remove(cookiename);
            }
            var co = new HttpCookie(cookiename) {Expires = DateTime.Now.AddYears(-1)};
            HttpContext.Current.Response.Cookies.Add(co);
        }

        public static void SetCookie(string cookiename, object value, bool remember)
        {
            if (HttpContext.Current.Response.Cookies.AllKeys.Contains(cookiename))
            {
                RemoveCookie(cookiename);   
            }
            AddCookie(cookiename, value, remember);
        }

        public static void AddCookie(string cookiename, object value,bool remember)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return;
            }
            var co = new HttpCookie(cookiename,HttpUtility.UrlEncode(value.ToString()));
            if (remember)
            {
                co.Expires = DateTime.Now.AddYears(1);
            }
            else
            {
                co.Expires = DateTime.Now.AddHours(1);
            }
            
            HttpContext.Current.Response.Cookies.Add(co);
        }

        public static void AddCookie(string cookiename, object value, DateTime expires)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return;
            }
            var co = new HttpCookie(cookiename, HttpUtility.UrlEncode(value.ToString())) { Expires = expires,HttpOnly=true}; 
            HttpContext.Current.Response.Cookies.Add(co);
        }

        public static string GetCookie(string cookiename, string defvalue)
        {
            HttpCookie co = HttpContext.Current.Request.Cookies[cookiename];
            if (null == co || string.IsNullOrEmpty(co.Value)) return defvalue;

            return HttpUtility.UrlDecode(co.Value);
        }
        public static int GetCookie(string cookiename, int defvalue)
        {
            HttpCookie co = HttpContext.Current.Request.Cookies[cookiename];
            if (null == co) return defvalue;

            return ConvertToInt32(co.Value.Trim(), defvalue);
        }

        private static int ConvertToInt32(string value, int defvalue)
        {

            int result = 0;
            if (int.TryParse(value, out result))
            {
                return result;
            }
            return defvalue;
        }
        public const string WeixinEnrtyptKey = "MLS#WeixinKey";

        public const string WeixinCookieKey = "adh-w-k";

        public const string StyleColorKey = "adh_style_color";
        public const string LayoutOptionKey = "adh_layout_option";
        public const string HeaderOptionKey = "adh_header_option";
        public const string SidebarOptionKey = "adh_sidebar_option";
        public const string FooterOptionKey = "adh_footer_option";
    }
}

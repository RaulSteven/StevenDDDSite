using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Steven.Core.Utilities
{
    public class BrowserUtility
    {
        /// <summary>
        /// 判断是否是微信浏览器
        /// </summary>
        /// <returns></returns>
        public static bool IsWxBrowser()
        {
            if (HttpContext.Current.Request.UserAgent != null &&
                        HttpContext.Current.Request.UserAgent.IndexOf("MicroMessenger", StringComparison.Ordinal) > 0)
            {
                return true;
            }

            return false;
        }
        public static bool IsMoblie()
        {
            string agent = (HttpContext.Current.Request.UserAgent + "").ToLower().Trim();

            if (agent == "" ||
                agent.IndexOf("mobile") != -1 ||
                agent.IndexOf("mobi") != -1 ||
                agent.IndexOf("nokia") != -1 ||
                agent.IndexOf("samsung") != -1 ||
                agent.IndexOf("sonyericsson") != -1 ||
                agent.IndexOf("mot") != -1 ||
                agent.IndexOf("blackberry") != -1 ||
                agent.IndexOf("lg") != -1 ||
                agent.IndexOf("htc") != -1 ||
                agent.IndexOf("j2me") != -1 ||
                agent.IndexOf("ucweb") != -1 ||
                agent.IndexOf("opera mini") != -1 ||
                agent.IndexOf("mobi") != -1 ||
                agent.IndexOf("android") != -1 ||
                agent.IndexOf("iphone") != -1)
            {
                //终端可能是手机

                return true;

            }

            return false;
        }
    }
}

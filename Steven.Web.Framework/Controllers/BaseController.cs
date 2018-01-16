using System.Web.Mvc;
using Steven.Web.Framework.Security;
using log4net;
using Steven.Domain.ViewModels;
using Steven.Core.Utilities;
using System.Web;
using Steven.Domain.Infrastructure;
using Steven.Domain.Infrastructure.SysUser;

namespace Steven.Web.Framework.Controllers
{
    public class BaseController : Controller
    {
        protected readonly ILog Log = null;

        public BaseController()
        {
            Log = LogManager.GetLogger(GetType().FullName);
        }
     
        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            var ip = filterContext.HttpContext.Request != null
                ? filterContext.HttpContext.Request.UserHostAddress
                : "";
            var rawUrl = filterContext.HttpContext.Request != null && filterContext.HttpContext.Request.UrlReferrer != null
                ? filterContext.HttpContext.Request.UrlReferrer.ToString()
                : "";
            Log.FatalFormat("\r\n controller:{0},action:{1},IP:{2},RawUrl:{3},msg:{4}",
                RouteData.Values["controller"],
                RouteData.Values["action"],
                ip,
                rawUrl,
                filterContext.Exception);
        }
    }
}
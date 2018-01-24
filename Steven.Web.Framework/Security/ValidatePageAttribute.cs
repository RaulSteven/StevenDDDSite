using Steven.Domain.Infrastructure;
using Steven.Domain.Infrastructure.SysUser;
using Steven.Domain.ViewModels;
using Steven.Web.Framework.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Steven.Web.Framework.Security
{
    public class ValidatePageAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            var url = new UrlHelper(filterContext.RequestContext);
            var adminUser = filterContext.HttpContext.User as AdminUser;
            var targetUrl = $"/Admin/{filterContext.RouteData.Values["controller"]}/{filterContext.RouteData.Values["action"]}";
            adminUser.UserModel.FindCurrentMenu(targetUrl);
            if (adminUser.UserModel.CurrPage == null)
            {
                //跳转到无权限页面
                var noPerUrl = url.AdminNoPermission();
                filterContext.Result = new RedirectResult(noPerUrl);
                return;
            }
        }
    }
}

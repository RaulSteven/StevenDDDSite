using Steven.Domain.Enums;
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
    public class ValidateButtonAttribute : ActionFilterAttribute
    {
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public SysButton Buttons { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            var url = new UrlHelper(filterContext.RequestContext);
            var adminUser = filterContext.HttpContext.User as AdminUser;
            ControllerName = ControllerName ?? filterContext.RouteData.Values["controller"].ToString();
            ActionName = ActionName ?? filterContext.RouteData.Values["action"].ToString();
            var targetUrl = $"/Admin/{ControllerName}/{ActionName}";
            adminUser.UserModel.FindCurrentMenu(targetUrl);
            if (adminUser.UserModel.FirstMenu == null
                || !adminUser.HasButton(Buttons))
            {
                //跳转到无权限页面
                var noPerUrl = url.AdminNoPermission();
                filterContext.Result = new RedirectResult(noPerUrl);
                
            }
        }
    }
}

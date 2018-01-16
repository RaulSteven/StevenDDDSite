using System.Web.Mvc;
using Steven.Domain.Enums;
using Steven.Domain.ViewModels;
using Steven.Web.Framework.Extensions;
using System.Linq;
using Steven.Domain.Infrastructure;
using Steven.Domain.Infrastructure.SysUser;

namespace Steven.Web.Framework.Security
{ 

    public class ValidateAdminLoginAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var url = new UrlHelper(filterContext.RequestContext);
            var adminUser = filterContext.HttpContext.User as AdminUser;
            if (adminUser == null 
                || !adminUser.Identity.IsAuthenticated)
            {
                var requestUrl = filterContext.HttpContext.Request.RawUrl.ToLower();
                var loginUrl = url.AdminLogin(requestUrl);
                filterContext.Result = new RedirectResult(loginUrl);
            }
        }
    }
}

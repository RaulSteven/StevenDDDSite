using Steven.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Steven.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            DependencyConfig.Register();
            AutoMapperConfig.Register();
            //在应用程序启动时运行的代码  
            log4net.Config.XmlConfigurator.Configure();
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthenticationSvc.GetCookieName()];
            if (authCookie != null)
            {
                var formAuthSvc = DependencyResolver.Current.GetService<IFormsAuthenticationSvc>();
                formAuthSvc.FromAuthenticationTicket(authCookie);
            }
        }
    }
}

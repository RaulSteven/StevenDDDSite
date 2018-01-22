using Steven.Domain.Models;
using System.Security.Principal;
using System.Web;

namespace Steven.Domain.Services
{
    public interface IFormsAuthenticationSvc
    {

        void CreateAuthenticationTicket(Users user, HttpResponseBase response, HttpContextBase httpContextBase, bool remember);
        void FromAuthenticationTicket(HttpCookie cookie);

        void LogOut(IPrincipal user);

        string GetUserNameCookieKey();
    }
}

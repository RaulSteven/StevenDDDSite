using Steven.Domain.Infrastructure;
using Steven.Domain.Infrastructure.SysUser;
using Steven.Domain.Models;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;

namespace Steven.Domain.Services
{
    public interface IFormsAuthenticationSvc
    {

        void CreateAuthenticationTicket(Users user, HttpResponseBase response, HttpContextBase httpContextBase, bool remember);
        void FromAuthenticationTicket(HttpCookie cookie);

        void LogOut(IPrincipal user);

        string GetUserNameCookieKey();
        #region web Api

        void CreateApiAuthMemberTiket(Users user);
        void FromApiAuthenticationTicket(string ticket);

        void ApiLogOut(string id);
        #endregion
    }
}

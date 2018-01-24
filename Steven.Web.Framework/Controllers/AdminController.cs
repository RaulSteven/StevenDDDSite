using System.Web.Mvc;
using Steven.Web.Framework.Security;
using Steven.Domain.ViewModels;
using Steven.Core.Utilities;
using Steven.Domain.Infrastructure;
using Steven.Domain.Repositories;
using Steven.Domain.Infrastructure.SysUser;

namespace Steven.Web.Framework.Controllers
{
    [ValidateAdminLogin]
    public class AdminController : BaseController
    {
        public ISysOperationLogRepository LogRepository { get; set; }
        protected PageSearchModel GetSearchModel()
        {
            var model = new PageSearchModel()
            {
                Sort = Request.QueryString["sort"]??"UpdateTime",
                Order = Request.QueryString["order"]??"desc",
                Offset = StringUtility.ConvertToInt(Request.QueryString["offset"],0),
                Limit = StringUtility.ConvertToInt(Request.QueryString["limit"],10)
            };
            return model;
        }

        [NonAction]
        public void ShowErrorMsg(string msg="记录不存在！")
        {
            TempData["msgType"] = "error";
            TempData["msg"] = msg;
        }

        [NonAction]
        public void ShowSuccMsg(string msg)
        {
            TempData["msgType"] = "succ";
            TempData["msg"] = msg;
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var targetUrl = $"/Admin/{filterContext.RouteData.Values["controller"]}/{filterContext.RouteData.Values["action"]}";
            //User.UserModel.FindCurrentMenu(targetUrl);
            ViewBag.CurrUser = User;

            base.OnActionExecuted(filterContext);
        }

        public new AdminUser User
        {
            get
            {
                if (base.User is AdminUser)
                {
                    return base.User as AdminUser;
                }
                return new AdminUser();
            }
        }

    }
}
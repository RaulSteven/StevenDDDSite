using Steven.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Steven.Web.Framework.Security;
using Steven.Domain.Infrastructure.SysUser;
using Steven.Domain.Infrastructure;
using Steven.Core.Utilities;

namespace Steven.Web.Framework.Controllers
{
    [ValidateShopLogin]
    public class ShopController : BaseController
    {
        public ISysOperationLogRepository LogRepository { get; set; }
        protected PageSearchModel GetSearchModel()
        {
            var model = new PageSearchModel()
            {
                Sort = Request.QueryString["sort"] ?? "Id",
                Order = Request.QueryString["order"] ?? "desc",
                Offset = StringUtility.ConvertToInt(Request.QueryString["offset"], 0),
                Limit = StringUtility.ConvertToInt(Request.QueryString["limit"], 10)
            };
            return model;
        }
        public new ShopUser User
        {
            get
            {
                if (base.User is ShopUser)
                {
                    return base.User as ShopUser;
                }
                return new ShopUser();
            }
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ViewBag.CurrUser = User;

            base.OnActionExecuted(filterContext);
        }

        [NonAction]
        public void ShowErrorMsg(string msg = "记录不存在！")
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
    }
}

using System.Web.Mvc;
using Steven.Web.Framework.Extensions;

namespace Steven.Web.Framework.Controllers
{
    public class WebSiteController :BaseController
    {
        [NonAction]
        public ActionResult RedirectTo404(string reUrl, string msg)
        {
            ViewBag.ReUrl = reUrl;
            ViewBag.Msg = msg;
            return View("NotFind");
        }
    }
}
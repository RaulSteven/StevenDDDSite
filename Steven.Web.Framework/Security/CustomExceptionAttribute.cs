using System.Web;
using System.Web.Mvc;

namespace Steven.Web.Framework.Security
{
    public class CustomExceptionAttribute : IExceptionFilter   //HandleErrorAttribute
    {
        public void OnException(ExceptionContext filterContext)
        {

            if (filterContext.ExceptionHandled)
            {
                HttpException httpExce = filterContext.Exception as HttpException;
                if (httpExce.GetHttpCode() != 500)//为什么要特别强调500 因为MVC处理HttpException的时候，如果为500 则会自动
                //将其ExceptionHandled设置为true，那么我们就无法捕获异常
                {
                    return;
                }
            }
            if (filterContext.Exception != null)
            {
                filterContext.Controller.ViewBag.UrlRefer = filterContext.HttpContext.Request.UrlReferrer;
                filterContext.Controller.TempData["msg"] = filterContext.Exception.Message;
                filterContext.Result = new RedirectResult("~/500.html");  
            }
            //写入日志 记录
            filterContext.ExceptionHandled = true;//设置异常已经处理
        }
    }
}
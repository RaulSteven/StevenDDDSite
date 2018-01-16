using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Steven.Web.Framework.Controllers;
using Steven.Core.Extensions;
using System.Text;
using Newtonsoft.Json;
using Steven.Domain.Enums;
using System.Threading.Tasks;
using Steven.Domain.ViewModels;

namespace Steven.Web.Areas.Admin.Controllers
{
    public class UtilityController : AdminController
    {
        #region Enum Formatter
        //function CommonStatusFormatter(value, row) {
        //    var arra = [];
        //    return arra[value];
        //};
        [NonAction]
        public ActionResult GenScript(string name, Dictionary<short, string> dict)
        {
            var script = new StringBuilder();
            script.AppendFormat("function {0}Formatter(value, row)", name).Append("{");
            var arr = JsonConvert.SerializeObject(dict);
            script.AppendFormat("var enumArra={0};", arr);
            script.AppendFormat("return enumArra[value];").Append("};");

            return JavaScript(script.ToString());
        }
        
        public ActionResult CommonStatusFormatter()
        {
            return GenScript("CommonStatus", CommonStatus.Deleted.GetDescriptDict());
        }

        public ActionResult GenderFormatter()
        {
            return GenScript("Gender", Gender.Female.GetDescriptDict());

        }

        public ActionResult SysConfigClassifyFormatter()
        {
            return GenScript("SysConfigClassify", SysConfigClassify.Home.GetDescriptDict());

        }

        public ActionResult SysConfigTypeFormatter()
        {
            return GenScript("SysConfigType", SysConfigType.Bool.GetDescriptDict());

        }

        public ActionResult ArticleFlagsFormatter()
        {
            return GenScript("ArticleFlags", ArticleFlags.Nothing.GetDescriptDict());
        }
   
        public ActionResult JobTaskStatusFormatter()
        {
            return GenScript("JobTaskStatus", JobTaskStatus.Deleted.GetDescriptDict());
        }
        #endregion


        #region BatchDele
        [HttpPost]
        public ActionResult BatchDele(TableSource src, string ids)
        {
            var result = new JsonModel();
            var dele = LogRepository.BatchDele(src, ids);
            if (dele > 0)
            {
                result.msg = "删除成功！";
                result.code = JsonModelCode.Succ;
                LogRepository.Insert(src, OperationType.Delete, ids);
            }
            else
            {
                result.msg = "删除失败！";
            }
            return Json(result);

        }
        #endregion
    }
}
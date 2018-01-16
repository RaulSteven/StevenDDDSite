using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Steven.Web.Framework.Extensions
{
    public static class HtmlExtensions
    {
        #region RadioButtonList

        public static IHtmlString RadioButtonList(this HtmlHelper helper, string name, IEnumerable<string> items, string selectedValue)
        {
            var selectList = new SelectList(items);
            return helper.RadioButtonList(name, selectList, selectedValue, "", "");
        }

        public static IHtmlString RadioButtonList(this HtmlHelper helper, string name, IEnumerable<SelectListItem> items)
        {
            return helper.RadioButtonList(name, items, null, "", "");
        }

        public static IHtmlString RadioButtonList(this HtmlHelper helper, string name, IEnumerable<SelectListItem> items, Enum enumValue)
        {
            return helper.RadioButtonList(name, items, enumValue.ToString(), "", "");
        }

        public static IHtmlString RadioButtonList(this HtmlHelper helper, string name, IEnumerable<SelectListItem> items, string selectedValue)
        {
            return helper.RadioButtonList(name, items, selectedValue, "", "");
        }

        public static IHtmlString RadioButtonList(this HtmlHelper helper, string name, IEnumerable<SelectListItem> items, string selectedValue, string optionText, string optionValue)
        {
            StringBuilder sb = new StringBuilder();
            const string radioTemplate = @"
                <div class=""i-checks checkbox-inline"">
                    <label>
                        <input type=""radio"" value=""{0}"" {1} name=""{2}"" id=""{3}"" >
                        <i></i>{4}
                    </label>
                </div> ";
            if (!string.IsNullOrEmpty(optionText))
            {
                string rbValue = optionValue ?? optionText;
                string rbId = name + "_" + rbValue;

                sb.AppendFormat(radioTemplate, rbValue, rbValue == selectedValue ? "checked" : "", name, rbId,optionText);
            }
            foreach (var item in items)
            {
                var rbValue = item.Value ?? item.Text;
                var rbId = name + "_" + rbValue;
                var isCheck = (null != item.Value && (item.Selected || item.Value == selectedValue));
                sb.AppendFormat(radioTemplate, rbValue, isCheck ? "checked" : "", name, rbId, item.Text);
            }
            return MvcHtmlString.Create(sb.ToString());
        }


        #endregion

        #region CheckboxList

        public static MvcHtmlString CheckboxList(this HtmlHelper helper, string name, IEnumerable<SelectListItem> items)
        {
            StringBuilder sb = new StringBuilder();
            const string template = @"
                <div class=""i-checks checkbox-inline"">
                    <label>
                        <input type=""checkbox"" value=""{0}"" name=""{1}"" id=""{2}"" >
                        <i></i>{3}
                    </label>
                </div> ";
            foreach (var item in items)
            {
                var rbValue = item.Value ?? item.Text;
                var rbId = name + "_" + rbValue;
                sb.AppendFormat(template, rbValue, name, rbId, item.Text);
            }
            return MvcHtmlString.Create(sb.ToString());
        }
        

        #endregion
    }
}

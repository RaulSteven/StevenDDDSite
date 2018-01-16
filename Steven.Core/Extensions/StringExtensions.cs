using System.Linq;
using System.Text.RegularExpressions;
using Steven.Core.Utilities;

namespace Steven.Core.Extensions
{
    public static class StringExtensions
    {
        public static string ToEllipsis(this string str, int count,bool more = false)
        {
            var replactString = "...";
            if (more)
            {
                replactString = "......";
            }
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            return str.Length < count ? str : str.Substring(0, count) + replactString;
        }

        public static string ToEllipsisForEnglish(this string str, int count, bool more = false)
        {
            var replactString = "...";
            if (more)
            {
                replactString = "......";
            }
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            var result = str.Split(' ').ToList();
            return result.Count < count ? str : string.Join(" ", result.Take(count).ToList()) + replactString;
        }

        /// <summary>
        /// 过滤html
        /// </summary>
        /// <param name="htmlstring"></param>
        /// <returns></returns>
        public static string NoHtml(this string htmlstring)
        { 
            htmlstring = Regex.Replace(htmlstring, @"<script[^>]*?>.*?</script>", "",RegexOptions.IgnoreCase); 
            htmlstring = Regex.Replace(htmlstring, @"<(.[^>]*)>", "",RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"([\r\n])[\s]+", "",RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(quot|#34);", "\"",RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(amp|#38);", "&",RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(lt|#60);", "<",RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(gt|#62);", ">",RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(nbsp|#160);", "   ",RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(iexcl|#161);", "\xa1",RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(cent|#162);", "\xa2",RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(pound|#163);", "\xa3",RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(copy|#169);", "\xa9",RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&#(\d+);", "",RegexOptions.IgnoreCase); 
            htmlstring = htmlstring.Replace("<", "");
            htmlstring = htmlstring.Replace(">", "");
            htmlstring = htmlstring.Replace("\r\n", ""); 
            return htmlstring;
        }


        //判断是否是整形
        public static bool IsNumber(string text)
        {   
           var regStr=  RegexUtility.NoNegativeInt;
            Regex reg = new Regex(regStr);
            Match ma = reg.Match(text);
            if (ma.Success)
            {
                return true;
            }
                return false;
        }

        public static string RemoveSpaces(this string val)
        {
            return string.IsNullOrEmpty(val) ? val : Regex.Replace(val, @"\s+", " ").Trim();
        }

        public static string ToUrl(this string url)
        {
            return string.IsNullOrEmpty(url) ? "javascript:;" : url;
        }
    }
}
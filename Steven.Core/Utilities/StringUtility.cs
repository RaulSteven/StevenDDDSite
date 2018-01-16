using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Steven.Core.Utilities
{
    public class StringUtility
    {
        public static long[] ConvertToBigIntArray(string value, char seperator)
        {
            if (string.IsNullOrEmpty(value))
            {
                return new long[] {0};
            }

            var strArr = value.Split(seperator);
            var longArr = new long[strArr.Length];
            for (int i = 0; i < strArr.Length; i++)
            {
                longArr[i] = ConvertToBigInt(strArr[i]);
            }
            return longArr;
        }

        public static int[] ConvertToInt32Array(string value, char seperator)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            var strArr = value.Split(seperator);
            var intArr = new int[strArr.Length];
            for (int i = 0; i < strArr.Length; i++)
            {
                intArr[i] = ConvertToInt(strArr[i]);
            }
            return intArr;
        }

        public static int ConvertToInt(string value, int def = 0)
        {
            int result = 0;
            return int.TryParse(value, out result) ? result : def;
        }

        public static long ConvertToBigInt(string value, long def = 0)
        {
            long result = 0;
            return long.TryParse(value,out result) ? result : def;
        }

        public static decimal ConvertToDecimal(string value, decimal def = 0)
        {
            decimal result = 0;
            return decimal.TryParse(value, out result) ? result : def;
        }

        public static DateTime ConvertToDateTime(string value)
        {
            var result = DateTime.MinValue;
            DateTime.TryParse(value, out result);
            return result;
        }

        public static bool ConvertToBool(string value)
        {
            var result = false;
            bool.TryParse(value, out result);
            return result;
        }

        /// <summary>
        /// 64位加密
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string XBase64Encode(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            return HttpServerUtility.UrlTokenEncode(Encoding.UTF8.GetBytes(value));
        }

        /// <summary>
        /// 64位解密
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string XBase64Decode(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            return UTF8Encoding.UTF8.GetString(HttpServerUtility.UrlTokenDecode(value));
        }

        public static string NewGuidHashCode()
        {
            return Math.Abs(Guid.NewGuid().GetHashCode()).ToString();
        }

        /// <summary>
        /// 返回图片地址
        /// </summary>
        /// <param name="content">提取的内容</param>
        /// <returns></returns>
        public static List<string> GetImgUrl(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return null;
            }
            var regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);  
            var matches = regImg.Matches(content);
            if (matches.Count < 1)
            {
                return null;
            }
            var i = 0;
            var list = new string[matches.Count]; 
            foreach (Match match in matches)
            {
                list[i++] = match.Groups["imgUrl"].Value;
            }
            return list.Where(d => !string.IsNullOrEmpty(d)).ToList();
        }

        /// <summary>
        /// 从编辑器中返回上传的图片地址
        /// </summary>
        /// <param name="content">编辑的内容</param>
        /// <returns></returns>
        public static string GetEditorImgUrl(string content)
        {
            try
            {
                var src = GetImgUrl(content);
                if (src == null || !src.Any())
                {
                    return "";
                }
                const string code = "/File/";
                var list = src.Where(d => !string.IsNullOrEmpty(d) && 
                                          d.IndexOf(code, StringComparison.Ordinal) >= 0)
                              .Select(d => XBase64Decode((d.TrimEnd('/')).Replace(code, "")))
                              .ToList();
                return string.Join(",", list);
            }
            catch (Exception e)
            {
                return "";
            }
           
        }

        public static bool IsChineseLetter(string input, int index)
        {
            int chfrom = Convert.ToInt32("4e00", 16);    //范围（0x4e00～0x9fff）转换成int（chfrom～chend）
            int chend = Convert.ToInt32("9fff", 16);
            if (input == "")
                return false;

            int code = Char.ConvertToUtf32(input, index);    //获得字符串input中指定索引index处字符unicode编码          
            return (code >= chfrom && code <= chend);
        }

        public static string LineFeed(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }
            return input.Replace("\r\n", "<br/>")
                .Replace("\n","<br/>")
                .Replace("\r","<br/>");
        }
        /// <summary>
        /// App客户端密码限制长度
        /// </summary>
        /// <param name="pwd">密码</param>
        /// <param name="msg">回调的错误信息</param>
        /// <param name="defaultError">自定义的错误提示</param>
        /// <returns></returns>
        public static bool IsAppUserPwd(string pwd, ref string msg, string defaultError = "")
        {
            if (pwd.Length >= 6 && pwd.Length <= 16)
            {
                return true;
            }
            msg = string.IsNullOrEmpty(defaultError) ? "密码的长度只能在6到16个字之间！" : defaultError;

            return false;
        }
    }
}
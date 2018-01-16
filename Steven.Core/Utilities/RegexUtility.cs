using System.Text.RegularExpressions;

namespace Steven.Core.Utilities
{
    public class RegexUtility
    {
        #region const regex 
        /// <summary>
        /// 支持手机号码，3-4位区号，7-8位直播号码，1－4位分机号
        /// </summary>
        public const string PhoneOrTel = @"((\d{11})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$)";

        /// <summary>
        /// 手机号验证
        /// </summary>
        public const string Phone = @"(^\d{11}$)|(^852\d{8}$)";


        /// <summary>
        /// 中国手机号，11位数字
        /// </summary>
        public const string ChinaPhone = @"^\d{11}$";
        /// <summary>
        /// 美国手机号，10位数字
        /// </summary>
        public const string USAPhone = @"^\d{10}$";

        /// <summary>
        /// 身份证号
        /// </summary>
        public const string IdentityNum = @"\d{17}[\d|X]|\d{15}";

        /// <summary>
        /// 邮政编码
        /// </summary>
        public const string Zipcode = @"^[0-9]\d{5}$";

        /// <summary>
        /// 手机号或者邮箱
        /// </summary>
        public const string PhoneOrEmail = @"(^1[0-9]{10}$)|(^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$)";

        /// <summary>
        /// 邮箱
        /// </summary>
        public const string Email = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";


        /// <summary>
        /// 登录名 
        /// </summary>
        public const string LoginName = @"^[a-zA-Z0-9_]{1,30}$";


        /// <summary>
        /// 非负整数（正整数+0）
        /// </summary>
        public const string NoNegativeInt = @"^\d+$";

        /// <summary>
        /// double类型
        /// </summary>
        public const string DoubleNum = @"^(:?(:?\d+.\d+)|(:?\d+))$";

        /// <summary>
        /// 正整数
        /// </summary>
        public const string PositiveInt = @"^[0-9]*[1-9][0-9]*$";

        /// <summary>
        /// 两位小数的正实数
        /// </summary>
        public const string TwoRealNum = @"^[0-9]+(.[0-9]{2})?$";

        /// <summary>
        /// 1\2位小数的正实数
        /// </summary>
        public const string OneTwoRealNum = @"^[0-9]+(.[0-9]{1,2})?$";

        /// <summary>
        /// 固定电话
        /// </summary>
        public const string TelePhone = @"^\d{3,4}-\d{7,8}(-\d{3,4})?$";

        /// <summary>
        /// 除特殊字符外
        /// </summary>
        public const string ChineseWord = @"^[\u4e00-\u9fa5a]{0,}$";

        public const string QQ = @"^[1-9]\d{4,12}$";

        /// <summary>
        /// 网址，以http或https开头
        /// </summary>
        public const string Url = @"^(http|https)://([\w-]+\.)+[\w-]+([\w- ./?%&=#]*)?";

        /// <summary>
        /// 经度
        /// </summary>
        public const string Lng = @"^(-?(?:1[0-7]|[1-9])?\d(?:\.\d{1,8})?|180(?:\.0{1,8})?)$";

        /// <summary>
        /// 纬度
        /// </summary>
        public const string Lat = @"^(-?[1-8]?\d(?:\.\d{1,8})?|90(?:\.0{1,8})?)$";
        #endregion


        #region methods
        public static bool IsMatch(string value, string regex)
        {
            return !string.IsNullOrEmpty(value) && Regex.IsMatch(value, regex);
        }
        /// <summary>
        /// 是否是手机号
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool IsPhone(string phone)
        {
            return IsMatch(phone, Phone);
        }
        /// <summary>
        /// 是否是邮箱
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsEmail(string email)
        {
            return IsMatch(email, Email);
        }

        public static bool IsIDCard(string idCard)
        {
            return IsMatch(idCard, IdentityNum);
        }

        public static bool IsNumeric(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
        }

        public static bool IsUrl(string url)
        {
            return Regex.IsMatch(url, Url);
        }
        #endregion

    }
}
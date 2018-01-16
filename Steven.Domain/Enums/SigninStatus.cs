using System.ComponentModel;

namespace Steven.Domain.Enums
{
    /// <summary>
    /// 登录状态
    /// </summary>
    public enum SigninStatus
    {
        [Description("登录失败")]
        Failed = 0,
        [Description("登录成功")]
        Succ = 1,
        [Description("该用户已停用")]
        Disabled = 2,
        [Description("找不到该用户")]
        UserNotFound = 3,
        [Description("密码不正确")]
        PasswordIncorrent = 4,
        [Description("用户未激活")]
        Inactive = 5
    }
}

using System.ComponentModel;

namespace Steven.Domain.Enums
{
    public enum UserJoinWay
    {
        [Description("网页注册")]
        Website = 0,
        [Description("管理后台录入")]
        Backoffice = 1,
        [Description("微信注册")]
        WeiXin = 2,
        [Description("Facebook")]
        Facebook = 3,
        [Description("QQ")]
        QQ = 4,
        [Description("快捷注册")]
        ShortCut,
        [Description("App")]
        App
    }

    public enum UserGroup
    {
        [Description("管理员")]
        Admin = 0,
        [Description("会员")]
        Member = 1,
        [Description("代理")]
        Agent = 2,
        [Description("商户")]
        Shop = 3
    }

    public enum UserPaltForm
    {
        [Description("暂无")]
        None = 0,
        [Description("IOS")]
        Ios = 1,
        [Description("安卓")]
        Andorid = 2,
        [Description("小程序")]
        WeApp = 3
    }
}

using System.ComponentModel;

namespace Steven.Domain.Enums
{
    public enum AdPosKey
    {
        [Description("首页右则图片轮播")]
        HomeImageCarousel = 1,
        [Description("首页右则视频轮播")]
        HomeVideoCarousel = 2,
        [Description("招商招展广告位")]
        HomeInvestmentBreeze = 3,
        [Description("英文版首页图片轮播")]
        HomeInformationEn = 4
    }
}

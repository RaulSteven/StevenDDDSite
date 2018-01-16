using System.ComponentModel;

namespace Steven.Domain.Enums
{
    /// <summary>
    /// 配置分类
    /// </summary>
    public enum SysConfigClassify
    {
        [Description("无")]
        None = 0,
        [Description("网站配置")]
        Website = 1,
        [Description("统计配置")]
        Statistics = 2,
        [Description("上传配置")]
        Upload = 3,
        [Description("首页配置")]
        Home = 4
    }
}

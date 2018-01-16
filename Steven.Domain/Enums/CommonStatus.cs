using System.ComponentModel;

namespace Steven.Domain.Enums
{
    public enum CommonStatus
    {
        [Description("垃圾箱")]
        Deleted = -1,
        [Description("暂存")]
        Disabled = 0,
        [Description("发布")]
        Enabled = 1
    }
    public enum JobTaskStatus
    {
        [Description("删除")]
        Deleted = -1,
        [Description("停止")]
        Disabled = 0,
        [Description("运行")]
        Enabled = 1
    }
}

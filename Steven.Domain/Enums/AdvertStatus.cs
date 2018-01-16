using System.ComponentModel;

namespace Steven.Domain.Enums
{
    public enum AdvertStatus
    {
        [Description("草稿")]
        Draft = 0,
        [Description("发布")]
        Publish = 1,
        [Description("回收站")]
        Trash = 2
    }

}
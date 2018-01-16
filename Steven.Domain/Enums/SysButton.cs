using System;
using System.ComponentModel;

namespace Steven.Domain.Enums
{
    [Flags]
    public enum SysButton
    {
        [Description("无")]
        None = 0,
        [Description("浏览")]
        Browse = 1,
        [Description("添加")]
        Add = 2,
        [Description("编辑")]
        Edit = 4,
        [Description("删除")]
        Delete = 8,
        [Description("查询")]
        Search = 16,
        [Description("刷新")]
        Refresh = 32,
        [Description("导出")]
        Export = 64,
        [Description("下载")]
        Download = 128,
        [Description("打印")]
        Print = 256,
        [Description("审核")]
        Auditing = 512,
        [Description("导入")]
        Leadingin = 1024,
        [Description("复制")]
        Copy = 2048,
        [Description("保存")]
        Save = 4096,
        [Description("授权")]
        Grant = 8192
    }
}

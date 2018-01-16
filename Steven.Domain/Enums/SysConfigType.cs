using System.ComponentModel;

namespace Steven.Domain.Enums
{
    public enum SysConfigType
    {
        [Description("字符")]
        String = 0,
        [Description("布尔")]
        Bool = 1,
        [Description("文本")]
        TextArea = 2,
        [Description("Int")]
        Int = 3,
        [Description("Long")]
        Long = 4,
        [Description("字符数组")]
        StringArray = 5
    }
}

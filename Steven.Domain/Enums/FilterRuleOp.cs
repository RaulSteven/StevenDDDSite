using System.ComponentModel;

namespace Steven.Domain.Enums
{
    public enum FilterRuleOp
    {
        [Description("等于")]
        Equal = 0,
        [Description("大于")]
        Greater = 1,
        [Description("大于等于")]
        GreaterOrEqual = 2,
        [Description("小于")]
        Less = 3,
        [Description("小于等于")]
        LessOrEqual = 4,
        [Description("相似")]
        Like = 5,
        [Description("以xx开头")]
        StartWith = 6,
        [Description("以xx结尾")]
        EndWith = 7,
        [Description("不等于")]
        NotEqual = 8,
        [Description("包含于")]
        In = 9,
        [Description("不包含于")]
        Notin = 10
    }
}

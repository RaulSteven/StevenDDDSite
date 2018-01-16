using System.ComponentModel;

namespace Steven.Domain.Enums
{
    public enum FilterGroupOp
    {
        [Description("并且")]
        And = 0,
        [Description("或者")]
        Or = 1
    }
}

using System.ComponentModel;

namespace Steven.Domain.Enums
{
    public enum Gender
    {
        [Description("保密")]
        Unknown = 0,
        [Description("男")]
        Male = 1,
        [Description("女")]
        Female = 2
    }
}

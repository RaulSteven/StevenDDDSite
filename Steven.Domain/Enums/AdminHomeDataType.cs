using System.ComponentModel;

namespace Steven.Domain.Enums
{
    public enum AdminHomeDataType
    {
        [Description("今天")]
        Today = 1,
        [Description("这个月")]
        Month,
        [Description("全年")]
        Year
    }
}

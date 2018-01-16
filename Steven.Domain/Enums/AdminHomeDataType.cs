using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

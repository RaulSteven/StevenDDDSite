using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steven.Domain.Enums
{
    public enum ShopStatus
    {
        [Description("已开店")]
        Open = 1,
        [Description("已关店")]
        Closed = 0,
    }
}

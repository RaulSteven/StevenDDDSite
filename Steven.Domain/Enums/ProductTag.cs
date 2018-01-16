using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steven.Domain.Enums
{
    [Flags]
    public enum ProductTag
    {
        [Description("无")]
        None = 0,
        [Description("主推")]
        MainPush = 1,
        [Description("热卖")]
        HotSale = 2,
        [Description("今日新品")]
        TodayNews = 4,
    }
}

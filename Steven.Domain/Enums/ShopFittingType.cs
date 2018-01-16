using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steven.Domain.Enums
{
    [Flags]
    public enum ShopFittingType
    {
        [Description("主推")]
        MainPush = 1,
        [Description("商品分组")]
        ProductClassify = 2,
        [Description("热卖")]
        HotSale = 3,
        [Description("今日新品")]
        TodayNews = 4,
        [Description("店铺展示")]
        Show = 5,
    }
}

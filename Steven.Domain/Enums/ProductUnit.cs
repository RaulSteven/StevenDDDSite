using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steven.Domain.Enums
{
    public enum ProductUnit
    {
        [Description("个")]
        Ge = 1,
        [Description("千克")]
        Kg = 2,
        [Description("张")]
        Zhang = 3,
        [Description("包")]
        Bao = 4,
        [Description("只")]
        Zhi = 5,
        [Description("瓶")]
        Ping = 6,
        [Description("件")]
        Jian = 7,
        [Description("箱")]
        Xiang = 8,
        [Description("盒")]
        He = 9,
        [Description("双")]
        Shuang = 10,
        [Description("把")]
        Ba = 11,
        [Description("套")]
        Tao = 12,
        [Description("斤")]
        Jin = 13
    }
}

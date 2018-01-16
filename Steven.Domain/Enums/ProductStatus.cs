using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steven.Domain.Enums
{
    public enum ProductStatus
    {
        [Description("上架")]
        OnSale = 1,
        [Description("下架")]
        UnSale = 2,
    }
}

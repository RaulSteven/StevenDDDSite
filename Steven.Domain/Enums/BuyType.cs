using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steven.Domain.Enums
{
    public enum BuyType
    {
        [Description("配送")]
        Delivery = 1,
        [Description("到店")]
        Arrival = 2,
    }
}

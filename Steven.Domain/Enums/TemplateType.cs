using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steven.Domain.Enums
{
    public enum TemplateType
    {
        [Description("订单待支付通知")]
        WaitPay = 1,
        [Description("订单支付成功通知")]
        Payed = 2,
        [Description("订单发货通知")]
        Send = 3,
        [Description("订单收货通知")]
        Take = 4,
        [Description("订单取消通知")]
        Closed = 5,
        [Description("用户下单通知")]
        UserOrderDown = 6,
        [Description("用户支付通知")]
        UserPay = 7,
        [Description("用户签收通知")]
        UserTake = 8
    }
}

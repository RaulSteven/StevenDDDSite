using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steven.Domain.Enums
{
    public enum OrderStatus
    {
        [Description("待付款")]
        UnPay = 0,
        [Description("已付款")]//用于到店购买方式
        Paid = 7,
        [Description("待接单")]
        Waiting = 1,
        [Description("待发货")]
        ShipmentPending = 2,
        [Description("已发货")]
        Consign = 3,
        [Description("已完成")]
        Completed = 4,
        [Description("已取消")]
        Closed = 5,
        [Description("退款中")]
        Refund = 6, 
    }

    public enum MemberOrderStatus
    {
        [Description("待支付")]
        WaitPay = 1,
        [Description("待使用")]//用于到店购买方式
        WaitUse = 6,
        [Description("待配送")]
        WaitPending = 2,
        [Description("已配送")]
        Pended = 3,
        [Description("已完成")]
        Completed = 4,
        [Description("已取消")]
        Closed = 5,
        
    }

    public enum PaymentStatus
    {
        [Description("未支付")]
        Unpay = 0,
        [Description("已支付")]
        Payed = 1
    }
    public enum PayType
    {
        [Description("微信支付")]
        Weixin = 0,
        [Description("商家收款")]
        Gathering = 1
    }
    public enum DeliveryType
    {
        [Description("暂无")]
        None = 0,
        [Description("商家自送")]
        Shop = 1,
        [Description("快递配送")]
       Express = 2
    }

}

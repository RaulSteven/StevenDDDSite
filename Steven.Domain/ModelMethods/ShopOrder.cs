using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.Enums;
using Dapper.Contrib.Extensions;

namespace Steven.Domain.Models
{
    public partial class ShopOrder
    {
        public ShopOrder()
        {
            Products = new List<ShopOrderProduct>();
        }
        [Write(false)]
        public string ShopName { get; set; }

        [Write(false)]
        public string UserName { get; set; }

        [Write(false)]
        public List<ShopOrderProduct> Products { get; set; }

        [Write(false)]
        public int TotalProNum
        {
            get
            {
                return Products.Sum(m => m.Number);
            }
            set { }
        }

        #region 用户订单操作

        [Flags]
        public enum OrderUserAction
        {
            [Description("无")]
            None = 0,
            [Description("去支付")]
            Pay = 1,
            [Description("联系商家")]
            ContactShop = 2,
            [Description("确认收货")]
            ConfirmTake = 4,
            [Description("验证码")]
            ValidCode = 8,
            [Description("配送信息")]
            DeliveryInfo = 16
        }

        [Write(false)]
        public OrderUserAction OrderUserActions
        {
            get { return GetOrderUserAction(this.OrderStatus, this.BuyType); }
        }

        public bool HasUserAction(OrderUserAction action)
        {
            return (this.OrderUserActions & action) == action;
        }

        public static OrderUserAction GetOrderUserAction(OrderStatus status, BuyType buyType)
        {
            var action = OrderUserAction.None;
            if (status == OrderStatus.UnPay)
            {
                action = OrderUserAction.Pay;
            }
            switch (buyType)
            {
                case BuyType.Delivery:
                    //如果订单已经取消，或者订单已经完成，则不能做任何操作
                    if (status == OrderStatus.Closed)
                    {
                        return action;
                    }
                    if (status == OrderStatus.Waiting || status == OrderStatus.ShipmentPending)
                    {
                        action = OrderUserAction.ContactShop;
                    }
                    if (status == OrderStatus.Completed)
                    {
                        action = OrderUserAction.DeliveryInfo | OrderUserAction.ContactShop;
                    }
                    if (status == OrderStatus.Consign)
                    {
                        action = OrderUserAction.ConfirmTake | OrderUserAction.DeliveryInfo | OrderUserAction.ContactShop;
                    }
                    break;
                case BuyType.Arrival:
                    if (status == OrderStatus.Paid)
                    {
                        action = OrderUserAction.ValidCode;
                    }
                    if (status == OrderStatus.Completed || status == OrderStatus.Closed)
                    {
                        action = OrderUserAction.ContactShop;
                    }
                    break;
            }

            return action;
        }

        [Write(false)]
        public MemberOrderStatus MemberOrderStatuss
        {
            get { return GetMemberOrderStatus(this.OrderStatus, this.BuyType); }
        }
        public static MemberOrderStatus GetMemberOrderStatus(OrderStatus status, BuyType buyType)
        {
            MemberOrderStatus result = MemberOrderStatus.Completed;
            switch (buyType)
            {
                case BuyType.Delivery:
                    if (status == OrderStatus.Closed)
                    {
                        return MemberOrderStatus.Closed;
                    }
                    if (status == OrderStatus.UnPay)
                    {
                        return MemberOrderStatus.WaitPay;
                    }
                    if (status == OrderStatus.Waiting || status == OrderStatus.ShipmentPending)
                    {
                        return MemberOrderStatus.WaitPending;
                    }
                    if (status == OrderStatus.Consign)
                    {
                        return MemberOrderStatus.Pended;
                    }
                    break;
                case BuyType.Arrival:
                    if (status == OrderStatus.Closed)
                    {
                        return MemberOrderStatus.Closed;
                    }
                    if (status == OrderStatus.UnPay)
                    {
                        return MemberOrderStatus.WaitPay;
                    }
                    if (status == OrderStatus.Paid)
                    {
                        return MemberOrderStatus.WaitUse;
                    }
                    break;
            }

            return result;
        }
        #endregion

        #region 商家订单操作

        [Flags]
        public enum OrderShopAction
        {
            [Description("无")]
            None = 0,
            [Description("已付款")]
            Pay = 1,
            [Description("接单")]
            Take = 2,
            [Description("发货")]
            Sended = 4,
            [Description("已签收")]
            Signed = 8,
            [Description("完成")]
            Completed = 16,
            [Description("取消订单")]
            Closed = 32,
            [Description("验证")]
            Valid = 64,
            [Description("配送信息")]
            DeliveryType = 128
        }

        [Write(false)]
        public OrderShopAction OrderShopActions
        {
            get { return GetOrderShopAction(this.OrderStatus, this.BuyType); }
        }

        public bool HasShopAction(OrderShopAction action)
        {
            return (this.OrderShopActions & action) == action;
        }

        public static OrderShopAction GetOrderShopAction(OrderStatus status, BuyType buyType)
        {
            var action = OrderShopAction.None;
            switch (buyType)
            {
                case BuyType.Delivery:
                    if (status == OrderStatus.UnPay)
                    {
                        action = OrderShopAction.Pay | OrderShopAction.Closed;
                    }
                    if (status == OrderStatus.Waiting)
                    {
                        action = OrderShopAction.Take | OrderShopAction.Closed;
                    }
                    if (status == OrderStatus.ShipmentPending)
                    {
                        action = OrderShopAction.Sended;
                    }
                    if (status == OrderStatus.Consign)
                    {
                        action = OrderShopAction.Signed | OrderShopAction.DeliveryType;
                    }
                    if (status == OrderStatus.Completed)
                    {
                        action = OrderShopAction.DeliveryType;
                    }
                    break;
                case BuyType.Arrival:
                    if (status == OrderStatus.UnPay)
                    {
                        action = OrderShopAction.Closed;
                    }
                    if (status == OrderStatus.Paid)
                    {
                        action = OrderShopAction.Valid | OrderShopAction.Closed;
                    }
                    break;
            }
            return action;
        }
        #endregion
    }
}

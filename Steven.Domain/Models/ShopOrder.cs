using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using Steven.Domain.Infrastructure;
using Steven.Domain.Enums;

namespace Steven.Domain.Models
{
    [Table("ShopOrder")]
    public partial class ShopOrder:AggregateRoot
    {
        public long ShopId { get; set; }
        public long UserId { get; set; }
        public string Code { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? PayTime { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public string Receiver { get; set; }
        public string Phone { get; set; }
        public string ShippingAddress { get; set; }

        public string LeaveMsg { get; set; }
        public decimal TotalPrice { get; set; }

        public DateTime? DeliveredTime { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

        public string PrePayId { get; set; }

        public PayType PayType { get; set; }

        public string ValidCode { get; set; }

        public BuyType BuyType { get; set; }

        public DeliveryType DeliveryType { get; set; }

        public long ExpressId { get; set; }

        public string ExpressName { get; set; }

        public string ExpressCode { get; set; }
    }
}

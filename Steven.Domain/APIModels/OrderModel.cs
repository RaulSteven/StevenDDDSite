using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.Enums;
using Steven.Domain.Models;

namespace Steven.Domain.APIModels
{
    [DataContract]
    public class OrderModel
    {
        [DataMember]
        public long OrderId { get; set; }

        public long ImgId { get; set; }
        [DataMember]
        public string Img { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public decimal TotalPrice { get; set; }

        [DataMember]
        public decimal TotalProNum { get; set; }

        public DateTime DTime { get; set; }
        [DataMember]
        public string OrderTime { get; set; }

        public OrderStatus OrderStatus { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public MemberOrderStatus StatusId { get; set; }
        [DataMember]
        public BuyType BuyType { get; set; }
    }
    [DataContract]
    public class OrderDetailModel : ApiResultBase
    {
        public OrderDetailModel()
        {
            List = new List<ConfirmOrderProModel>();
        }

        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public MemberOrderStatus StatusId { get; set; }

        [DataMember]
        public long OrderId { get; set; }

        [DataMember]
        public string OrderCode { get; set; }

        [DataMember]
        public List<ConfirmOrderProModel> List { get; set; }

        [DataMember]
        public decimal TotalPrice { get; set; }

        [DataMember]
        public string Receiver { get; set; }

        [DataMember]
        public string Phone { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string OrderTime { get; set; }

        [DataMember]
        public string DeliveredTime { get; set; }

        [DataMember]
        public string LeaveMsg { get; set; }

        [DataMember]
        public BuyType BuyType { get; set; }
    }
}

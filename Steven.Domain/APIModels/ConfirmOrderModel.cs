using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.Enums;
using Steven.Domain.ViewModels;

namespace Steven.Domain.APIModels
{
    [DataContract]
    public class ConfirmOrderModel : ApiResultBase
    {
        public ConfirmOrderModel()
        {
            List = new List<ConfirmOrderProModel>();
            BuyTypeList = new List<BuyTypeBizModel>();
        }

        [DataMember]
        public string Receiver { get; set; }

        [DataMember]
        public string Phone { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public decimal TotalPrice { get; set; }

        [DataMember]
        public List<ConfirmOrderProModel> List { get; set; }

        [DataMember]
        public string LeaveMsg { get; set; }

        public string FormId { get; set; }

        [DataMember]
        public long ProductId { get; set; }

        [DataMember]
        public List<BuyTypeBizModel> BuyTypeList { get; set; }

        [DataMember]
        public int BuyType { get; set; }
    }
    [DataContract]
    public class ConfirmOrderProModel
    {
        [DataMember]
        public long ProductId { get; set; }

        [DataMember]
        public long ImgId { get; set; }
        [DataMember]
        public string Img { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Unit { get; set; }

        [DataMember]
        public decimal Price { get; set; }

        [DataMember]
        public int Number { get; set; }
    }
    public enum ConfirmOrderType
    {
        [Description("购物车购买")]
        Cart = 0,
        [Description("立即购买")]
        Now = 1
    }
}

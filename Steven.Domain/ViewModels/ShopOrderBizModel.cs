using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.Enums;

namespace Steven.Domain.ViewModels
{
    public class ShopOrderBizModel
    {
        public ShopOrderBizModel()
        {
            List=new List<ShopOrderProBizModel>();
        }
        public long OrderId { get; set; }

        public string OrderCode { get; set; }

        public DateTime OrderTime { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public string Receiver { get; set; }

        public string Phone { get; set; }

        public string ShippingAddress { get; set; }
        
        public decimal TotalPrice { get; set; }
        
        public decimal TotalProNum { get; set; }

        public string LeaveMsg { get; set; }

        public List<ShopOrderProBizModel> List { get; set; }
    }

    public class ShopOrderProBizModel
    {
        public long ImgId { get; set; }
        public string Img { get; set; }
        
        public string Name { get; set; }
        
        public decimal Price { get; set; }
        
        public int Number { get; set; }
    }
}

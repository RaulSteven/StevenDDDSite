using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Steven.Domain.Enums;
using Steven.Domain.Models;

namespace Steven.Web.Areas.Shop.Models
{
    public class OrderIndexModel
    {
        public string KeyWord { get; set; }
        public OrderStatus? CurrStatus { get; set; }
        public DateTime? CurrStartTime { get; set; }
        public DateTime? CurrEndTime { get; set; }

        public bool IsHaveOrder { get; set; }

        public BuyType Type { get; set; }
        public List<ShopBuyWay> TypeList { get; set; }

        public List<SelectListItem> ExpressList { get; set; }
    }
}
using Steven.Domain.Enums;
using Steven.Domain.Models;
using Steven.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Steven.Web.Areas.Shop.Models
{
    public class HomeIndexModel
    {
        public decimal TodayTotalOrderPrice { get; set; }
        public int TotalPayedOrderCount { get; set; }
        public int TodayViewCount { get; set; }
        public ShopStatus Status { get; set; }
        public int WaitingCount { get; set; }
        public int ShipmentPendingCount { get; set; }
        public IEnumerable<ArticleSimpleModel> LstArticle { get; set; }

        public string JsonTime { get; set; }
        public string JsonCreateOrderCount { get; set; }
        public string JsonPayedOrderCount { get; set; }
    }
}
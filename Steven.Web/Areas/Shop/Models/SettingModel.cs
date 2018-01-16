using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Steven.Web.Areas.Shop.Models
{
    public class SettingModel
    {
        public long LogoAttaId { get; set; }
        public string KefuPhone { get; set; }
        public string Descript { get; set; }
        public string Name { get; set; }

        public long BgAttaId { get; set; }
        public string Address { get; set; }
        //经度
        public double Lng { get; set; }
        //纬度
        public double Lat { get; set; }
        
        //配送
        public bool IsDelivery { get; set; }

        //到店
        public bool IsArrival { get; set; }
    }
}
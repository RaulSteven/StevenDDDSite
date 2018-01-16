using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Steven.Web.Areas.Admin.Models
{
    public class ShopAppInfoModel
    {
        public long Id { get; set; }
        public long ShopId { get; set; }

        public string BeiLinAppId { get; set; }
        public string BeiLinAppSecrect { get; set; }
        public string WxAppId { get; set; }
        public string WxAppSecrect { get; set; }
        public string WxMchId { get; set; }
        public string WxKey { get; set; }
    }
}
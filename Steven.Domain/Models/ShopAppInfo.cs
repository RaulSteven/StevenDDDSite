using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.Infrastructure;
using Dapper.Contrib.Extensions;
namespace Steven.Domain.Models
{
    [Table("ShopAppInfo")]
    public class ShopAppInfo : AggregateRoot
    {
        public long ShopId { get; set; }
        public string BeiLinAppId { get; set; }
        public string BeiLinAppSecrect { get; set; }
        public string WxAppId { get; set; }
        public string WxAppSecrect { get; set; }
        public string WxMchId { get; set; }
        public string WxKey { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.Enums;
using Steven.Domain.Infrastructure;
using Dapper.Contrib.Extensions;

namespace Steven.Domain.Models
{
    [Table("ShopBuyWay")]
    public class ShopBuyWay : AggregateRoot
    {
        public long ShopId { get; set; }
        public BuyType Type { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.Infrastructure;
using Dapper.Contrib.Extensions;

namespace Steven.Domain.Models
{
    [Table("ShopViewRecord")]
    public class ShopViewRecord:AggregateRoot
    {
        public long ShopId { get; set; }
        public string PageName { get; set; }
    }
}

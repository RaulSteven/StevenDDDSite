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
    [Table("ShopOrderProduct")]
    public partial class ShopOrderProduct:AggregateRoot
    {
        public long OrderId { get; set; }
        public long ProductId { get; set; }
        public long ProductPicAttId { get; set; }
        public string ProductName { get; set; }
        public string ProductUnit { get; set; }
        public int Number { get; set; }
        public decimal Price { get; set; }
    }
}

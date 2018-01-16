using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using Steven.Domain.Infrastructure;

namespace Steven.Domain.Models
{
    [Table("ShoppingCart")]
    public class ShoppingCart:AggregateRoot
    {
        public long UserId { get; set; }
        public long ProductId { get; set; }
        public int Number { get; set; }
        public long ShopId { get; set; }
        public bool IsChoose { get; set; }
    }
}

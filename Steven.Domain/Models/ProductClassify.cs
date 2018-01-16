using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using Steven.Domain.Infrastructure;
namespace Steven.Domain.Models
{
    [Table("ProductClassify")]
    public class ProductClassify: AggregateRoot
    {
        public long ShopId { get; set; }
        public string Name { get; set; }
        public int Sort { get; set; }
        public long IconAttaId { get; set; }
        public int ProductNumber { get; set; }
    }
}

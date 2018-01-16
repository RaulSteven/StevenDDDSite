using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.Infrastructure;

namespace Steven.Domain.Models
{
    [Table("ProductSpecs")]
    public class ProductSpecs : AggregateRoot
    {
        public long ShopId { get; set; }
        public long ProductId { get; set; }
        public string Name { get; set; }
        public string SpecsValue { get; set; }

    }
}

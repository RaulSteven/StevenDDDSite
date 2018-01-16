using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.Enums;
using Dapper.Contrib.Extensions;
using Steven.Domain.Infrastructure;

namespace Steven.Domain.Models
{
    [Table("Product")]
    public partial class Product: AggregateRoot
    {
        public long ShopId { get; set; }
        public string PicAttIds { get; set; }
        public string Title { get; set; }
        public string Unit { get; set; }
        public ProductStatus Status { get; set; }
        public decimal Price { get; set; }
        public decimal OldPrice { get; set; }
        public int Stock { get; set; }
        public long ProductClassifyId { get; set; }
        public string Descript { get; set; }
        public int Sort { get; set; }
        public ProductTag Tag { get; set; }
    }
}

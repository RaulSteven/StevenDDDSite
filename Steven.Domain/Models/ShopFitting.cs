using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.Infrastructure;
using Dapper.Contrib.Extensions;
using Steven.Domain.Enums;
namespace Steven.Domain.Models
{
    [Table("ShopFitting")]
    public class ShopFitting : AggregateRoot
    {
        public long ShopId { get; set; }
        public ShopFittingType FittingType { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public bool HasSelected { get; set; }
        public string JsonData { get; set; }

    }
}

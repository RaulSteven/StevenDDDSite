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
    [Table("ShopTemplate")]
    public class ShopTemplate: AggregateRoot
    {
        public long ShopId { get; set; }
        public string Name { get; set; }
        public TemplateType TemplateType { get; set; }
        public string TemplateId { get; set; }
        public bool IsUsed { get; set; }
    }
}

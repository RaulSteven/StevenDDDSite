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
    [Table("UserHistory")]
    public class UserHistory : AggregateRoot
    {
        public long ShopId { get; set; }
        public long UserId { get; set; }
        public TableSource Source { get; set; }
        public long SourceId { get; set; }
    }
}

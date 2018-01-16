using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using Steven.Domain.Infrastructure;
    
namespace Steven.Domain.Models
{
    [Table("Agent")]
    public partial class Agent:AggregateRoot
    {
        public string Name { get; set; }

        public string Descript { get; set; }
        public long UserId { get; set; }

    }
}

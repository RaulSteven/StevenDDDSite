using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using Steven.Domain.Infrastructure;

namespace Steven.Domain.Models
{
    [Table("SysUnit")]
    public class SysUnit : AggregateRoot
    {
        public string Name { get; set; }
        public int Sort { get; set; }
    }
}

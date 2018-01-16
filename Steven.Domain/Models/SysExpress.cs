using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.Infrastructure;
using Dapper.Contrib.Extensions;

namespace Steven.Domain.Models
{
    [Table("SysExpress")]
    public class SysExpress : AggregateRoot
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int Sort { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.Infrastructure;
using Dapper.Contrib.Extensions;
namespace Steven.Domain.Models
{
    [Table("SysCase")]
    public class SysCase:AggregateRoot
    {
        public string Name { get; set; }

        public long PicAttaId { get; set; }
        public long QrAttaId { get; set; }

        public string Brief { get; set; }

        public string CaseUrl { get; set; }

        public int Sort { get; set; }
    }
}

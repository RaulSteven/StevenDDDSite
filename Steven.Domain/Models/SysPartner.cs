using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.Infrastructure;
using Dapper.Contrib.Extensions;

namespace Steven.Domain.Models
{
    [Table("SysPartner")]
    public class SysPartner : AggregateRoot
    {
        public string Name { get; set; }

        public long PicAttaId { get; set; }

        public string PartnerUrl { get; set; }

        public int Sort { get; set; }
    }
}

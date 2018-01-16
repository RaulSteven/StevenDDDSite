using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.Infrastructure;
using Steven.Domain.Enums;

namespace Steven.Domain.Models
{
    [Table("Shop")]
    public partial class Shop:AggregateRoot
    {
        public long UserId { get; set; }
        public long LogoAttaId { get; set; }
        public long AgentId { get; set; }
        public string KefuPhone { get; set; }
        public string Descript { get; set; }
        public string Name { get; set; }

        public long BgAttaId { get; set; }
        public ShopStatus Status { get; set; }
        public long QrCodeAttaId { get; set; }
        public int ProLimitNum { get; set; }
        public int ClsLimitNum { get; set; }
        public string Address { get; set; }
        //经度
        public double Lng { get; set; }
        //纬度
        public double Lat { get; set; }
        public string Others { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.Infrastructure;
using Dapper.Contrib.Extensions;

namespace Steven.Domain.Models
{
    [Table("UserShipingAddress")]
    public class UserShipingAddress:AggregateRoot
    {
        public long UserId { get; set; }
        public string Receiver { get; set; }
        public string Phone { get; set; }
        public string ShippingAddress { get; set; }
        public bool IsDefault { get; set; }
    }
}

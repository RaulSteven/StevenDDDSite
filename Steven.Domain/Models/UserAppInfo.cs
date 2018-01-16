using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.Infrastructure;
using Dapper.Contrib.Extensions;

namespace Steven.Domain.Models
{
    [Table("UserAppInfo")]
    public class UserAppInfo : AggregateRoot
    {
        public long UserId { get; set; }

        public long ShopId { get; set; }

        public string WxAppId { get; set; }

        public string WxAppSecrect { get; set; }

        public string SessionKey { get; set; }

        public string OpenId { get; set; }

        public string UnionId { get; set; }

        public string City { get; set; }

        public string Province { get; set; }

        public string Country { get; set; }
    }
}

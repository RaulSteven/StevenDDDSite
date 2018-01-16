using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;

namespace Steven.Domain.Models
{
    public partial class ShopOrderProduct
    {
        [Write(false)]
        public string ProductPic { get; set; }
    }
}

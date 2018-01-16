using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Core.Utilities;
using Steven.Domain.Services;

namespace Steven.Domain.Models
{
    public partial class Product
    {
        [Write(false)]
        public string ShopName { get; set; }

        [Write(false)]
        public long CoverImgId
        {
            get
            {
                return ProductImg.GetCoverImgId(PicAttIds);                 
            }
        }
    }
}

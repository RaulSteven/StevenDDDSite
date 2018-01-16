using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Steven.Domain.Models;

namespace Steven.Web.Areas.Shop.Models
{
    public class ProductIndexModel
    {
        public IEnumerable<ProductClassify> LstClassify { get; set; }
        public long CurrClzId { get; set; }
    }
}
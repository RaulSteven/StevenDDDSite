using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Steven.Web.Areas.Admin.Models
{
    public class ProductClassifyModel
    {
        public long Id { get; set; }
        public long ShopId { get; set; }
        public string Name { get; set; }
        public int Sort { get; set; }
        public long IconAttaId { get; set; }

        public string IconUrl { get; set; }
    }
}
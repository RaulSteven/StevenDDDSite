using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Steven.Web.Areas.Admin.Models
{
    public class SysPartnerModel
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public long PicAttaId { get; set; }

        public string PartnerUrl { get; set; }

        public int Sort { get; set; }
    }
}
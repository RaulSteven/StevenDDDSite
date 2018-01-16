using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Steven.Web.Areas.Admin.Models
{
    public class SysCaseModel
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public long PicAttaId { get; set; }
        public long QrAttaId { get; set; }

        public string Brief { get; set; }

        public string CaseUrl { get; set; }

        public int Sort { get; set; }
    }
}
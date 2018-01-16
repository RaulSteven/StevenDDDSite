using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Steven.Web.Areas.Admin.Models
{
    public class SysExpressModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public int Sort { get; set; }
    }
}
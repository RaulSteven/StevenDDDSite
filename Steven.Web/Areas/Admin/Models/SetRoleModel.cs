using Steven.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Steven.Web.Areas.Admin.Models
{
    public class SetRoleModel
    {
        public string UserIds { get; set; }
        public long[] RoleIds { get; set; }
    }
}
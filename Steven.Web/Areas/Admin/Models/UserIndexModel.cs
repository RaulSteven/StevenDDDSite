using Steven.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Steven.Web.Areas.Admin.Models
{
    public class UserIndexModel
    {
        public IEnumerable<UserRole> UserRoleList { get; set; }
    }
}
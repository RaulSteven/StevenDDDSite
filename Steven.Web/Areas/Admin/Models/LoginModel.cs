using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Steven.Web.Areas.Admin.Models
{
    public class LoginModel
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string ValidateCode { get; set; }

        public bool IsRemember { get; set; }

    }
}
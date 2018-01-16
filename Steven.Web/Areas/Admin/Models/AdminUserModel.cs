using Steven.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Steven.Web.Areas.Admin.Models
{
    public class AdminUserModel:BaseViewModel
    {
        public long Id { get; set; }

        public string LoginName
        {
            get;
            set;
        }

        public string RealName
        {
            get;
            set;
        }

        public string Password { get; set; }
        public long HeadImageId { get; set; }
        public Gender Gender { get; set; }
        public CommonStatus CommonStatus { get; set; }

    }
}
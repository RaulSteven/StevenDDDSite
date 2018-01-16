using Steven.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Steven.Web.Areas.Admin.Models
{
    public class SysConfigModel
    {
        public long Id { get; set; }

        public string ConKey { get; set; }

        public string ConValue { get; set; }

        public string Name { get; set; }

        public SysConfigType ConfigType { get; set; }

        public SysConfigClassify ConfigClassify { get; set; }

        public string Remark { get; set; }

        #region 内容值
        public string StringValue { get; set; }
        public bool BoolValue { get; set; }
        public string TextAreaValue { get; set; }
        public int IntValue { get; set; }
        public long LongValue { get; set; }
        public string StringArrayValue { get; set; }
        public string ImageArrayValue { get; set; }

        #endregion
    }
}
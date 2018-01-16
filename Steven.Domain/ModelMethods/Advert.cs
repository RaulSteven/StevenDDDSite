using System;
using Steven.Domain.Enums;
using Dapper.Contrib.Extensions;

namespace Steven.Domain.Models
{
    public partial class Advert
    {
        #region methods

        public bool IsNormal()
        {
            var isNormal = CommonStatus == CommonStatus.Enabled
                           && StartTime <= DateTime.Now
                           && (EndTime == null || EndTime >= DateTime.Now);
            return isNormal;
        }

        public string GetTarget()
        {
            return "_" + Target.ToString().ToLower();
        }

        #endregion

        [Write(false)]
        public string ImageUrl { get; set; }
        [Write(false)]
        public string AdPosSize { get; set; }

        [Write(false)]
        public AdPosKey Code { get; set; }

        [Write(false)]
        public string AdPosName { get; set; }
    }
}

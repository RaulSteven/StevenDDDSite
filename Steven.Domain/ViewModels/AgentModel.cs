using Steven.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Steven.Domain.ViewModels
{
    public class AgentModel
    {
        #region Agent信息
        public long Id { get; set; }

        public string Name { get; set; }
        public string Descript { get; set; }

        public long UserId { get; set; }

        #endregion

        #region 用户信息
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
        public CommonStatus CommonStatus { get; set; }
        public DateTime UpdateTime { get; set; }
        public int LoginCount { get; set; }
        #endregion



    }
}
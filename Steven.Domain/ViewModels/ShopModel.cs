using Steven.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steven.Domain.ViewModels
{
    public class ShopModel
    {
        public ShopModel()
        {
            ProLimitNum = 30;
            ClsLimitNum = 4;
        }
        #region 商铺信息
        public long Id { get; set; }


        public string Name { get; set; }
        public string Descript { get; set; }

        public long UserId { get; set; }
        public long LogoAttaId { get; set; }
        public long AgentId { get; set; }
        public string KefuPhone { get; set; }

        public string AgentName { get; set; }
        public long BgAttaId { get; set; }
        public ShopStatus Status { get; set; }
        public long QrCodeAttaId { get; set; }
        public int ProLimitNum { get; set; }
        public int ClsLimitNum { get; set; }
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

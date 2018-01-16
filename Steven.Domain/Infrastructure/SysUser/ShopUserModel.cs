using Steven.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Steven.Domain.Infrastructure.SysUser
{
    [Serializable]
    public class ShopUserModel : ISysUserModel
    {
        #region Properties
        public string GId
        {
            get;
            set;
        }

        public long HeadImageId
        {
            get;
            set;
        }

        public long UserId
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }

        public long LogoAttaId { get; set; }
        public string KefuPhone { get; set; }
        public string Name { get; set; }
        public long BgAttaId { get; set; }
        public long ShopId { get; set; }
        #endregion

        public ShopUserModel()
        {

        }
        public ShopUserModel(Users user,Shop shop)
        {
            this.GId = user.GId;
            this.UserId = user.Id;
            this.UserName = user.LoginName;
            this.HeadImageId = user.HeadImageId;

            this.BgAttaId = shop.BgAttaId;
            this.LogoAttaId = shop.LogoAttaId;
            this.Name = shop.Name;
            this.KefuPhone = shop.KefuPhone;
            this.ShopId = shop.Id;
        }

    }
}
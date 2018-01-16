using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Steven.Domain.Models;
namespace Steven.Domain.Infrastructure.SysUser
{
    [Serializable]
    public class MemberUserModel : ISysUserModel
    {
        #region  properties
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

        #endregion

        public MemberUserModel()
        {

        }
        public MemberUserModel(Users user)
        {
            this.GId = user.GId;
            this.UserId = user.Id;
            this.UserName = user.RealName??user.LoginName;
            this.HeadImageId = user.HeadImageId;
        }
    }
}
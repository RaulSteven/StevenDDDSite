using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace Steven.Domain.Infrastructure.SysUser
{
    public class ShopUser : ISysUser
    {
        public IIdentity Identity
        {
            get;
            private set;
        }

        public ShopUserModel UserModel
        {
            get;
            private set;
        }

        ISysUserModel ISysUser.UserModel
        {
            get
            {
                return UserModel;
            }
        }

        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }

        public ShopUser()
        {
            Identity = new GenericIdentity("");
        }

        public ShopUser(ShopUserModel model)
        {
            UserModel = model;
            Identity = new GenericIdentity(model.UserName);
        }
    }
}
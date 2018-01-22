using Steven.Domain.Enums;
using System;
using System.Security.Principal;

namespace Steven.Domain.Infrastructure.SysUser
{
    public class AdminUser : ISysUser
    {
        public IIdentity Identity
        {
            get;
            private set;
        }

        public AdminUserModel UserModel
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

        public AdminUser()
        {
            Identity = new GenericIdentity("");
        }

        public AdminUser(AdminUserModel model)
        {
            UserModel = model;
            Identity = new GenericIdentity(model.UserName);
        }

        public bool HasButton(SysButton btn)
        {
            if (UserModel == null || UserModel.FirstMenu == null)
            {
                return false;
            }

            return (UserModel.FirstMenu.Buttons & btn) == btn;
        }
    }
}
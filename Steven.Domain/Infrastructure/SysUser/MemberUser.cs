using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace Steven.Domain.Infrastructure.SysUser
{
    public class MemberUser : ISysUser
    {
        public IIdentity Identity
        {
            get;
            private set;
        }

        public MemberUserModel UserModel
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

        public MemberUser()
        {
            Identity = new GenericIdentity("");
            UserModel=new MemberUserModel();
        }

        public MemberUser(MemberUserModel model)
        {
            UserModel = model;
            Identity = new GenericIdentity(model.UserName);
        }
    }
}
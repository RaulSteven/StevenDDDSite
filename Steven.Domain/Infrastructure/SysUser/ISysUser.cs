using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace Steven.Domain.Infrastructure.SysUser
{
    public interface ISysUser:IPrincipal
    {
        ISysUserModel UserModel { get; }
    }
}
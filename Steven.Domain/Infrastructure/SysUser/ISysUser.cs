using System.Security.Principal;

namespace Steven.Domain.Infrastructure.SysUser
{
    public interface ISysUser:IPrincipal
    {
        ISysUserModel UserModel { get; }
    }
}
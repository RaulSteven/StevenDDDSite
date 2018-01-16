using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Steven.Domain.Infrastructure.SysUser
{
    public interface ISysUserModel
    {
        long UserId { get; set; }
        string UserName { get; set; }
        long HeadImageId { get; set; }
        string GId { get; set; }
    }
}
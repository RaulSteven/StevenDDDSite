using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.Models;

namespace Steven.Domain.Repositories
{
    public interface IUserAppInfoRepository : IRepository<UserAppInfo>
    {
        UserAppInfo GetByOpenId(string openId, long shopId);

        UserAppInfo GetByUserId(long userId, long shopId);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.Models;
using Dapper;

namespace Steven.Domain.Repositories
{
    public class UserAppInfoRepository : Repository<UserAppInfo>, IUserAppInfoRepository
    {
        public UserAppInfo GetByOpenId(string openId,long shopId)
        {
            var model = DbConn.QueryFirstOrDefault<UserAppInfo>(Query() + " and OpenId = @openId and ShopId=@shopId", new { openId,shopId });
            return model;
        }

        public UserAppInfo GetByUserId(long userId,long shopId)
        {
            var model = DbConn.QueryFirstOrDefault<UserAppInfo>(Query() + " and UserId = @userId and ShopId=@shopId", new { userId,shopId });
            return model;
        }
    }
}

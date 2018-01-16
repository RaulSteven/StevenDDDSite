using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.Models;
using Dapper;

namespace Steven.Domain.Repositories
{
    public class ShopAppInfoRepository : Repository<ShopAppInfo>, IShopAppInfoRepository
    {
        public long GetShopIdByKey(string key)
        {
            var obj = DbConn.QueryFirstOrDefault<ShopAppInfo>(Query() + " and BeiLinAppSecrect = @key", new { key });
            if (obj == null)
            {
                return 0;
            }
            return obj.ShopId;
        }
        public ShopAppInfo GetByShopId(long shopId)
        {
            var sql = AdminQuery() +" and ShopId=@shopId";
            var obj = DbConn.QueryFirstOrDefault<ShopAppInfo>(sql, new { shopId });
            return obj;
        }
        public ShopAppInfo GetShopIdByAppId(string appId)
        {
            var obj = DbConn.QueryFirstOrDefault<ShopAppInfo>(Query() + " and BeiLinAppId = @appId", new { appId });
            return obj;
        }
    }
}

using Steven.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steven.Domain.Repositories
{
    public interface IShopAppInfoRepository:IRepository<ShopAppInfo>
    {
        long GetShopIdByKey(string key);
        ShopAppInfo GetByShopId(long shopId);
        ShopAppInfo GetShopIdByAppId(string appId);
    }
}

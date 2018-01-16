using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.APIModels;
using Steven.Domain.Enums;
using Steven.Domain.Models;
using Steven.Domain.ViewModels;

namespace Steven.Domain.Repositories
{
    public interface IShopBuyWayRepository : IRepository<ShopBuyWay>
    {
        bool IsHaveByShop(long shopId);
        bool IsHaveOneByShop(long shopId);
        ShopBuyWay GetByShopBuyType(long shopId, BuyType type);
        List<ShopBuyWay> GetListByShop(long shopId);
        List<BuyTypeBizModel> GetBuyTypeListByShop(long shopId);
    }
}

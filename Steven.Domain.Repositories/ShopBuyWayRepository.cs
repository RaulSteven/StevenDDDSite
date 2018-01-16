using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.APIModels;
using Steven.Domain.Enums;
using Steven.Domain.Models;
using Steven.Domain.ViewModels;
using Dapper;

namespace Steven.Domain.Repositories
{
    public class ShopBuyWayRepository : Repository<ShopBuyWay>, IShopBuyWayRepository
    {
        public bool IsHaveByShop(long shopId)
        {
            var sql = Query() + " and ShopId=@shopId";
            var model = DbConn.QueryFirstOrDefault<ShopBuyWay>(sql, new { shopId });
            return model != null;
        }

        public bool IsHaveOneByShop(long shopId)
        {
            var sql = Query(" count(*) ") + " and ShopId=@shopId";
            var count = DbConn.QueryFirstOrDefault<int>(sql, new { shopId });
            return count>1;
        }

        public ShopBuyWay GetByShopBuyType(long shopId, BuyType type)
        {
            var sql = Query() + " and ShopId=@shopId and Type=@type";
            var model = DbConn.QueryFirstOrDefault<ShopBuyWay>(sql, new { shopId, type });
            return model;
        }

        public List<ShopBuyWay> GetListByShop(long shopId)
        {
            var sql = Query() + " and ShopId=@shopId order by Type asc";
            var list = DbConn.Query<ShopBuyWay>(sql, new { shopId }).ToList();
            return list;
        }
        public List<BuyTypeBizModel> GetBuyTypeListByShop(long shopId)
        {
            var sql = Query(" Type ") + " and ShopId=@shopId order by Type asc";
            var list = DbConn.Query<BuyTypeBizModel>(sql, new { shopId }).ToList();
            return list;
        }
    }
}

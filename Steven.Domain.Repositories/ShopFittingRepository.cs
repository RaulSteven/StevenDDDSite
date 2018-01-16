using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.Models;
using Dapper;
using Steven.Domain.Enums;

namespace Steven.Domain.Repositories
{
    public class ShopFittingRepository : Repository<ShopFitting>, IShopFittingRepository
    {
        public IEnumerable<ShopFitting> GetList(long shopId)
        {
            var sql = Query() + " and ShopId=@shopId";
            return DbConn.Query<ShopFitting>(sql, new { shopId });
        }

        public ShopFitting Get(long shopId, ShopFittingType type)
        {
            var sql = Query() + " and ShopId=@shopId and FittingType=@type";
            return DbConn.QueryFirstOrDefault<ShopFitting>(sql, new { shopId,type });
        }

        public ShopFitting Save(long shopId,string title, string subTitle, bool hasSelected, ShopFittingType fittingType,string jsonData)
        {
            var fitting = Get(shopId, fittingType);
            if (fitting == null)
            {
                fitting = new ShopFitting();
            }
            fitting.ShopId = shopId;
            fitting.Title = title;
            fitting.SubTitle = subTitle;
            fitting.HasSelected = hasSelected;
            fitting.FittingType = fittingType;
            fitting.JsonData = jsonData;
            Save(fitting);
            return fitting;
        }
    }
}

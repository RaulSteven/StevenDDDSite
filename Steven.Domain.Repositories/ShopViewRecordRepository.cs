using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.Models;
using Dapper;
namespace Steven.Domain.Repositories
{
    public class ShopViewRecordRepository:Repository<ShopViewRecord>,IShopViewRecordRepository
    {
        public int GetTodayCount(long shopId)
        {
            var sql = Count() + " and ShopId=@shopId and DateDiff(dd,UpdateTime,getdate())=0";
            return DbConn.QueryFirst<int>(sql, new { shopId });
        }
    }
}

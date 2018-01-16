using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.Models;
using Steven.Domain.Enums;
using Steven.Domain.Infrastructure;
using Steven.Domain.ViewModels;
using Dapper;

namespace Steven.Domain.Repositories
{
    public class ShopOrderRepository : Repository<ShopOrder>, IShopOrderRepository
    {
        public ShopOrder GetByOrderId(long id, long shopId, long userId)
        {
            var sql = Query() + " and ShopId=@shopId and UserId=@userId and Id=@id";
            return DbConn.QueryFirstOrDefault<ShopOrder>(sql, new { shopId, userId, id });
        }

        public int GetCount(long shopId, OrderStatus status)
        {
            var sql = Count() + " and ShopId=@shopId and OrderStatus=@status";
            return DbConn.QueryFirst<int>(sql, new { shopId, status });
        }

        public int GetCount(OrderStatus? status)
        {
            var sql = Count();
            if (status.HasValue)
            {
                sql = sql + "and OrderStatus=@status";
            }
            return DbConn.QueryFirst<int>(sql, new { status });
        }

        public decimal GetIncome()
        {
            var sql = Query();
            return DbConn.Query<ShopOrder>(sql).Sum(m => m.TotalPrice);
        }

        public int GetCreateCount(long shopId, int day)
        {
            var sql = Count() + " and ShopId=@shopId and DateDiff(dd,CreateTime,getdate())=@day";
            var count = DbConn.QueryFirst<int>(sql, new { shopId, day });
            return count;
        }

        public int GetPayedCount(long shopId, int day)
        {
            var sql = Count() + " and ShopId=@shopId and DateDiff(dd,PayTime,getdate())=@day";
            var count = DbConn.QueryFirst<int>(sql, new { shopId, day });
            return count;
        }

        public int GetTodayPayedCount(long shopId)
        {
            var sql = Count() + " and ShopId=@shopId and DateDiff(dd,PayTime,getdate())=0";
            var count = DbConn.QueryFirst<int>(sql, new { shopId });
            return count;
        }

        public decimal GetTodayTotalPrice(long shopId)
        {
            var sql = Query("SUM(TotalPrice)") + " and ShopId=@shopId and DateDiff(dd,PayTime,getdate())=0";
            var totalPrice = DbConn.QueryFirst<decimal?>(sql, new { shopId });
            return totalPrice.HasValue ? totalPrice.Value : 0;
        }
        public Pager<ShopOrder> GetShopOrderPager(long shopId, BuyType? t, string keyword, DateTime? startTime, DateTime? endTime, OrderStatus? status, PageSearchModel search)
        {
            var sql = new StringBuilder();
            sql.Append(Query() + "and ShopId=@shopId");
            var param = new DynamicParameters();
            param.Add("shopId", shopId);
            if (t.HasValue)
            {
                sql.Append(" and BuyType=@t");
                param.Add("t", t.Value);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                sql.Append(" and (Receiver like @keyword or Phone like @keyword or Code like @keyword)");
                param.Add("keyword", $"%{keyword}%");
            }
            if (startTime.HasValue && endTime.HasValue)
            {
                endTime = endTime.Value.AddDays(1);
                sql.Append(" and CreateTime between CONVERT(varchar(100), @startTime, 111) and CONVERT(varchar(100),@endTime, 111) ");
                param.Add("startTime", startTime.Value);
                param.Add("endTime", endTime.Value);
            }
            if (status.HasValue)
            {
                sql.Append(" and OrderStatus=@status");
                param.Add("status", status.Value);
            }
            return Pager(sql.ToString(), param, search);
        }
        public ShopOrder GetByShopOrderId(long id, long shopId)
        {
            var sql = Query() + " and ShopId=@shopId and Id=@id";
            return DbConn.QueryFirstOrDefault<ShopOrder>(sql, new { shopId, id });
        }
        public ShopOrder GetByOrderCode(string code)
        {
            var sql = Query() + " and Code=@code";
            return DbConn.QueryFirstOrDefault<ShopOrder>(sql, new { code });
        }

        public Pager<ShopOrder> GetAdminOrderPager(long? shopId, string keyword, DateTime? startTime, DateTime? endTime, OrderStatus? status, BuyType? buyType, PageSearchModel search)
        {
            var sql = new StringBuilder();
            sql.Append(AdminQuery());
            var param = new DynamicParameters();
            if (shopId.HasValue)
            {
                sql.Append(" and ShopId=@shopId");
                param.Add("shopId", shopId.Value);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                sql.Append(" and (Receiver like @keyword or Phone like @keyword or Code like @keyword or ValidCode like @keyword)");
                param.Add("keyword", $"%{keyword}%");
            }
            if (startTime.HasValue && endTime.HasValue)
            {
                endTime = endTime.Value.AddDays(1);
                sql.Append(" and CreateTime between CONVERT(varchar(100), @startTime, 111) and CONVERT(varchar(100),@endTime, 111) ");
                param.Add("startTime", startTime.Value);
                param.Add("endTime", endTime.Value);
            }
            if (status.HasValue)
            {
                sql.Append(" and OrderStatus=@status");
                param.Add("status", status.Value);
            }
            if (buyType.HasValue)
            {
                sql.Append(" and BuyType=@buyType");
                param.Add("buyType", buyType.Value);
            }
            return Pager(sql.ToString(), param, search);
        }
        public bool IsShopHaveOrder(long shopId, OrderStatus? status)
        {
            var sql = Count() + " and ShopId=@shopId";
            if (status.HasValue)
            {
                sql = sql + " and OrderStatus=@status";
            }
            var count = DbConn.QueryFirst<int>(sql, new { shopId, status });
            return count > 0;
        }

        public List<ShopOrder> GetAdminOrderByTime(DateTime startTime, DateTime endTime)
        {
            var sql = Query() + " and CreateTime between CONVERT(varchar(100), @startTime, 111) and CONVERT(varchar(100),@endTime, 111) ";

            var list = DbConn.Query<ShopOrder>(sql, new { startTime, endTime }).ToList();
            return list;
        }
        public List<AdminHomeStatisModel> GetAdminOrderStatisByTime(AdminHomeDataType t, DateTime startTime, DateTime endTime)
        {
            string sql = string.Empty;
            switch (t)
            {
                case AdminHomeDataType.Today:
                    sql = Query("count(id) as [Count] ,datepart(hh,CreateTime) [Time]") + " and CreateTime between CONVERT(varchar(100), @startTime, 111) and CONVERT(varchar(100),@endTime, 111) group by datepart(hh,CreateTime) ";
                    break;
                case AdminHomeDataType.Month:
                    sql = Query("count(id) as [Count] ,datepart(MM-dd,CreateTime) [Time]") + " and CreateTime between CONVERT(varchar(100), @startTime, 111) and CONVERT(varchar(100),@endTime, 111) group by datepart(dd,CreateTime) ";
                    break;
                case AdminHomeDataType.Year:
                    sql = Query("count(id) as [Count] ,datepart(yyyy-mm,CreateTime) [Time]") + " and CreateTime between CONVERT(varchar(100), @startTime, 111) and CONVERT(varchar(100),@endTime, 111) group by datepart(mm,CreateTime) ";
                    break;
            }
            var list = DbConn.Query<AdminHomeStatisModel>(sql, new { startTime, endTime }).ToList();
            return list;
        }
        public ShopOrder GetByOrderValidCode(long shopId, string validCode)
        {
            var sql = Query() + " and ValidCode=@validCode and ShopId=@shopId";
            return DbConn.QueryFirstOrDefault<ShopOrder>(sql, new { validCode, shopId });
        }

        public int GetCountByBuyType(long shopId, long userId, BuyType type)
        {
            var param = new DynamicParameters();
            var sql = Count() + " and UserId = @userId and ShopId=@shopId and BuyType=@type";
            param.Add("shopId", shopId);
            param.Add("userId", userId);
            param.Add("type", type);
            switch (type)
            {
                case BuyType.Delivery:
                    sql = sql + " and (OrderStatus=@status or OrderStatus=@status2 or OrderStatus=@status3)";
                    param.Add("status", OrderStatus.UnPay);
                    param.Add("status2", OrderStatus.Waiting);
                    param.Add("status3", OrderStatus.ShipmentPending);
                    break;
                case BuyType.Arrival:
                    sql = sql + " and (OrderStatus=@status or OrderStatus=@status2)";
                    param.Add("status", OrderStatus.UnPay);
                    param.Add("status2", OrderStatus.Paid);
                    break;
            }
            var count = DbConn.QueryFirst<int>(sql, param);
            return count;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.Models;
using Steven.Domain.Enums;
using Steven.Domain.Infrastructure;
using Steven.Domain.ViewModels;

namespace Steven.Domain.Repositories
{
    public interface IShopOrderRepository:IRepository<ShopOrder>
    {
        ShopOrder GetByOrderId(long id, long shopId, long userId);

        decimal GetTodayTotalPrice(long shopId);

        int GetTodayPayedCount(long shopId);

        int GetCount(long shopId, OrderStatus status);
        int GetCount(OrderStatus? status);
        decimal GetIncome();
        int GetCreateCount(long shopid, int day);

        int GetPayedCount(long shopId, int day);

        Pager<ShopOrder> GetShopOrderPager(long shopId, BuyType? t, string keyword, DateTime? startTime, DateTime? endTime,
            OrderStatus? status, PageSearchModel search);

        ShopOrder GetByShopOrderId(long id, long shopId);

        ShopOrder GetByOrderCode(string code);

        Pager<ShopOrder> GetAdminOrderPager(long? shopId, string keyword, DateTime? startTime, DateTime? endTime, OrderStatus? status, BuyType? buyType,
            PageSearchModel search);

        bool IsShopHaveOrder(long shopId, OrderStatus? status);

        List<ShopOrder> GetAdminOrderByTime(DateTime startTime, DateTime endTime);

        List<AdminHomeStatisModel> GetAdminOrderStatisByTime(AdminHomeDataType t, DateTime startTime, DateTime endTime);

        ShopOrder GetByOrderValidCode(long shopId, string validCode);

        int GetCountByBuyType(long shopId, long userId, BuyType type);
    }
}

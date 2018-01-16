using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.APIModels;
using Steven.Domain.Enums;
using Steven.Domain.Infrastructure;
using Steven.Domain.Models;

namespace Steven.Domain.Services
{
    public interface IShopOrderSvc
    {
        Pager<OrderModel> GetMemberOrderList(BuyType type, MemberOrderStatus? status, long userId, long shopId, PageSearchModel search);

        Task SendTemplate(long orderId, long shopId, long userId, TemplateType type, string formId);

        void ShopNotifyOrderSave(long shopId, ShopOrder order, TemplateType type);
    }
}

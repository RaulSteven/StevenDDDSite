using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.APIModels;
using Steven.Domain.Models;
using Steven.Domain.ViewModels;

namespace Steven.Domain.Repositories
{
    public interface IShopOrderProductRepository:IRepository<ShopOrderProduct>
    {
        List<ConfirmOrderProModel> GetProListByOrderId(long orderId);

        List<ShopOrderProduct>  GetShopOrderProList(long orderId);

        string GetShopProName(long orderId);
    }
}

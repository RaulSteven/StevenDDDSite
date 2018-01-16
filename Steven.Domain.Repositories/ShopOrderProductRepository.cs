using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Core.Utilities;
using Steven.Domain.APIModels;
using Steven.Domain.Models;
using Steven.Domain.ViewModels;
using Dapper;

namespace Steven.Domain.Repositories
{
    public class ShopOrderProductRepository : Repository<ShopOrderProduct>, IShopOrderProductRepository
    {
        public List<ConfirmOrderProModel> GetProListByOrderId(long orderId)
        {
            return DbConn.Query<ConfirmOrderProModel>(Query("ProductId,ProductPicAttId as ImgId,ProductName as Name,ProductUnit as Unit,Price,Number") + " and OrderId = @orderId",
                new { orderId }).ToList();
        }

        public List<ShopOrderProduct> GetShopOrderProList(long orderId)
        {
            var list = DbConn.Query<ShopOrderProduct>(Query() + " and OrderId = @orderId",
                new { orderId })
                .ToList();
            return list;
        }

        public string GetShopProName(long orderId)
        {
            var list = DbConn.Query<string>(Query("ProductName+' x'+ cast(Number as varchar(200))as name") + " and OrderId = @orderId",
                 new { orderId })
                 .ToList();
            var arr = list.Select(m => m).ToList();
            var name = string.Join("，", arr);
            return name;
        }
    }
}

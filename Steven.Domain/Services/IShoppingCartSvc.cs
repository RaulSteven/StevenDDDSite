using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.APIModels;
using Steven.Domain.Models;

namespace Steven.Domain.Services
{
    public interface IShoppingCartSvc
    {
        UserCartModel GetUserCart(long userId, long shopId);

        void UpdateAllChecked(bool isChecked, long userId, long shopId);

        void DeleteUserCart(long userId, long shopId);

        ConfirmOrderModel GetConfirmOrder(long userId, long shopId);
        ConfirmOrderModel GetNowConfirmOrder(long shopId, Product product);
    }
}

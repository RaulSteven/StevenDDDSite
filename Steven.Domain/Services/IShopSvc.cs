using Steven.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steven.Domain.Services
{
    public interface IShopSvc
    {
        JsonModel Save(ShopModel model);

        bool IsLimitShopPro(long shopId, ref int limitNum, ref int count);

        bool IsLimitShopCls(long shopId, ref int limitNum, ref int count);
    }
}

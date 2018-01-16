using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.Models;
using Steven.Domain.Enums;

namespace Steven.Domain.Repositories
{
    public interface IShopFittingRepository:IRepository<ShopFitting>
    {
        IEnumerable<ShopFitting> GetList(long shopId);

        ShopFitting Get(long shopId, ShopFittingType type);

        ShopFitting Save(long shopId, string title, string subTitle, bool hasSelected, ShopFittingType fittingType,string jsonData="");
    }
}

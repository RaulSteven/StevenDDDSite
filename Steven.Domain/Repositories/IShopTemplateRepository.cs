using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.Enums;
using Steven.Domain.Models;
using Steven.Domain.ViewModels;

namespace Steven.Domain.Repositories
{
    public interface IShopTemplateRepository : IRepository<ShopTemplate>
    {
        List<ShopTemplate> GetList(long shopId);
        ShopTemplate GetByShopTemplateType(long shopId, TemplateType type);
        JsonModel Save(string templateIds, long shopId);
        void ShopNotifyAdd(long shopId, ISysConfigRepository sysConfigRepository);
        void ShopNotifyDelete(long shopId);

        bool IsHaveNotify(long shopId, TemplateType type);
        string GetTemplateId(TemplateType t, ISysConfigRepository sysConfigRepository);
    }
}

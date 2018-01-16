using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.Models;
using Steven.Domain.Infrastructure;
using Steven.Domain.ViewModels;

namespace Steven.Domain.Repositories
{
    public interface IShopRepository : IRepository<Shop>
    {
        Shop GetIncludeUser(long id);

        Pager<ShopModel> GetPager(string keyword,long ? agentId, PageSearchModel search);

        int BatchDele(string ids);
        IEnumerable<TypeaheadModel> GetList();

        Shop GetByUserId(long userId);
    }
}

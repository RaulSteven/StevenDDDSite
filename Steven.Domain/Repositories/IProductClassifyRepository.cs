using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.APIModels;
using Steven.Domain.Models;
using Steven.Domain.Infrastructure;
using Steven.Domain.ViewModels;

namespace Steven.Domain.Repositories
{
    public interface IProductClassifyRepository:IRepository<ProductClassify>
    {
        Pager<ProductClassify> GetPager(long shopId,PageSearchModel search);
        JsonModel BatchDele(string ids);

        IEnumerable<TypeaheadModel> GetList(long shopId);

        List<HomeClassifyListModel> GetHomeClassify(long shopId);

        IEnumerable<ProductClassify> GetListByShopId(long shopId);

        JsonModel Delete(long id, long shopId);

        int ShopClsCount(long shopId);
    }
}

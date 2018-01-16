using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.APIModels;
using Steven.Domain.Enums;
using Steven.Domain.Models;
using Steven.Domain.Infrastructure;
using Steven.Domain.ViewModels;
using Dapper;

namespace Steven.Domain.Repositories
{
    public interface IProductRepository:IRepository<Product>
    {
        Pager<Product> GetPager(string keyword, long? shopId,ProductStatus?status, PageSearchModel search);      

        int BatchDele(string ids);

        Pager<HomeProductListModel> GetApiProductPagedList(long shopId, long? cls, PageSearchSortModel search);

        Product GetApiProduct(long shopId, long id);
        string ApiBaseSql(string query, long shopId, ref DynamicParameters parameters);
        Pager<Product> GetPager(long? clzId, long shopId, PageSearchSortModel search);
        Pager<Product> GetPager(ProductTag tag, long shopId,PageSearchModel search);

        int BatchTag(long[] idArr, ProductTag tag,long shopId);
        int BatchStatus(long[] idArr, ProductStatus status,long shopId);
        int BatchClassify(long[] idArr, long clzId, long shopId);

        IEnumerable<ProductSelectModel> GetList(long shopId);

        List<HomeProductListModel> GetHomeProduct(long shopId, string jsonData);

        int ShopProCount(long shopId);

        int UpdateStock(long id, int stock);
    }
}

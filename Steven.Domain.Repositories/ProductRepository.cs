using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Core.Extensions;
using Steven.Domain.Infrastructure;
using Steven.Domain.APIModels;
using Steven.Domain.Enums;
using Steven.Domain.Models;
using Dapper;
using Steven.Core.Utilities;
using MLS.Domain.APIModels;
using Steven.Domain.ViewModels;

namespace Steven.Domain.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public Pager<Product> GetPager(string keyword, long? shopId, ProductStatus? status, PageSearchModel search)
        {
            var sql = new StringBuilder();
            sql.Append(@"select p.*,s.Name as ShopName from Product p
                left join Shop s on p.ShopId = s.Id
                where 1=1 ");
            var param = new DynamicParameters();
            if (!string.IsNullOrEmpty(keyword))
            {
                sql.Append(" and Title like @keyword");
                param.Add("keyword", $"%{keyword}%");
            }
            if (shopId.HasValue)
            {
                sql.Append(" and ShopId =@shopId");
                param.Add("shopId", shopId.Value);
            }
            if (status.HasValue)
            {
                sql.Append(" and Status=@status");
                param.Add("status", status.Value);
            }
            return Pager(sql.ToString(), param, search);
        }

        public int BatchDele(string ids)
        {
            var idArra = StringUtility.ConvertToBigIntArray(ids, ',');
            if (idArra == null || idArra.Length == 0)
            {
                return 0;
            }
            return Trans(trans =>
            {
                var count = BatchDele(TableSource.Product, idArra, trans);
                var deleSpecs = "delete from ProductSpecs where ProductId in @productIds";
                DbConn.Execute(deleSpecs, new { productIds = idArra }, trans);
                return count;
            });
        }

        public Pager<HomeProductListModel> GetApiProductPagedList(long shopId, long? cls, PageSearchSortModel search)
        {
            var param = new DynamicParameters();
            var query = Query("Id,Title as Name,Price as PriceDecimal,OldPrice,Unit,PicAttIds,Sort,UpdateTime");
            var baseSql = ApiBaseSql(query, shopId, ref param);
            var sql = new StringBuilder(baseSql);
            if (cls.HasValue)
            {
                sql.Append(" and ProductClassifyId=@cls");
                param.Add("cls", cls.Value);
            }
            return Pager<HomeProductListModel>(sql.ToString(), param, search);
        }

        public Product GetApiProduct(long shopId, long id)
        {
            var param = new DynamicParameters();
            var query = Query() + " and Id=@id";
            var sql = ApiBaseSql(query, shopId, ref param);
            param.Add("id", id);
            var obj = DbConn.QueryFirstOrDefault<Product>(sql, param);
            return obj;
        }

        public string ApiBaseSql(string query, long shopId, ref DynamicParameters parameters)
        {
            var sql = query + " and ShopId=@shopId and Status=@status";
            parameters.Add("shopId", shopId);
            parameters.Add("status", ProductStatus.OnSale);
            return sql;
        }

        public Pager<Product> GetPager(long? clzId, long shopId, PageSearchSortModel search)
        {
            var sql = new StringBuilder(Query() + " and ShopId=@shopId");
            var param = new DynamicParameters();
            param.Add("shopId", shopId);
            if (clzId.HasValue && clzId.Value != 0)
            {
                sql.Append(" and ProductClassifyId=@clzId");
                param.Add("clzId", clzId.Value);
            }
            return Pager<Product>(sql.ToString(), param, search);
        }

        public Pager<Product> GetPager(ProductTag tag, long shopId, PageSearchModel search)
        {
            var sql = new StringBuilder(Query() + " and ShopId=@shopId and Tag=@tag");
            var param = new DynamicParameters();
            param.Add("shopId", shopId);
            param.Add("tag", tag);
            return Pager(sql.ToString(), param, search);
        }

        public int BatchTag(long[] idArr, ProductTag tag, long shopId)
        {
            var sql = "Update Product set Tag = @tag where Id in @idArr and ShopId =@shopId";
            return DbConn.Execute(sql, new { idArr, tag, shopId });
        }

        public int BatchStatus(long[] idArr, ProductStatus status, long shopId)
        {
            var sql = "Update Product set Status = @status where Id in @idArr and ShopId =@shopId";
            return DbConn.Execute(sql, new { idArr, status, shopId });
        }

        public int BatchClassify(long[] idArr, long clzId, long shopId)
        {
            var sql = "Update Product set ProductClassifyId = @clzId where Id in @idArr and ShopId =@shopId";
            var count = DbConn.Execute(sql, new { idArr, clzId, shopId });
            //更新商品分类的商品数量
            var updateSql = "update ProductClassify set ProductNumber = (Select Count(Id) from Product where ProductClassifyId = ProductClassify.Id) where ShopId=@shopId";
            DbConn.Execute(updateSql, new { shopId });
            return count;
        }

        public IEnumerable<ProductSelectModel> GetList(long shopId)
        {
            var sql = Query("Id,Title,PicAttIds,ProductClassifyId,Price,OldPrice")+" and ShopId=@shopId and Status=@status";
            var list = DbConn.Query<ProductSelectModel>(sql, new { shopId, status = ProductStatus.OnSale });
            return list;
        }
        public List<HomeProductListModel> GetHomeProduct(long shopId, string jsonData)
        {
            var param = new DynamicParameters();
            var query =
                Query("Id,Title as Name,Price as PriceDecimal,OldPrice,Unit,PicAttIds") + " and (Id in @ids)";
            var baseSql = ApiBaseSql(query, shopId, ref param);
            var ids = jsonData.Split(',');
            param.Add("ids", ids);
            var list = DbConn.Query<HomeProductListModel>(baseSql, param).ToList();
            foreach (var item in list)
            {
                item.Price = item.PriceDecimal.ToString("F2");
                item.OriginalPrice = item.OldPrice.ToString("F2");
            }
            return list;
        }

        public int ShopProCount(long shopId)
        {
            var sql = Count() + " and ShopId=@shopId";
            var count = DbConn.QueryFirstOrDefault<int>(sql, new { shopId });
            return count;
        }

        public int UpdateStock(long id,int stock)
        {
            var param = new DynamicParameters();
            var sql = "Update Product set Stock=@stock where Id=@id ";
            param.Add("id", id);
            param.Add("stock", stock);
            return DbConn.Execute(sql, param);
        }
    }
}

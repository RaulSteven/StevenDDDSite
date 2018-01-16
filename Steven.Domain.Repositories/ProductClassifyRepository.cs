using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Core.Extensions;
using Steven.Domain.APIModels;
using Steven.Domain.Enums;
using Steven.Domain.Infrastructure;
using Steven.Domain.Models;
using Dapper;
using Steven.Domain.ViewModels;
using Steven.Core.Utilities;

namespace Steven.Domain.Repositories
{
    public class ProductClassifyRepository : Repository<ProductClassify>, IProductClassifyRepository
    {
        public JsonModel BatchDele(string ids)
        {
            var result = new JsonModel();
            var idArra = StringUtility.ConvertToBigIntArray(ids, ',');
            if (idArra == null || idArra.Length == 0)
            {
                result.msg = $"要删除的数据[{ids}]是空的！";
                return result;
            }
            var sql = Count() + " and Id in @ids and ProductNumber > 0";
            var count = DbConn.QueryFirst<int>(sql, new { ids = idArra });

            if (count == 0)
            {
                BatchDele(TableSource.ProductClassify, idArra);
                result.msg = "删除成功！";
                result.code = JsonModelCode.Succ;
            }
            else
            {
                result.msg = "该商品分类里还有商品，无法删除！请先修改商品的所属分类！";
            }
            return result;
        }
        public List<HomeClassifyListModel> GetHomeClassify(long shopId)
        {
            var sql = Query(" top 4 Id,Name,IconAttaId,Sort") + " and ShopId=@shopId order by Sort desc";
            var list = DbConn.Query<HomeClassifyListModel>(sql, new { shopId })
                .ToList();
            return list;
        }

        public IEnumerable<TypeaheadModel> GetList(long shopId)
        {
            var sql = AdminQuery("Id,Name") + " and ShopId =@shopId";
            return DbConn.Query<TypeaheadModel>(sql, new { shopId });
        }

        public Pager<ProductClassify> GetPager(long shopId, PageSearchModel search)
        {
            var sql = AdminQuery() + " and ShopId =@shopId";
            var param = new DynamicParameters();
            param.Add("shopId", shopId);
            return Pager(sql, param, search);
        }

        public IEnumerable<ProductClassify> GetListByShopId(long shopId)
        {
            var sql = Query("Id,Name,IconAttaId,ShopId,Sort,(Select Count(Id) from Product where ProductClassify.Id = ProductClassifyId) as ProductNumber") + " and ShopId =@shopId order by Sort desc";
            return DbConn.Query<ProductClassify>(sql, new { shopId });
        }

        public JsonModel Delete(long id, long shopId)
        {
            var result = new JsonModel();

            var sql = "select count(Id) from Product where ProductClassifyId = @id and shopId = @shopId";
            var count = DbConn.QueryFirst<int>(sql, new { id, shopId });
            if (count > 0)
            {
                result.msg = "该商品分组里还有商品，无法删除！请先修改商品的所属分组！";
                return result;
            }
            sql = Delete() + " and Id=@id and ShopId=@shopId";
            DbConn.Execute(sql, new { id, shopId });
            result.msg = "删除成功！";
            result.code = JsonModelCode.Succ;
            return result;
        }
        public int ShopClsCount(long shopId)
        {
            var sql = Count() + " and ShopId=@shopId";
            var count = DbConn.QueryFirstOrDefault<int>(sql, new { shopId });
            return count;
        }
    }
}

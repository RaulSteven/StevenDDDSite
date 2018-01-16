using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.APIModels;
using Steven.Domain.Models;
using Dapper;
using Steven.Domain.ViewModels;


namespace Steven.Domain.Repositories
{
    public class ProductSpecsRepository : Repository<ProductSpecs>, IProductSpecsRepository
    {
        public List<ProductDetailSpecsModel> GetProductSpecs(long shopId,long productId)
        {
            var param = new DynamicParameters();
            var sql =
                Query("Name,SpecsValue as Value") + " and ShopId=@shopId and ProductId=@productId";
            param.Add("shopId", shopId);
            param.Add("productId", productId);
            var list = DbConn.Query<ProductDetailSpecsModel>(sql, param).ToList();
            return list;
        }
        public IEnumerable<ProductSpecsModel> GetList(long productId)
        {
            var sql = AdminQuery() + " and ProductId  =@productId";
            return DbConn.Query<ProductSpecsModel>(sql, new { productId });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.APIModels;
using Steven.Domain.Models;
using Steven.Domain.ViewModels;

namespace Steven.Domain.Repositories
{
    public interface IProductSpecsRepository:IRepository<ProductSpecs>
    {
        List<ProductDetailSpecsModel> GetProductSpecs(long shopId, long productId);
        IEnumerable<ProductSpecsModel> GetList(long productId);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.APIModels;
using Steven.Domain.ViewModels;

namespace Steven.Domain.Services
{
    public interface IProductSvc
    {
        JsonModel Save(ProductModel model);

        List<HomeProductListModel> GetCartProduct(long shopId);
    }
}

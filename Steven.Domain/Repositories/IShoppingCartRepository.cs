using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.Models;
namespace Steven.Domain.Repositories
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
        ShoppingCart Get(long userId, long shopId, long productId);

        int GetTotalProNumber(long userId, long shopId);

        List<ShoppingCart> GetUserCart(long userId, long shopId);

        List<ShoppingCart> GetUserChooseCart(long userId, long shopId);
    }
}

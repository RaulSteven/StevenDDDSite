using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.Models;
using Dapper;

namespace Steven.Domain.Repositories
{
    public class ShoppingCartRepository:Repository<ShoppingCart>,IShoppingCartRepository
    {
        public ShoppingCart Get(long userId, long shopId, long productId)
        {
            var model = DbConn.QueryFirstOrDefault<ShoppingCart>(Query() + " and UserId = @userId and ProductId=@productId and ShopId=@shopId", new { userId, productId,shopId });
            return model;
        }

        public int GetTotalProNumber(long userId, long shopId)
        {
            var total = DbConn.QueryFirstOrDefault<string>(Query("sum(number)") + " and UserId = @userId and  ShopId=@shopId", new { userId, shopId });
            if (string.IsNullOrEmpty(total))
            {
                return 0;
            }
            return int.Parse(total);
        }

        public List<ShoppingCart> GetUserCart(long userId, long shopId)
        {
            return DbConn.Query<ShoppingCart>(Query() + " and UserId = @userId and ShopId=@shopId order by Id desc",
                new {userId, shopId}).ToList();
        }

        public List<ShoppingCart> GetUserChooseCart(long userId, long shopId)
        {
            return DbConn.Query<ShoppingCart>(Query() + " and IsChoose=1 and UserId = @userId and ShopId=@shopId",
                new { userId, shopId }).ToList();
        }
    }
}

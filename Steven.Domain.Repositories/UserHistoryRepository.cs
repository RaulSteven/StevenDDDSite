using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.Enums;
using Steven.Domain.Models;
using Dapper;

namespace Steven.Domain.Repositories
{
    public class UserHistoryRepository : Repository<UserHistory>, IUserHistoryRepository
    {
        public UserHistory GetModel(long shopId, long userId, TableSource source, long sourceId)
        {
            var sql = Query() + " and ShopId=@shopId and UserId=@userId and Source=@source and SourceId=@sourceId";
            var obj = DbConn.QueryFirstOrDefault<UserHistory>(sql, new {shopId, userId, source, sourceId});
            return obj;
        }
    }
}

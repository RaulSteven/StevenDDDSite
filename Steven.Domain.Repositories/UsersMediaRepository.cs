using System.Linq;
using System.Threading.Tasks;
using Steven.Domain.Models;
using Dapper;

namespace Steven.Domain.Repositories
{
    public class UsersMediaRepository : Repository<UsersMedia>, IUsersMediaRepository
    {

        public UsersMedia Save(long userId, string userName, string openId, string unionId, string headImg = "")
        {
            if (userId <= 0) return null;
            var sql = AdminQuery() + " and UserId=@userId";
            var query = DbConn.Query<UsersMedia>(sql, new { userId });
            // 用户在每个第三方平台上只能有一个账号
            if (query.Any(m => m.UserId == userId)) return null;
            var userMedia = new UsersMedia
            {
                AvatarPic = headImg,
                UserId = userId,
                UserName = userName,
                UserOpenId = openId,
                UserUnionId = unionId
            };
            Save(userMedia);
            return userMedia;
        }

        public UsersMedia GetByOpenIdAsync(string openId)
        {
            var sql = BaseQuery() + " and UserOpenId=@openId";
            return DbConn.QueryFirstOrDefault<UsersMedia>(sql, new { openId });
        }

        public UsersMedia GetByUnionIdAsync(string userUnionId)
        {
            var sql = BaseQuery() + " and UserUnionId=@userUnionId";
            return DbConn.QueryFirstOrDefault<UsersMedia>(sql, new { userUnionId });
        }

        private string BaseQuery()
        {
            return "select um.* from UsersMedias as um join Users as u on um.UserId=u.Id where 1=1";
        }

        public UsersMedia GetByUserId(long userId)
        {
            var sql = AdminQuery() + " and UserId=@userId";
            return DbConn.QueryFirstOrDefault<UsersMedia>(sql, new { userId });
        }
    }
}

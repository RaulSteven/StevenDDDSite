using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Steven.Domain.Infrastructure;
using Steven.Domain.Models;
using Steven.Core.Utilities;
using Steven.Core.Cache;
using Dapper;

namespace Steven.Domain.Repositories
{
    public class User2RoleRepository : Repository<User2Role>, IUser2RoleRepository
    {
        public ICacheManager Cache { get; set; }
        public IEnumerable<long> GetLstRoleId(long userId)
        {
            return DbConn.Query<long>(Query("RoleId") + " and UserId=@userId", new { userId = userId });
        }

        public IEnumerable<long> GetLstUserId(long roleId)
        {
            return DbConn.Query<long>(Query("UserId") + " and RoleId=@roleId", new { roleId = roleId });
        }

        public void SaveList(string userids, long[] roleIds)
        {
            var userIdArr = StringUtility.ConvertToBigIntArray(userids, ',');
            DbConn.Execute($"Delete from {GetTableName()} where UserId in @id", new { id = userIdArr });

            foreach (var userId in userIdArr)
            {
                if (roleIds == null
                  || roleIds.Length == 0)
                {
                    continue;
                }
                foreach (var roleId in roleIds)
                {
                    var role2menu = new User2Role()
                    {
                        UserId = userId,
                        RoleId = roleId,
                    };
                    Insert(role2menu);
                }
                Cache.Remove(Users.GIdPrefix + userId);
            }
        }
    }
}

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
    public class User2ApartmentRepository : Repository<User2Apartment>, IUser2ApartmentRepository
    {
        public ICacheManager Cache { get; set; }
        public IEnumerable<long> GetLstApartId(long userId)
        {
            return DbConn.Query<long>(Query("ApartmentId") + " and UserId=@userId", new { userId = userId });
        }

        public void SaveList(string userIds, string apartIds)
        {
            var userIdArr = StringUtility.ConvertToBigIntArray(userIds, ',');
            DbConn.Execute($"Delete from {GetTableName()} where UserId in @id", new { id = userIdArr});

            var apartIdArra = StringUtility.ConvertToBigIntArray(apartIds, ',');
            foreach (var userId in userIdArr)
            {
                foreach (var apartId in apartIdArra)
                {
                    if (apartId == 0)
                    {
                        continue;
                    }
                    var user2Apart = new User2Apartment()
                    {
                        UserId = userId,
                        ApartmentId = apartId,
                    };
                    Insert(user2Apart);
                }
                Cache.Remove(Users.GIdPrefix + userId);
            }
        }

        public IEnumerable<long> GetLstUserId(IEnumerable<long> lstApartId)
        {
            return DbConn.Query<long>(Query("UserId") + " and ApartmentId in @apartIds", new { apartIds = lstApartId });
        }
    }
}

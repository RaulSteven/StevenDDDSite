using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Steven.Domain.Infrastructure;
using Steven.Domain.Models;
using Steven.Core.Utilities;
using Dapper;

namespace Steven.Domain.Repositories
{
    public class UserRole2ApartmentRepository : Repository<UserRole2Apartment>, IUserRole2ApartmentRepository
    {
        public IEnumerable<long> GetLstRoleId(long apartId)
        {
            return DbConn.Query<long>(Query("RoleId") + " and ApartmentId=@apartId", new { apartId });
        }

        public void SaveList(long apartId, long[] roleIds)
        {
            DbConn.Execute($"Delete from {GetTableName()} where ApartmentId = @apartId", new { apartId });

            if (roleIds == null || roleIds.Length == 0)
            {
                return;
            }
            foreach (var roleId in roleIds)
            {
                var role2Apart = new UserRole2Apartment()
                {
                    ApartmentId = apartId,
                    RoleId = roleId,
                };
                Insert(role2Apart);
            }
        }

        public IEnumerable<long> GetLstRoleId(IEnumerable<long> apartIds)
        {
            return DbConn.Query<long>(Query("RoleId") + " and ApartmentId in @apartIds", new { apartIds });
        }

        public IEnumerable<long> GetLstApartId(long roleId)
        {
            return DbConn.Query<long>(Query("ApartmentId") + " and RoleId=@roleId", new { roleId });
        }
    }
}

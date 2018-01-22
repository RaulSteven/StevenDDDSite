using System.Collections.Generic;
using Steven.Domain.Models;

namespace Steven.Domain.Repositories
{
    public interface IUser2RoleRepository : IRepository<User2Role>
    {
        IEnumerable<long> GetLstRoleId(long userId);
        IEnumerable<long> GetLstUserId(long roleId);
        void SaveList(string userIds, long[] roleIds);
    }

}

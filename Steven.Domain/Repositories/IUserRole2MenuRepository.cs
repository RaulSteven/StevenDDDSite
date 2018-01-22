using System.Collections.Generic;
using Steven.Domain.Models;

namespace Steven.Domain.Repositories
{
    public interface IUserRole2MenuRepository:IRepository<UserRole2Menu>
    {
        void SaveList(long roleId, string menuIds);

        void SaveRole2MenuButtons(long roleId, string btnIds);

        IEnumerable<long> GetLstMenuId(long roleId);


        bool UpdateFilterGroups(long id, string filterGroups);
    }
}

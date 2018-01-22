using System.Collections.Generic;
using Steven.Domain.Infrastructure;
using Steven.Domain.Models;

namespace Steven.Domain.Repositories
{
    public interface IUserRoleRepository : IRepository<UserRole>
    {
        Pager<UserRole> GetList(string keyword,PageSearchModel search);

        void Delete(long id);

        IEnumerable<UserRole> GetList();
    }
}

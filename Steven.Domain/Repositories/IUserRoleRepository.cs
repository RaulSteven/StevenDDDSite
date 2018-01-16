using System.Collections.Generic;
using System.Threading.Tasks;
using Steven.Domain.Infrastructure;
using Steven.Domain.Models;
using Steven.Domain.ViewModels;

namespace Steven.Domain.Repositories
{
    public interface IUserRoleRepository : IRepository<UserRole>
    {
        Pager<UserRole> GetList(string keyword,PageSearchModel search);

        void Delete(long id);

        IEnumerable<UserRole> GetList();
    }
}

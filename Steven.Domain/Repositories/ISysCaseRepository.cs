using System.Collections.Generic;
using Steven.Domain.Infrastructure;
using Steven.Domain.Models;

namespace Steven.Domain.Repositories
{
    public interface ISysCaseRepository : IRepository<SysCase>
    {
        Pager<SysCase> GetPager(string name, PageSearchModel search);

        IEnumerable<SysCase> GetAll();
    }
}

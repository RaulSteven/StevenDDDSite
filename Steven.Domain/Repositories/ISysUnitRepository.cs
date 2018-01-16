using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.Infrastructure;
using Steven.Domain.Models;

namespace Steven.Domain.Repositories
{
    public interface ISysUnitRepository : IRepository<SysUnit>
    {
        Pager<SysUnit> GetPager(string name, PageSearchModel search);
        IEnumerable<string> GetLstName();
    }
}

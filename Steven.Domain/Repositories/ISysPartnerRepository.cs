using System.Collections.Generic;
using Steven.Domain.Models;
using Steven.Domain.Infrastructure;

namespace Steven.Domain.Repositories
{
    public interface ISysPartnerRepository : IRepository<SysPartner>
    {
        Pager<SysPartner> GetPager(string name, PageSearchModel search);

        IEnumerable<SysPartner> GetAll();
    }
}

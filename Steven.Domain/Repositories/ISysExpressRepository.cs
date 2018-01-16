using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.Infrastructure;
using Steven.Domain.Models;
using System.Web.Mvc;

namespace Steven.Domain.Repositories
{
    public interface ISysExpressRepository : IRepository<SysExpress>
    {
        Pager<SysExpress> GetPager(string name, PageSearchModel search);
        List<SelectListItem> GetSelectList();
    }
}

using Steven.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.ViewModels;
using Steven.Domain.Infrastructure;

namespace Steven.Domain.Repositories
{
    public interface IAgentRepository:IRepository<Agent>
    {
        Agent GetIncludeUser(long id);

        Pager<AgentModel> GetPager(string keyword, PageSearchModel search);

        int BatchDele(string ids);

        IEnumerable<TypeaheadModel> GetList();
    }
}

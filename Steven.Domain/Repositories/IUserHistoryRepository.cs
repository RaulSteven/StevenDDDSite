using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.Enums;
using Steven.Domain.Models;

namespace Steven.Domain.Repositories
{
    public interface IUserHistoryRepository : IRepository<UserHistory>
    {
        UserHistory GetModel(long shopId, long userId, TableSource source, long sourceId);
    }
}

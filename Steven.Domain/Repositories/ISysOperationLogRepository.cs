using System;
using System.Threading.Tasks;
using Steven.Domain.Infrastructure;
using Steven.Domain.Models;
using Steven.Domain.Enums;
using Steven.Domain.ViewModels;

namespace Steven.Domain.Repositories
{
    public interface ISysOperationLogRepository:IRepository<SysOperationLog>
    {
         long Insert(TableSource cat, OperationType type, string srcId);

         long Insert(TableSource cat, OperationType type, long srcId);

        Pager<SysOperationLog> GetPager(PageSearchModel search, string source, string sourceId,
            TableSource? cat, OperationType? type, string q, string uname, DateTime? minTime, DateTime? maxTime);
    }
}

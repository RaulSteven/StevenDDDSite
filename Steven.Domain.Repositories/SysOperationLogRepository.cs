using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Steven.Domain.Enums;
using Steven.Domain.Infrastructure;
using Steven.Domain.Models;
using Steven.Core.Extensions;
using Steven.Core.Utilities;
using Dapper;
using Steven.Domain.ViewModels;

namespace Steven.Domain.Repositories
{
    public class SysOperationLogRepository : Repository<SysOperationLog>, ISysOperationLogRepository
    {
        public  long Insert(TableSource cat, OperationType type, long srcId)
        {
            return  Insert(cat, type, srcId.ToString());
        }

        public long Insert(TableSource cat, OperationType type, string srcId)
        {
            var title = string.Format("{0} {1}", type.GetDescriotion(), cat.GetDescriotion());
            var log = new SysOperationLog
            {
                DataSouceId = srcId,
                DataSource = cat.ToString(),
                LogCat = cat,
                LogDesc = "",
                LogTitle = title,
                LogType = type,
                CreateIP = GetIP()
            };
           return Insert(log);
        }

        public Pager<SysOperationLog> GetPager(PageSearchModel search, string source, string sourceId,
            TableSource? cat, OperationType? type, string q, string uname, DateTime? minTime, DateTime? maxTime)
        {
            var dynamicParams = new DynamicParameters();
            var sqlStr = new StringBuilder(AdminQuery());

            if (!string.IsNullOrEmpty(source))
            {
                sqlStr.Append(" and DataSource like @source");
                dynamicParams.Add("source", $"%{source}%");
            }
            if (!string.IsNullOrEmpty(sourceId))
            {
                sqlStr.Append(" and  DataSouceId = @srcId");
                dynamicParams.Add("srcId", sourceId);
            }
            if (cat.HasValue)
            {
                sqlStr.Append(" and  LogCat = @cat");
                dynamicParams.Add("cat", cat.Value);
            }
            if (type.HasValue)
            {
                sqlStr.Append(" and  LogType = @type");
                dynamicParams.Add("type", type.Value);
            }
            if (!string.IsNullOrEmpty(q))
            {
                sqlStr.Append(" and  LogTitle like @title");
                dynamicParams.Add("title", $"%{q}%");
            }
            if (!string.IsNullOrEmpty(uname))
            {
                sqlStr.Append(" and  CreateUserName = @uname");
                dynamicParams.Add("uname", uname);
            }
            if (minTime.HasValue)
            {
                sqlStr.Append(" and CreateTime >= @minTime");
                dynamicParams.Add("minTime", minTime.Value);
            }
            if (maxTime.HasValue)
            {
                sqlStr.Append(" and  CreateTime <= @maxTime");
                dynamicParams.Add("maxTime", maxTime.Value);
            }
            return Pager(sqlStr.ToString(), dynamicParams,search);
        }
    }
}

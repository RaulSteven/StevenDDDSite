using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Steven.Domain.Infrastructure;
using Steven.Domain.Models;
using Steven.Domain.Enums;
using Dapper;
using Steven.Domain.ViewModels;

namespace Steven.Domain.Repositories
{
    public class JobTaskRepository : Repository<JobTask>, IJobTaskRepository
    {
        /// <summary>
        /// 返回所有的记录
        /// </summary>
        /// <param name="name">搜索关键词</param>
        /// <param name="status"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public Pager<JobTask> GetPager(string name, CommonStatus? status, PageSearchModel search)
        {
            var sql = new StringBuilder(Query());
            var param = new DynamicParameters();
            if (!string.IsNullOrEmpty(name))
            {
                sql.Append(" and Name like @name");
                param.Add("name", $"%{name}%");
            }
            if (status.HasValue)
            {
                sql.Append(" and Status = @status");
                param.Add("status", status.Value);
            }
            return Pager(sql.ToString(), param, search);
        }

        public List<JobTask> GetList()
        {
            return DbConn.Query<JobTask>(Query() + " order by Id desc").ToList();
        }

        public JobTask GetByTaskId(string taskId)
        {
            Guid guid;
            if (!Guid.TryParse(taskId, out guid)) return null;
            return DbConn.QueryFirstOrDefault<JobTask>(Query() + " and TaskId=@taskId", new { taskId });
        }
    }
}

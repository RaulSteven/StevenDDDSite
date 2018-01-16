using System.Collections.Generic;
using System.Threading.Tasks;
using Steven.Domain.Enums;
using Steven.Domain.Infrastructure;
using Steven.Domain.Models;

namespace Steven.Domain.Repositories
{
    public interface IJobTaskRepository : IRepository<JobTask>
    {

        /// <summary>
        /// 返回所有的记录
        /// </summary>
        /// <param name="name">搜索关键词</param>
        /// <param name="status"></param>
        /// <param name="orderDirection"></param>
        /// <param name="pageCurrent">当前页</param>
        /// <param name="pageSize">每页显示总数</param>
        /// <param name="orderField"></param>
        /// <returns></returns>
        Pager<JobTask> GetPager(string name, CommonStatus? status, PageSearchModel search);

        List<JobTask> GetList();

        JobTask GetByTaskId(string taskId);
    }
}

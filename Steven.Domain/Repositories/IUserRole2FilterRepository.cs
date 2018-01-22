using System.Collections.Generic;
using Steven.Domain.Models;
using Steven.Domain.ViewModels;

namespace Steven.Domain.Repositories
{
    public interface IUserRole2FilterRepository : IRepository<UserRole2Filter>
    {
        IEnumerable<UserRole2Filter> GetRoleFilterList(List<long> roleId);

        /// <summary>
        /// 是否存在数据规则
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="source">资源</param>
        /// <returns></returns>
        bool ExistSource(long id, string source);

        IEnumerable<FilterGroupModel> GetLstFilterGroup(long roleId);
    }
}

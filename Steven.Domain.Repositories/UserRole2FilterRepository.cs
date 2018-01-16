using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Steven.Domain.Infrastructure;
using Steven.Domain.Models;
using Dapper;
using Steven.Domain.ViewModels;

namespace Steven.Domain.Repositories
{
    public class UserRole2FilterRepository : Repository<UserRole2Filter>, IUserRole2FilterRepository
    {
        public IEnumerable<UserRole2Filter> GetRoleFilterList(List<long> roleId)
        {
            return DbConn.Query<UserRole2Filter>(Query() + " and RoleId in @roleId", new { roleId =roleId});
        }

        /// <summary>
        /// 是否存在数据规则
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="source">资源</param>
        /// <returns></returns>
        public bool ExistSource(long id, string source)
        {
            var count =  DbConn.QueryFirst<int>(Count() + " and Id != @id and Source=@source", new { id=id, source=source });
            return count > 0;
        }

        public IEnumerable<FilterGroupModel> GetLstFilterGroup(long roleId)
        {
            var sql = Query("Id,Source,FilterGroups,Name") + " and RoleId =@roleId";
            return DbConn.Query<FilterGroupModel>(sql, new { roleId =roleId});
        }
    }
}

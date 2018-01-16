using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Steven.Domain.Infrastructure;
using Steven.Domain.Models;

using System.Data.SqlClient;
using Steven.Domain.Enums;
using Dapper;
using Steven.Domain.ViewModels;

namespace Steven.Domain.Repositories
{
    public class UserRoleRepository : Repository<UserRole>, IUserRoleRepository
    {
        public void  Delete(long id)
        {
            //删除角色
             DbConn.Execute($"delete from {GetTableName()} where Id = @id", new { id });

            //删除关联用户信息
             DbConn.Execute($"delete from User2Role where RoleId = @id", new { id });

            //删除关联菜单信息
             DbConn.Execute($"delete from UserRole2Menu where RoleId = @id", new { id });

            //删除关联部门信息
             DbConn.Execute($"delete from UserRole2Apartment where RoleId = @id", new { id });
        }

        public Pager<UserRole> GetList(string keyword,PageSearchModel search)
        {
            var sql = new StringBuilder(AdminQuery());
            var param = new DynamicParameters();
            if (!string.IsNullOrEmpty(keyword))
            {
                sql.Append(" and (Name like @keyword or Remark like @keyword)");
                param.Add("keyword", $"%{keyword}%");
            }
            return Pager(sql.ToString(), param,search);
        }

        public IEnumerable<UserRole> GetList()
        {
            return DbConn.Query<UserRole>(Query() + " order by Sort");
        }
    }
}

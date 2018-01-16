using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.Infrastructure;
using Steven.Domain.Models;
using Dapper;
namespace Steven.Domain.Repositories
{
    public class SysCaseRepository : Repository<SysCase>, ISysCaseRepository
    {
        public IEnumerable<SysCase> GetAll()
        {
            return DbConn.Query<SysCase>(Query()+" order by Sort desc,Id desc");
        }

        public Pager<SysCase> GetPager(string name, PageSearchModel search)
        {
            var sql = AdminQuery();
            var param = new DynamicParameters();
            if (!string.IsNullOrEmpty(name))
            {
                sql += " and Name like @name";
                param.Add("name", $"%{name}%");
            }
            return Pager(sql, param, search);
        }
    }
}

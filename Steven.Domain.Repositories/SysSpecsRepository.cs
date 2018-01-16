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
    public class SysSpecsRepository : Repository<SysSpecs>, ISysSpecsRepository
    {
        public Pager<SysSpecs> GetPager(string name, PageSearchModel search)
        {
            var sql = AdminQuery() + " and Name like @name";
            var param = new DynamicParameters();
            param.Add("name", $"%{name}%");
            return Pager(sql, param, search);
        }

        public IEnumerable<string> GetLstName()
        {
            var sql = AdminQuery("Name") + " order by Sort desc,Id desc";
            return DbConn.Query<string>(sql);
        }
    }
}

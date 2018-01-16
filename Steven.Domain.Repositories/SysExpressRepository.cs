using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Steven.Domain.Enums;
using Steven.Domain.Infrastructure;
using Steven.Domain.Models;
using Dapper;

namespace Steven.Domain.Repositories
{
    public class SysExpressRepository : Repository<SysExpress>, ISysExpressRepository
    {
        public Pager<SysExpress> GetPager(string name, PageSearchModel search)
        {
            var sql = AdminQuery() + " and Name like @name";
            var param = new DynamicParameters();
            param.Add("name", $"%{name}%");
            return Pager(sql, param, search);
        }
        public List<SelectListItem> GetSelectList()
        {
            var query = DbConn.Query<SysExpress>(AdminQuery()+" order by Sort desc");
            var list = query
                                  .Select(d => new SelectListItem { Text = d.Name, Value = d.Id.ToString() })
                                  .ToList();
            return list;
        }
    }
}

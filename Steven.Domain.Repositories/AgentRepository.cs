using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.Models;
using Dapper;
using Steven.Domain.Infrastructure;
using Steven.Domain.ViewModels;
using Steven.Domain.Enums;
using Steven.Core.Utilities;

namespace Steven.Domain.Repositories
{
    public class AgentRepository : Repository<Agent>, IAgentRepository
    {
        public Agent GetIncludeUser(long id)
        {
            var sql = @"select * from Agent a 
            left join Users u on a.UserId = u.Id
            where a.Id =@id";
            var obj = DbConn.Query<Agent, Users, Agent>(sql,
                (agent, user) => { agent.User = user; return agent; },
                new { id });
            return obj.FirstOrDefault();
        }

        public Pager<AgentModel> GetPager(string keyword, PageSearchModel search)
        {
            var sql = new StringBuilder();
            sql.Append(@"select a.Id,a.Name,u.LoginName,u.RealName,u.CommonStatus,u.UpdateTime,u.LoginCount from Agent a 
                left join Users u on a.UserId = u.Id 
                where u.UserGroup=@agent ");
            var param = new DynamicParameters();
            param.Add("agent", UserGroup.Agent);
            if (!string.IsNullOrEmpty(keyword))
            {
                sql.Append(" and (Name like @keyword or Remark like @keyword)");
                param.Add("keyword", $"%{keyword}%");
            }
            return Pager<AgentModel>(sql.ToString(), param, search);
        }

        public int BatchDele(string ids)
        {
            var idArra = StringUtility.ConvertToBigIntArray(ids, ',');
            if (idArra == null || idArra.Length == 0)
            {
                return 0;
            }
            //先删除用户，在删除代理数据
            var sql = AdminQuery("UserId") + " and Id in @ids";
            var lstUserId = DbConn.Query<long>(sql, new { ids = idArra });

            BatchDele(TableSource.Users, lstUserId);
            var count = BatchDele(TableSource.Agent, idArra);
            return count;
        }

        public IEnumerable<TypeaheadModel> GetList()
        {
            var sql = AdminQuery("Id,Name");
            var list = DbConn.Query<TypeaheadModel>(sql);
            return list;
        }
    }
}

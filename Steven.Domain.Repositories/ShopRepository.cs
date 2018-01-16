using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.Models;
using Steven.Domain.Infrastructure;
using Steven.Domain.ViewModels;
using Dapper;
using Steven.Domain.Enums;
using Steven.Core.Utilities;

namespace Steven.Domain.Repositories
{
    public class ShopRepository : Repository<Shop>, IShopRepository
    {
        public Shop GetIncludeUser(long id)
        {
            var sql = @"select * from Shop a 
            left join Users u on a.UserId = u.Id
            where a.Id =@id";
            var obj = DbConn.Query<Shop, Users, Shop>(sql,
                (shop, user) => { shop.User = user; return shop; },
                new { id });
            return obj.FirstOrDefault();
        }

        public Pager<ShopModel> GetPager(string keyword, long? agentId, PageSearchModel search)
        {
            var sql = new StringBuilder();
            sql.Append(@"select a.Id,a.Name,a.KefuPhone,u.LoginName,u.RealName,u.CommonStatus,u.UpdateTime ,ag.Name as AgentName
                from Shop a left join Users u on a.UserId = u.Id 
                left join Agent ag on ag.Id = a.AgentId
                where u.UserGroup=@agent ");
            var param = new DynamicParameters();
            param.Add("agent", UserGroup.Shop);
            if (!string.IsNullOrEmpty(keyword))
            {
                sql.Append(" and (Name like @keyword or Remark like @keyword or KefuPhone like @keyword or LoginName like @keyword or RealName like @keyword)");
                param.Add("keyword", $"%{keyword}%");
            }
            if (agentId.HasValue)
            {
                sql.Append(" and AgentId=@agentId");
                param.Add("agentId", agentId.Value);
            }
            return Pager<ShopModel>(sql.ToString(), param, search);
        }

        public int BatchDele(string ids)
        {
            var idArra = StringUtility.ConvertToBigIntArray(ids, ',');
            if (idArra == null || idArra.Length == 0)
            {
                return 0;
            }

            return Trans(trans =>
            {
                var sql = AdminQuery("UserId") + " and Id in @ids";
                var lstUserId = DbConn.Query<long>(sql, new { ids = idArra },trans);
                BatchDele(TableSource.Users, lstUserId, trans);
                var count = BatchDele(TableSource.Shop, idArra, trans);

                var deleApp = "delete from ShopAppInfo where ShopId in @shopIds";
                DbConn.Execute(deleApp, new { shopIds = idArra }, trans);

                var deleProduct = "delete from Product where ShopId in @shopIds";
                DbConn.Execute(deleProduct, new { shopIds = idArra }, trans);

                var deleClassify = "delete from ProductClassify where ShopId in @shopIds";
                DbConn.Execute(deleClassify, new { shopIds = idArra }, trans);

                var deleSpecs = "delete from ProductSpecs where ShopId in @shopIds";
                DbConn.Execute(deleSpecs, new { shopIds = idArra }, trans);

                var deleTemp = "delete from ShopTemplate where ShopId in @shopIds";
                DbConn.Execute(deleTemp, new { shopIds = idArra }, trans);

                return count;
            });

        }

        public IEnumerable<TypeaheadModel> GetList()
        {
            var sql = AdminQuery("Id,Name");
            var list = DbConn.Query<TypeaheadModel>(sql);
            return list;
        }

        public Shop GetByUserId(long userId)
        {
            var sql = Query() + " and UserId = @userId";
            return DbConn.QueryFirstOrDefault<Shop>(sql, new { userId });
        }
    }
}

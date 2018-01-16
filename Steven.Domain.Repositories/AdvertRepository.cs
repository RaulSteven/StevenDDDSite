using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Steven.Domain.Enums;
using Steven.Domain.Infrastructure;
using Steven.Domain.Models;
using Steven.Domain.ViewModels;

using Steven.Core.Cache;
using Dapper;

namespace Steven.Domain.Repositories
{
    public class AdvertRepository : Repository<Advert>, IAdvertRepository
    {
        public Pager<AdvertBizModel> GetList(long adPosId, string name, AdvertStatus? status, PageSearchModel search)
        {
            var strBuilder = new StringBuilder(AdminQuery());
            var param = new DynamicParameters();
            strBuilder.Append(" and AdPosId = @adPosId");
            param.Add("adPosId", adPosId);

            if (!string.IsNullOrEmpty(name))
            {
                strBuilder.Append(" and Name like @name");
                param.Add("name", $"%{name}%");
            }
            if (status.HasValue)
            {
                strBuilder.Append(" and AdvertStatus = @status");
                param.Add("status", status.Value);
            }
            return Pager<AdvertBizModel>(strBuilder.ToString(), param, search);

        }

        #region Cache
        public ICacheManager Cache { get; set; }
        private string GetListByAdPostIdKey
        {
            get
            {
                return CacheKey + "-GetListByAdPostIdCache-{0}-{1}-{2}";
            }
        }

        public List<Advert> GetListByAdPostIdCache(long adPosId, AdvertType? type, int takeSize)
        {
            return Cache.Get(string.Format(GetListByAdPostIdKey, adPosId, type, takeSize), (Func<List<Advert>>)(() =>
            {
                var strBuilder = new StringBuilder();
                strBuilder.Append($@" and AdPosId={adPosId} 
                                      and AdvertStatus={AdvertStatus.Publish}
                                      and (EndTime is null || EndTime >= '{DateTime.Now})'");
                var param = new DynamicParameters();

                if (type.HasValue)
                {
                    strBuilder.Append(" and AdType = @type");
                    param.Add("type", type.Value);
                }
                if (takeSize > 0)
                {
                    strBuilder.Append($"select Top {takeSize} * from {GetTableName()} where CommonStatus != {CommonStatus.Deleted}");
                }
                else
                {
                    strBuilder.Insert(0, base.Query());
                }
                //Sort
                strBuilder.Append(" order by SortIndex,CreateTime desc");
                var list = DbConn.Query<Advert>(strBuilder.ToString(), param);
                return list.ToList();
               
            }));
        }

        public override void RemoveCache(Advert entity)
        {
            Cache.RemoveByPattern(CacheKey);
        }

        #endregion
    }
}

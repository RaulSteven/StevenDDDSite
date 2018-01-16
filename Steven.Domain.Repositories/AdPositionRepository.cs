using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Steven.Core.Extensions;
using Steven.Domain.Enums;
using Steven.Domain.Infrastructure;
using Steven.Domain.Models;
using Steven.Domain.ViewModels;
using Steven.Core.Cache;
using Dapper;

namespace Steven.Domain.Repositories
{
    public class AdPositionRepository : Repository<AdPosition>, IAdPositionRepository
    {
        #region Cache

        public ICacheManager Cache { get; set; }

        public string GetByPosKeyCacheKey
        {
            get
            {
                return CacheKey + "GetByPosKeyCache-{0}";
            }
        }

        public AdPosition GetByPosKeyCache(AdPosKey value)
        {
            var result = Cache.Get(string.Format(GetByPosKeyCacheKey, value), (Func<AdPosition>)(() =>
            {
                var sql = $"{base.Query()} and Code = @code";
                var model = DbConn.QueryFirstOrDefault<AdPosition>(sql, new { code = value });
                return model ?? new AdPosition();
            }));
            return result;
        }

        public override void RemoveCache(AdPosition entity)
        {
            Cache.RemoveByPattern(CacheKey);
        }

        #endregion

    }
}

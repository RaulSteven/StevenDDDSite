
using System.Collections.Generic;
using System.Linq;
using Steven.Domain.Enums;
using Steven.Domain.Models;
using Dapper;

namespace Steven.Domain.Repositories
{
    public class WeixinNotifyRepository : Repository<WeixinNotify>, IWeixinNotifyRepository
    {
        public List<WeixinNotify> GetNeedSendList()
        {
            return DbConn.Query<WeixinNotify>(Query() + " and Status=@status", new { status = CommonStatus.Disabled }).ToList();
        }
    }
}
using System.Collections.Generic;
using Steven.Domain.Models;

namespace Steven.Domain.Repositories
{
    public interface IWeixinNotifyRepository:IRepository<WeixinNotify>
    {
        List<WeixinNotify> GetNeedSendList();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.Models;

namespace Steven.Domain.Services
{
    public interface IWeixinPaySvc
    {
        bool GetPrepayId(string openId, ShopAppInfo shopAppInfo, ShopOrder order,
            ref string prePayId, ref string nonceStr);

        string GetPaySign(string nonceStr, string timeStamp, string prePayId, ShopAppInfo shopAppInfo);

        string GetNoncestr();

        string GetTimestamp();
    }
}

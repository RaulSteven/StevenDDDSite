using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.Enums;
using Steven.Domain.Infrastructure;
using Steven.Domain.Models;

namespace Steven.Domain.Repositories
{
    public interface IUsersMediaRepository: IRepository<UsersMedia>
    {
        /// <summary>
        /// 用户是否已经有该平台的登录信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="openId"></param>
        /// <param name="unionId"></param>
        /// <param name="headImg"></param>
        /// <returns></returns>
        UsersMedia Save(long userId, string userName, string openId, string unionId, string headImg = "");

        UsersMedia GetByOpenIdAsync(string openId);
        UsersMedia GetByUnionIdAsync(string userUnionId);
        UsersMedia GetByUserId(long userId);
    }
}

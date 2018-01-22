using System.Collections.Generic;
using Steven.Domain.Models;

namespace Steven.Domain.Repositories
{
    public interface IUsersResetPwdRepository:IRepository<UsersResetPwd>
    {
        UsersResetPwd Get(string email, string code);

        IEnumerable<UsersResetPwd> GetUnUseList(long userId);
    }
}
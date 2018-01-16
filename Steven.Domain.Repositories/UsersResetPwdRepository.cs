using System.Collections.Generic;
using System.Linq;

using Steven.Domain.Infrastructure;
using Steven.Domain.Models;
using Dapper;

namespace Steven.Domain.Repositories
{
    public class UsersResetPwdRepository : Repository<UsersResetPwd>,IUsersResetPwdRepository
    {
        public IUsersRepository UsersRepository { get; set; }

        public  UsersResetPwd Get(string email, string code)
        {
            var user =   UsersRepository.GetByEmail(email);
            if (user == null)
            {
                return null;
            }
            var obj = DbConn.QueryFirstOrDefault<UsersResetPwd>(Query() + " and ResetUserId=@userId and Code=@code",
                new { userId=user.Id,code});
            
            return obj;
        }

        public IEnumerable<UsersResetPwd> GetUnUseList(long userId)
        {
            return DbConn.Query<UsersResetPwd>(Query() + " and ResetUserId=@userId and Used = 0");
        }
    }
}
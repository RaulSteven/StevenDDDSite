using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.APIModels;
using Steven.Domain.Models;
using Dapper;

namespace Steven.Domain.Repositories
{
    public class UserShipingAddressRepository:Repository<UserShipingAddress>,IUserShipingAddressRepository
    {
        public UserShipingAddress GetByUserId(long userId)
        {
            var model = DbConn.QueryFirstOrDefault<UserShipingAddress>(Query() + " and UserId = @userId", new { userId });
            return model;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.APIModels;
using Steven.Domain.Models;
namespace Steven.Domain.Repositories
{
    public interface IUserShipingAddressRepository:IRepository<UserShipingAddress>
    {
        UserShipingAddress GetByUserId(long userId);
    }
}

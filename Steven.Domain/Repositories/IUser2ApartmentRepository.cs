using System.Collections.Generic;
using Steven.Domain.Models;

namespace Steven.Domain.Repositories
{
    public interface IUser2ApartmentRepository:IRepository<User2Apartment>
    {
        IEnumerable<long> GetLstApartId(long userId);
        void SaveList(string userIds, string apartIds);

        IEnumerable<long> GetLstUserId(IEnumerable<long> lstApartId);

    }
}

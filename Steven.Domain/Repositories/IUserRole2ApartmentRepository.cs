using System.Collections.Generic;
using System.Threading.Tasks;
using Steven.Domain.Models;

namespace Steven.Domain.Repositories
{
    public interface IUserRole2ApartmentRepository : IRepository<UserRole2Apartment>
    {
        IEnumerable<long> GetLstRoleId(long apartId);
        void SaveList(long apartId, long[] roleIds);

        IEnumerable<long> GetLstRoleId(IEnumerable<long> apartId);
        IEnumerable<long> GetLstApartId(long roleId);
    }
}

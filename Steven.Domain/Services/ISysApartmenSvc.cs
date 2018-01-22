using Steven.Domain.Models;

namespace Steven.Domain.Services
{
    public interface ISysApartmenSvc
    {
        void Save(SysApartment apart,long[] lstRoleIds);
    }
}

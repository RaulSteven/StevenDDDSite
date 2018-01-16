using Steven.Domain.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Steven.Domain.Models;

namespace Steven.Domain.Services
{
    public interface ISysApartmenSvc
    {
        void Save(SysApartment apart,long[] lstRoleIds);
    }
}

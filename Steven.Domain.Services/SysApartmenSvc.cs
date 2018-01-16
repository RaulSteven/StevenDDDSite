using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.ViewModels;
using Steven.Domain.Repositories;
using Steven.Domain.Models;

namespace Steven.Domain.Services
{
    public class SysApartmenSvc : ISysApartmenSvc
    {
        public ISysApartmentRepository SysApartRepository { get; set; }
        public IUser2ApartmentRepository User2ApartRepository { get; set; }
        public IUserRole2ApartmentRepository UserRole2ApartRepository { get; set; }

        public void Save(SysApartment apart, long[] lstRoleIds)
        {
            SysApartRepository.Save(apart);

            UserRole2ApartRepository.SaveList(apart.Id, lstRoleIds);
        }


    }
}

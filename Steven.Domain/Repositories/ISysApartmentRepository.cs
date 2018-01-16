using System.Collections.Generic;
using System.Threading.Tasks;
using Steven.Domain.Models;
using Steven.Domain.ViewModels;

namespace Steven.Domain.Repositories
{
    public interface ISysApartmentRepository : IRepository<SysApartment>
    {
        List<JsTreeJsonModel> GetJsonList();

        string Delete(long id);

        ///// <summary>
        ///// 返回部门所有的记录及部门下所有人员，用于树形结构
        ///// </summary>
        ///// <param name="userApartmentQuery">部门用户信息</param>
        ///// <param name="userQuery">用户信息</param>
        ///// <returns></returns>
        //Task<List<SysApartZTreeModel>> GetListByZTree(IQueryable<User2Apartment> userApartmentQuery,
        //    IQueryable<Users> userQuery);

        int GetIndexOfParent(SysApartment apart);
    }
}

using Steven.Domain.Enums;
using Steven.Domain.Models;
using Steven.Domain.ViewModels;
using System.Collections.Generic;


namespace Steven.Domain.Services
{
    public interface IUserRoleSvc
    {
        /// <summary>
        /// 获取该用户的角色Id = 用户x角色 + 用户x部门x角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<long> GetRoleIdList(long userId);

        void SaveList(UserRole role, string menuIds);

        void ClearRoleUserCache(long roleId);
        
        IEnumerable<DropdownItemModel> GetResList(FilterCurrent curr);

        IEnumerable<FilterGroupModel> GetFilterList(long roleId);

        IEnumerable<Users> GetLstUsers(string roleIds);
    }
}

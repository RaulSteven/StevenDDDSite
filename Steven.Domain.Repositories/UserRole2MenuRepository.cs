using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Steven.Domain.Infrastructure;
using Steven.Domain.Models;
using Steven.Core.Utilities;
using Steven.Domain.Enums;
using Dapper;
using Dapper.Contrib.Extensions;

namespace Steven.Domain.Repositories
{
    public class UserRole2MenuRepository : Repository<UserRole2Menu>, IUserRole2MenuRepository
    {
        public void  SaveList(long roleId, string menuIds)
        {
            var idStrArr = menuIds.Split(',');
            var idList = idStrArr.Where(m => !m.Contains("_")).Select(m => StringUtility.ConvertToBigInt(m));
            var oldList =  DbConn.Query<UserRole2Menu>(Query() + " and RoleId =@roleId", new { roleId });
            //删除不存在的
            var oldDeleList = oldList.Where(m => !idList.Contains(m.MenuId))
                .Select(m => m.Id)
                .ToArray();
             DbConn.Execute($"Delete from {GetTableName()} where Id in @ids", new { ids = oldDeleList });

            //依然保持的数据
            var oldHoldList = oldList.Where(m => idList.Contains(m.MenuId))
                .Select(m => m.MenuId)
                .ToList();

            //新增的数据
            var newIdList = idList.Except(oldHoldList);
            foreach (var menuId in newIdList)
            {
                var role2menu = new UserRole2Menu()
                {
                    RoleId = roleId,
                    MenuId = menuId,
                };
                Insert(role2menu);
            }

        }

        private UserRole2Menu Get(long roleId,long menuId)
        {
            return DbConn.QueryFirstOrDefault<UserRole2Menu>(Query() + " and RoleId=@roleId and MenuId =@menuId", new { roleId, menuId });
        }

        public IEnumerable<long> GetLstMenuId(long roleId)
        {
            return DbConn.Query<long>(Query("MenuId") + " and RoleId =@roleId", new { roleId });
        }

        public  bool UpdateFilterGroups(long id, string filterGroups)
        {
            var obj =  Get(id);
            if (obj == null)
            {
                return false;
            }
            obj.FilterGroups = filterGroups;
             Update(obj);
            return true;
        }

        public void  SaveRole2MenuButtons(long roleId, string btnIds)
        {
            #region check params

            if (string.IsNullOrEmpty(btnIds))
            {
                return;
            }
            var idArr = btnIds.Split(',');
            if (idArr == null || idArr.Length == 0)
            {
                return;
            }
            if (roleId == 0)
            {
                return;
            }

            #endregion

            var menuAllList =  DbConn.Query<UserRole2Menu>(Query() + " and RoleId=@roleId", new { roleId });
            
            var role2MenuList = new List<UserRole2Menu>();
            foreach (var r2mId in idArr)
            {
                var r2mArr = StringUtility.ConvertToBigIntArray(r2mId, '_');
                var role2Menu = role2MenuList.FirstOrDefault(m => m.Id == r2mArr[0]);
                if (role2Menu == null)
                {
                    role2Menu = menuAllList.FirstOrDefault(m=>m.Id == r2mArr[0]);
                    role2Menu.Buttons = SysButton.None;
                    role2MenuList.Add(role2Menu);
                }
                role2Menu.Buttons = role2Menu.Buttons | (SysButton)r2mArr[1];
                 Update(role2Menu);
            }
            var noneButtonMenuList = menuAllList
                .Where(m => !role2MenuList.Any(r => r.Id == m.Id))
                .Select(m => m.Id)
                .ToArray();
             DbConn.Execute($"Update {GetTableName()} set Buttons = @none where Id in @ids", new { none = SysButton.None, ids = noneButtonMenuList });
        }
    }
}

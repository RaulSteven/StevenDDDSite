using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.ViewModels;
using Steven.Domain.Repositories;
using Steven.Domain.Models;
using Steven.Domain.Enums;
using Newtonsoft.Json;
using Dapper;
using Steven.Core.Utilities;

namespace Steven.Domain.Services
{
    public class SysMenuSvc : BaseSvc, ISysMenuSvc
    {
        public ISysMenuRepository SysMenuRepository { get; set; }
        public IUserRole2MenuRepository UserRole2MenuReopsitory { get; set; }

        public List<JsTreeJsonModel> GetJsonList(long roleId)
        {
            var menuList = SysMenuRepository.GetJsonList();

            var menuIdList = UserRole2MenuReopsitory.GetLstMenuId(roleId);
            //TODO:jstree
            return menuList;
        }

        public IEnumerable<SysUserRole2MenuModel> GetRole2MenuList(long roleId)
        {
            var sql = @"select r2m.Id,menu.Buttons,menu.Id as MenuId,menu.Name as MenuName,r2m.RoleId,r2m.Buttons as SelectedButtons,menu.TreePath as MenuTreePath 
                        from UserRole2Menu r2m,SysMenu menu 
                        where r2m.MenuId = menu.Id
                        and r2m.RoleId = @roleId
                        order by MenuTreePath";
            var list = DbConn.Query<SysUserRole2MenuModel>(sql, new { roleId });
            return list;
        }

        private void SetChildren(UserMenuModel model, IEnumerable<UserMenuModel> all)
        {
            model.Children = all.Where(m => m.Pid == model.Id).ToList();
            if (model.HasChildren)
            {
                foreach (var item in model.Children)
                {
                    SetChildren(item, all);
                }
            }
        }

        public IEnumerable<UserMenuModel> GetList(List<long> lstRoleId)
        {
            if (lstRoleId == null || !lstRoleId.Any())
            {
                return new List<UserMenuModel>();
            }

            var lstPageMenuId = DbConn.Query<long>("select MenuId from UserRole2Menu where RoleId in @roleIds", new { roleIds = lstRoleId });
            var lstTreePath = DbConn.Query<string>("Select TreePath from SysMenu where Id in @menuIds", new { menuIds = lstPageMenuId });
            var lstMenuId = new List<long>();
            foreach (var treePath in lstTreePath)
            {
                var idArr = StringUtility.ConvertToBigIntArray(treePath, '.');
                lstMenuId.AddRange(idArr);
            }
            lstMenuId = lstMenuId.Distinct().ToList();

            var sql = @"select * from  SysMenu menu
                        where Id in @menuIds";
            var menuList = DbConn.Query<SysMenu>(sql, new { menuIds = lstMenuId });
            var r2mList = DbConn.Query<UserRole2Menu>("select * from UserRole2Menu where RoleId in @roleIds", new { roleIds = lstRoleId });

            List<UserMenuModel> allList = new List<UserMenuModel>();
            foreach (var menu in menuList)
            {
                var menuModel = allList.FirstOrDefault(m => m.Id == menu.Id);

                if (menuModel == null)
                {
                    menuModel = new UserMenuModel()
                    {
                        Id = menu.Id,
                        Icon = menu.Icon,
                        Name = menu.Name,
                        Pid = menu.Pid,
                        Sort = menu.Sort,
                        Source = menu.Source,
                        Url = menu.Url,
                        Buttons = menu.Buttons
                    };
                    allList.Add(menuModel);
                }
                FilterGroup filterGroup = null;
                var r2m = r2mList.FirstOrDefault(m => m.MenuId == menu.Id);
                if (r2m != null)
                {
                    menuModel.Buttons = menuModel.Buttons | menu.Buttons;
                    if (!string.IsNullOrEmpty(r2m.FilterGroups))
                    {
                        filterGroup = JsonConvert.DeserializeObject<FilterGroup>(r2m.FilterGroups);
                    }
                }
                if (menuModel.FilterGroup == null)
                {
                    menuModel.FilterGroup = filterGroup;
                }
                else if (filterGroup != null)
                {
                    menuModel.FilterGroup = new FilterGroup()
                    {
                        Op = FilterGroupOp.Or,
                        ListGroup = new List<FilterGroup>()
                        {
                            menuModel.FilterGroup,
                            filterGroup
                        }
                    };
                }
            }
            var list = allList.Where(m => m.Pid == 0).OrderBy(m=>m.Sort);
            foreach (var item in list)
            {
                SetChildren(item, allList);
            }

            return list;
        }
    }
}

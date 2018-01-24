using Steven.Domain.Models;
using Steven.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Steven.Domain.Infrastructure.SysUser
{
    [Serializable]
    public class AdminUserModel : ISysUserModel
    {
        #region properties

        public string GId
        {
            get;
            set;
        }

        public long HeadImageId
        {
            get;
            set;
        }

        public long UserId
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }

        public string RealName { get; set; }

        public List<long> RoleIdList { get; set; }

        public List<UserMenuModel> MenuList { get; set; }

        //public UserMenuModel FirstMenu { get; set; }
        //public UserMenuModel SecMenu { get; set; }
        //public UserMenuModel ThiredMenu { get; set; }

        public UserMenuModel CurrPage { get; set; }

        public List<long> SysApartIdList { get; set; }

        public List<UserRole2Filter> UserRoleFilterList { get; set; }

        #endregion

        #region constructor
        public AdminUserModel(Users user, List<long> roleIdList, IEnumerable<UserMenuModel> menuList, IEnumerable<long> apartIdList, IEnumerable<UserRole2Filter> userRoleFilterList)
        {
            UserId = user.Id;
            UserName = user.LoginName;
            RealName = user.RealName;
            HeadImageId = user.HeadImageId;
            GId = user.GId;
            RoleIdList = roleIdList;
            MenuList = menuList.ToList();
            SysApartIdList = apartIdList.ToList();
            UserRoleFilterList = userRoleFilterList.ToList();
        }

        public AdminUserModel()
        {

        }
        #endregion

        public void FindCurrentMenu(string targetUrl)
        {
            if (string.IsNullOrEmpty(targetUrl))
            {
                return;
            }
            targetUrl = targetUrl.ToLower();
            //sysMenu的Url保存的时候自动转为小写
            foreach (var menu in MenuList)
            {
                if (targetUrl.Equals(menu.Url, StringComparison.OrdinalIgnoreCase))
                {
                    CurrPage = menu;
                    return;
                }
                if (menu.HasChildren)
                {
                    foreach (var secMenu in menu.Children)
                    {
                        if (targetUrl.Equals(secMenu.Url, StringComparison.OrdinalIgnoreCase))
                        {
                            CurrPage = secMenu;
                            return;
                        }
                        if (secMenu.HasChildren)
                        {
                            foreach (var thirdMenu in secMenu.Children)
                            {
                                if (targetUrl.Equals(thirdMenu.Url, StringComparison.OrdinalIgnoreCase))
                                {
                                    CurrPage = thirdMenu;
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }

        public string IsCurrPage(UserMenuModel menu)
        {
            if (CurrPage == null)
            {
                return "";
            }
            if (CurrPage.Id == menu.Id || 
                (CurrPage.Parent != null && CurrPage.Parent.Id == menu.Id) ||
                (CurrPage.Parent != null && CurrPage.Parent.Parent!=null && CurrPage.Parent.Parent.Id == menu.Id))
            {
                return "class=active";
            }
            return "";
        }

    }
}
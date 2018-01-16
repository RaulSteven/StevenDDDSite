using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.Models;
using Steven.Core.Cache;
using Steven.Domain.Repositories;
using System.Web;
using Steven.Domain.ViewModels;
using System.Web.Security;
using Steven.Core.Utilities;
using Steven.Domain.Infrastructure;
using System.Threading;
using Steven.Domain.APIModels;
using log4net;
using Steven.Domain.Infrastructure.SysUser;
using Steven.Domain.Enums;
using System.Security.Principal;

namespace Steven.Domain.Services
{
    public class FormsAuthenticationSvc : IFormsAuthenticationSvc
    {
        //public ICacheManager Cache { get; set; }
        //public const int CacheTime = 10800;
        public IUsersRepository UserRep { get; set; }
        public IUserRoleSvc UserRoleSvc { get; set; }
        public ISysMenuSvc SysMenuSvc { get; set; }
        public IUser2ApartmentRepository User2ApartRepository { get; set; }
        public IUserRole2FilterRepository UserRole2FilterRepository { get; set; }
        public IShopRepository ShopRepository { get; set; }
        public IAttachmentSvc AttachmentSvc { get; set; }

        private const string CurrentShopEncryptKey = "BeiLin$Shop$key";

        public void CreateAuthenticationTicket(Users user, HttpResponseBase response, HttpContextBase httpContextBase, bool remember)
        {
            var serializeModel = GetModel(user);
            var userGroup = user.UserGroup;
            UserRep.AddUserCache(serializeModel);
            var expiration = remember ? DateTime.Now.AddYears(1) : DateTime.Now.AddMinutes(FormsAuthentication.Timeout.TotalMinutes);
            string userData = $"{user.GId}|{UserRep.GetIP()}|{userGroup}";
            var authTicket = new FormsAuthenticationTicket(
              1, user.GId.ToString(), DateTime.Now, expiration, remember, userData);
            string encTicket = FormsAuthentication.Encrypt(authTicket);

            CookieUtils.AddCookie(GetCookieName(), encTicket, expiration);
            setPrinciple(serializeModel, userGroup);
        }

        #region get models
        private ISysUserModel GetModel(Users user)
        {
            switch (user.UserGroup)
            {
                case UserGroup.Admin:
                    return GetAdminUserModel(user);
                case UserGroup.Member:
                    return GetMemberUserModel(user);
                case UserGroup.Agent:
                    return null;
                case UserGroup.Shop:
                    return GetShopUserModel(user);
                default:
                    return null;
            }
        }

        public ShopUserModel GetShopUserModel(Users user)
        {
            var shop = ShopRepository.GetByUserId(user.Id);
            return new ShopUserModel(user, shop);
        }

        private MemberUserModel GetMemberUserModel(Users user)
        {
            return new MemberUserModel(user);
        }

        private AdminUserModel GetAdminUserModel(Users user)
        {
            var roleIdList = UserRoleSvc.GetRoleIdList(user.Id);
            var menuList = SysMenuSvc.GetList(roleIdList);
            var apartIdList = User2ApartRepository.GetLstApartId(user.Id);
            var userFilterList = UserRole2FilterRepository.GetRoleFilterList(roleIdList);
            var model = new AdminUserModel(user, roleIdList, menuList, apartIdList, userFilterList);
            return model;
        }

        private ISysUserModel GetModelFromCache(string userGid, UserGroup userGroup)
        {
            ISysUserModel serializeModel = UserRep.GetByCache(userGid);
            return serializeModel;
        }
        #endregion

        #region set Principle

        private void setPrinciple(ISysUserModel model, UserGroup userGroup)
        {
            ISysUser sysUser = null;
            switch (userGroup)
            {
                case UserGroup.Admin:
                    sysUser = new AdminUser((AdminUserModel)model);
                    break;
                case UserGroup.Member:
                    sysUser = new MemberUser((MemberUserModel)model);
                    break;
                case UserGroup.Agent:
                    break;
                case UserGroup.Shop:
                    sysUser = new ShopUser((ShopUserModel)model);
                    break;
                default:
                    break;
            }
            if (sysUser != null)
            {
                HttpContext.Current.User = sysUser;
                Thread.CurrentPrincipal = sysUser;
            }
        }

        #endregion

        #region cookieName
        public static string GetCookieName()
        {
            var cookieName = FormsAuthentication.FormsCookieName;
            if (HttpContext.Current.Request.Url.AbsolutePath.StartsWith("/admin", true, null))
            {
                cookieName += "_a";
            }
            else if (HttpContext.Current.Request.Url.AbsolutePath.StartsWith("/shop", true, null))
            {
                cookieName += "_shop";
            }
            return cookieName;
        }

        public string GetUserNameCookieKey()
        {
            return FormsAuthentication.FormsCookieName + "_uname";
        }

        #endregion


        public void FromAuthenticationTicket(HttpCookie cookie)
        {
            FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(cookie.Value);
            if (authTicket == null)
            {
                return;
            }
            var userData = authTicket.UserData;
            var userDataArr = userData.Split('|');
            if (userDataArr.Length != 3
                || UserRep.GetIP() != userDataArr[1])
            {
                FormsAuthentication.SignOut();
                return;
            }
            if (FormsAuthentication.SlidingExpiration)
            {
                var expiration = DateTime.Now.AddMinutes(FormsAuthentication.Timeout.TotalMinutes);
                cookie.Expires = expiration;
                cookie.HttpOnly = true;
                HttpContext.Current.Response.Cookies.Set(cookie);
            }
            var userGid = userDataArr[0];
            var userGroup = (UserGroup)Enum.Parse(typeof(UserGroup), userDataArr[2]);
            ISysUserModel serializeModel = UserRep.GetByCache(userGid);
            if (serializeModel == null)
            {
                var user = UserRep.GetByGid(userGid);
                if (user == null)
                {
                    UserRep.RemoveUserCache(userGid);
                    FormsAuthentication.SignOut();
                    return;
                }
                serializeModel = GetModel(user);
                UserRep.AddUserCache(serializeModel);

            }

            setPrinciple(serializeModel, userGroup);
        }


        public void LogOut(IPrincipal user)
        {
            var cookieName = GetCookieName();
            CookieUtils.RemoveCookie(cookieName);
            var sysUser = user as ISysUser;
            if (user != null)
            {
                UserRep.RemoveUserCache(sysUser.UserModel.GId);
            }
            FormsAuthentication.SignOut();
        }

        #region Web Api

        private const string CurrentUserHeadKey = "ck";
        private const string CurrentUserEncryptKey = "BeiLin$User$key";

        public static string GetHeadKey()
        {
            var key = CurrentUserHeadKey;
            var path = HttpContext.Current.Request.Url.AbsolutePath;
            if (path.StartsWith("/api/Service/", true, null))
            {
                key += "_s";
            }
            else
            {
                key += "_m";
            }
            return key;

        }

        public static string GetWebAppCookieName()
        {
            return CurrentUserHeadKey + "_m";
        }
        public void CreateApiAuthMemberTiket(Users user)
        {
            var userModel = GetModel(user);
            UserRep.AddUserCache(userModel);

            string userData = $"{user.GId}|{user.UserGroup}";

            var ticket = EncryptUtils.Encrypt(userData, CurrentUserEncryptKey);
            var baseTicket = StringUtility.XBase64Encode(ticket);

            HeadUtility.AddHead(GetHeadKey(), baseTicket);

            setPrinciple(userModel, user.UserGroup);
        }

        public void FromApiAuthenticationTicket(string ticket)
        {
            try
            {
                ticket = StringUtility.XBase64Decode(ticket);
                var userData = EncryptUtils.Decrypt(ticket, CurrentUserEncryptKey);
                if (string.IsNullOrEmpty(userData))
                {
                    return;
                }
                var userDataArr = userData.Split('|');
                if (userDataArr.Length != 2)
                {
                    return;
                }
                var key = GetHeadKey();
                if (key.Contains("_m") || key.Contains("_s"))
                {
                    //会员
                    var userGroup = (UserGroup)Enum.Parse(typeof(UserGroup), userDataArr[1]);
                    RestoreUser(userDataArr[0], userGroup);
                }
            }
            catch (Exception ex)
            {
                var log = LogManager.GetLogger(this.GetType().FullName);
                log.Error("恢复用户信息失败！错误原因：" + ex, ex);
            }
        }

        private void RestoreUser(string gid, UserGroup userGroup)
        {
            var userModel = UserRep.GetByCache(gid);
            if (userModel == null)
            {
                var user = UserRep.GetByGid(gid);
                if (user == null)
                {
                    UserRep.RemoveUserCache(gid);
                    FormsAuthentication.SignOut();
                    return;
                }
                userModel = GetModel(user);
                UserRep.AddUserCache(userModel);

            }
            setPrinciple(userModel, userGroup);
        }

        public void ApiLogOut(string gid)
        {
            HeadUtility.AddHead(GetHeadKey(), "");
            UserRep.RemoveUserCache(gid);
        }
        #endregion
    }
}

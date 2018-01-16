using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Steven.Domain.Infrastructure;
using Steven.Domain.Models;
using Steven.Core.Utilities;
using Steven.Domain.Enums;
using Steven.Domain.ViewModels;
using Dapper;
using Steven.Core.Cache;
using Steven.Domain.Infrastructure.SysUser;

namespace Steven.Domain.Repositories
{
    public class UsersRepository : Repository<Users>, IUsersRepository
    {
        public ICacheManager Cache { get; set; }
        public Users GetByGid(string gid)
        {
            if (string.IsNullOrEmpty(gid))
            {
                return null;
            }
            var arr = gid.Split('-');
            if (arr.Length < 2)
            {
                return null;
            }
            var id = StringUtility.ConvertToBigInt(arr[1]);
            if (id == 0)
            {
                return null;
            }
            return Get(id);
        }

        public void  InitialAdmin()
        {
            var user = GetByLoginName("admin");
            if (user!= null) return;

            user = new Users
            {
                CreateUserName = "admin",
                RealName = "管理员",
                HeadImageId = 0,
                CommonStatus = CommonStatus.Enabled,
                Gender = Gender.Male,
                LoginCount = 0,
                Remark = "系统管理员",
                PasswordSalt = HashUtils.GenerateSalt(),
                LoginName = "admin"
            };
            user.Password = HashUtils.HashPasswordWithSalt("123123", user.PasswordSalt);
             Insert(user);
        }

        public Users GetByLoginName(string loginName)
        {
            return DbConn.QueryFirstOrDefault<Users>(Query() + " and LoginName=@loginName", new { loginName });
        }
        public Users GetByEmail(string email)
        {
            return DbConn.QueryFirstOrDefault<Users>(Query() + " and Email=@email", new { email });
        }

        public  LoginResult AdminLogin(string loginName, string pwd)
        {
            var group = UserGroup.Admin;
            return Login(loginName, pwd, group);
        }

        private LoginResult Login(string loginName, string pwd, UserGroup group)
        {
            var model = new LoginResult();
            if (string.IsNullOrEmpty(loginName) || string.IsNullOrEmpty(pwd))
            {
                model.Status = SigninStatus.Failed;
                return model;
            }
            var user = GetByLoginName(loginName);
            if (user == null)
            {
                model.Status = SigninStatus.UserNotFound;
                return model;
            }
            if (user.CommonStatus == CommonStatus.Disabled)
            {
                model.Status = SigninStatus.Disabled;
                return model;
            }
            if (user.UserGroup != group || !user.Password.Equals(HashUtils.HashPasswordWithSalt(pwd, user.PasswordSalt)))
            {
                model.Status = SigninStatus.PasswordIncorrent;
                return model;
            }
            user.LoginCount++;
            Update(user);

            model.UserInfo = user;
            model.Status = SigninStatus.Succ;
            return model;
        }

        public LoginResult ShopLogin(string loginName, string pwd)
        {
            return Login(loginName, pwd, UserGroup.Shop);
        }

        /// <summary>
        /// 返回所有列表，可以查询
        /// </summary>
        /// <param name="group"></param>
        /// <param name="keyWord">搜索关键词</param>
        /// <param name="gender">性别</param>
        /// <param name="orderField">排序的字段</param>
        /// <param name="orderDirection">排序的顺序</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示的数量</param>
        /// <returns></returns>
        public Pager<Users> GetPager(UserGroup group, string keyWord,PageSearchModel search)
        {
            var sql = new StringBuilder(AdminQuery());
            var param = new DynamicParameters();
            sql.Append(" and UserGroup=@group");
            param.Add("group", group);
            
            if (!string.IsNullOrEmpty(keyWord))
            {
                sql.Append($@" and (LoginName like @keyword 
                                or Email like @keyword
                                or RealName like @keyword)");
                param.Add("keyword", keyWord);
            }
            return Pager(sql.ToString(), param, search);
        }



        /// <summary>
        /// 是否存在用户名
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="loginName">用户名</param>
        /// <returns></returns>
        public  bool ExistLoginName(long id, string loginName)
        {
            var count = DbConn.QueryFirst<int>(Count() + " and Id != @id and LoginName = @loginName", new { id, loginName });
            return count > 0;
        }

        /// <summary>
        /// 是否存在邮箱
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="email">邮箱</param>
        /// <returns></returns>
        public  bool ExistEmail(long id, string email)
        {
            var count =  DbConn.QueryFirst<int>(Count() + " and Id != @id and Email = @email", new { id, email });
            return count > 0;
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="ids">Id集合</param>
        /// <returns></returns>
        public  IEnumerable<Users> BatchPasswordReset(string ids)
        {
            if (string.IsNullOrEmpty(ids))
            {
                return null;
            }
            var idList = StringUtility.ConvertToBigIntArray(ids, ',');
            if (idList == null || !idList.Any())
            {
                return null;
            }
            var list =  DbConn.Query<Users>(Query() + " and Id in @ids", new { ids = idList });
            foreach (var item in list)
            {
                item.PasswordSalt = HashUtils.GenerateSalt();
                item.Password = HashUtils.HashPasswordWithSalt("123123", item.PasswordSalt);
                 Update(item);
            }

            return list;
        }
        public int GetCount(bool isMonth=false)
        {
            var param = new DynamicParameters();
            var sql = Count() + " and UserGroup=@UserGroup";
            param.Add("UserGroup", UserGroup.Member);
            if (isMonth)
            {
                var now = DateTime.Now;
                var startTime = now.AddMonths(-1);
                sql=sql+ " and UpdateTime between CONVERT(varchar(100), @startTime, 111) and CONVERT(varchar(100),@endTime, 111) ";
                param.Add("startTime", startTime);
                param.Add("endTime", now);
            }
            return DbConn.QueryFirst<int>(sql,param);
        }


        #region 缓存
        public const int CacheTime = 10800;

        public override void RemoveCache(Users entity)
        {
            base.RemoveCache(entity);
            RemoveUserCache(entity.GId);
        }
        private string GetUserCacheKey(string gid)
        {
            return CacheKey + gid;
        }
        public void AddUserCache(ISysUserModel model)
        {
            Cache.Add(GetUserCacheKey(model.GId), model, CacheTime);
        }

        public ISysUserModel GetByCache(string gid)
        {
            return Cache.Get<ISysUserModel>(GetUserCacheKey(gid));
        }

        public void RemoveUserCache(string gid)
        {
            Cache.Remove(GetUserCacheKey(gid));
        }
        #endregion

    }
}

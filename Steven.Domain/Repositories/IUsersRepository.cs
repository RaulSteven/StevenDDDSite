using System.Collections.Generic;
using System.Threading.Tasks;
using Steven.Domain.Infrastructure;
using Steven.Domain.Models;
using Steven.Domain.Enums;
using Steven.Domain.ViewModels;
using Steven.Domain.Infrastructure.SysUser;

namespace Steven.Domain.Repositories
{
    public interface IUsersRepository:IRepository<Users>
    {
        Users GetByGid(string gid);

        void InitialAdmin();

        Users GetByLoginName(string loginName);

        Users GetByEmail(string email);

        LoginResult AdminLogin(string loginName, string pwd);

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
        Pager<Users> GetPager(UserGroup group, string keyWord, PageSearchModel search);

        /// <summary>
        /// 是否存在用户名
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="loginName">用户名</param>
        /// <returns></returns>
        bool ExistLoginName(long id, string loginName);

        /// <summary>
        /// 是否存在邮箱
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="email">邮箱</param>
        /// <returns></returns>
        bool ExistEmail(long id, string email);

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="ids">Id集合</param>
        /// <returns></returns>
        IEnumerable<Users> BatchPasswordReset(string ids);

        int GetCount(bool isMonth=false);

        void AddUserCache(ISysUserModel model);

        ISysUserModel GetByCache(string gid);

        void RemoveUserCache(string gid);
    }
}

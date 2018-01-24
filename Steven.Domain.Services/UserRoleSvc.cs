using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.Repositories;
using Steven.Domain.Models;
using Steven.Core.Cache;
using Steven.Core.Utilities;
using Steven.Domain.ViewModels;
using System.Reflection;
using Newtonsoft.Json;
using System.ComponentModel;
using Steven.Domain.Infrastructure;
using Steven.Domain.Enums;
using Steven.Core.Extensions;
using Dapper;

namespace Steven.Domain.Services
{
    public class UserRoleSvc : BaseSvc, IUserRoleSvc
    {
        public IUser2RoleRepository User2RoleRepository { get; set; }
        public IUser2ApartmentRepository User2ApartRepository { get; set; }
        public IUserRole2ApartmentRepository Role2ApartRepository { get; set; }
        public IUserRoleRepository UserRoleRepository { get; set; }
        public ICacheManager Cache { get; set; }
        public IUserRole2MenuRepository Role2MenuRepository { get; set; }
        public ISysMenuRepository SysMenuRepository { get; set; }
        public IUsersRepository UserRepository { get; set; }
        public ISysApartmentRepository SysApartRepository { get; set; }
        public IUserRole2FilterRepository Role2FilterRepository { get; set; }

        public List<long> GetRoleIdList(long userId)
        {
            if (userId == 0)
            {
                return new List<long>();
            }
            //用户x角色
            var lstU2RRoleId = User2RoleRepository.GetLstRoleId(userId);

            //用户x部门x角色
            var lstApartId = User2ApartRepository.GetLstApartId(userId);
            var lstR2ARoleId = Role2ApartRepository.GetLstRoleId(lstApartId);

            //RoleIdList
            lstU2RRoleId.Concat(lstR2ARoleId);
            var roleIdList = lstU2RRoleId
                .Distinct()
                .ToList() ;
            return roleIdList;
        }

        public void SaveList(UserRole role, string menuIds)
        {
            UserRoleRepository.Save(role);

            Role2MenuRepository.SaveList(role.Id, menuIds);

            ClearRoleUserCache(role.Id);
        }

        /// <summary>
        /// 清除包含这个角色的用户缓存
        /// </summary>
        /// <param name="roleId"></param>
        public void ClearRoleUserCache(long roleId)
        {
            //user2role 找到userid
            var lstUserId = User2RoleRepository.GetLstUserId(roleId);

            //user2apart  role2apart 找到userid
            var lstApartId = Role2ApartRepository.GetLstApartId(roleId);
            var lstApartUserId = User2ApartRepository.GetLstUserId(lstApartId);
            //合集
            lstUserId.Concat(lstApartUserId);

            //剔除重复
            lstUserId = lstUserId.Distinct().ToList();

            //清除缓存
            foreach (var userId in lstUserId)
            {
                UserRepository.RemoveUserCache(Users.GIdPrefix + userId);
            }
        }

        public IEnumerable<DropdownItemModel> GetResList(FilterCurrent curr)
        {
            var sql = "";
            switch (curr)
            {
                case FilterCurrent.CurrentUserId:
                    sql = "select Id as Value,LoginName as Text from Users order by Id desc";
                    break;
                case FilterCurrent.CurrentRoleId:
                    sql = "select Id as Value,Name as Text from UserRole order by Sort,Id desc";
                    break;
                case FilterCurrent.CurrentDeptId:
                    sql = "select Id as Value,Name as Text from SysApartment order by Sort,Id desc";
                    break;
                default:
                    break;
            }
            return DbConn.Query<DropdownItemModel>(sql);
        }
        public IEnumerable<FilterGroupModel> GetFilterList(long roleId)
        {
            var list = Role2FilterRepository.GetLstFilterGroup(roleId);
            foreach (var item in list)
            {
                #region 加载属性
                var type = Assembly.Load("Steven.Domain").GetType("Steven.Domain.Models." + item.Source);
                var propModelList = new List<PropertyModel>();
                var propertyInfoList = type.GetProperties();
                foreach (var property in propertyInfoList)
                {
                    var proModel = new PropertyModel()
                    {
                        Name = property.Name,
                        TypeName = property.PropertyType.IsEnum ? "Enum" : property.PropertyType.Name,
                    };
                    var nameAttr = property.GetCustomAttribute(typeof(DisplayNameAttribute));
                    if (nameAttr == null)
                    {
                        proModel.DisplayName = property.Name;
                    }
                    else
                    {
                        proModel.DisplayName = ((DisplayNameAttribute)nameAttr).DisplayName;
                    }
                    proModel.DisplayName = $"{proModel.DisplayName}({proModel.TypeName})";
                    propModelList.Add(proModel);
                }
                foreach (var curr in FilterCurrent.CurrentDeptId.GetSList())
                {
                    propModelList.Add(new PropertyModel()
                    {
                        Name = curr.Value,
                        DisplayName = curr.Text,
                        TypeName = "Enum"
                    });
                }
                item.SourceProperties = JsonConvert.SerializeObject(propModelList);
                #endregion

            }
            return list;
        }

        public IEnumerable<Users> GetLstUsers(string roleIds)
        {
            if (string.IsNullOrEmpty(roleIds))
            {
                return null;
            }
            var roleIdArray = StringUtility.ConvertToBigIntArray(roleIds, ',');

            var sql = @"select u.* from Users u,User2Role u2r
                        where u.Id = u2r.UserId
                        and u2r.RoleId in @roleIds";
            var list = DbConn.Query<Users>(sql, new { roleIds = roleIdArray });
            return list;
        }
    }
}

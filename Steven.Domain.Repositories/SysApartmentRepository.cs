using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Steven.Domain.Infrastructure;
using Steven.Domain.Models;
using Steven.Domain.ViewModels;
using Dapper;
using System.Data;

namespace Steven.Domain.Repositories
{
    public class SysApartmentRepository : Repository<SysApartment>, ISysApartmentRepository
    {
        /// <summary>
        /// 树显示方式
        /// </summary>
        public string TreePathFormat = "00000000";
        public List<JsTreeJsonModel> GetJsonList()
        {
            var list =  DbConn.Query<JsTreeJsonModel>(Query("Id as id,Name as text,Pid as pid,Sort as sort") + " order by sort,id");

            var result = list.Where(m => m.pid == "0").ToList();
            foreach (var apart in result)
            {
                SetChildren(apart, list);
            }
            return result;
        }

        private void SetChildren(JsTreeJsonModel target, IEnumerable<JsTreeJsonModel> all)
        {
            if (all.Any(m => m.pid == target.id))
            {
                target.children = all.Where(m => m.pid == target.id)
                    .OrderBy(m => m.sort)
                    .ThenBy(m => m.id)
                    .ToList();
                foreach (var child in target.children)
                {
                    SetChildren(child, all);
                }
                target.icon = "fa fa-folder-open-o";
            }else
            {
                target.icon = "fa fa-file";
            }
        }

        public override long  Save(SysApartment entity, IDbTransaction trans = null)
        {
             base.Save(entity,trans);

            entity.TreePath = "";
            if (entity.Pid != 0)
            {
                var parent =  Get(entity.Pid);
                entity.TreePath = parent.TreePath + ".";
            }
            entity.TreePath += entity.Id.ToString(TreePathFormat);
             Update(entity);
            return entity.Id;
        }

        public string Delete(long id)
        {
            var treePath = id.ToString(TreePathFormat);
            var list =  GetListByTreePath(treePath);
            var ids = "";
            if (list.Any())
            {
                ids = string.Join(",", list.Select(m => m.Id).ToArray());
                 BatchDele(Enums.TableSource.SysApartment, ids);
            }
            return ids;
        }

        private IEnumerable<SysApartment> GetListByTreePath(string treePath)
        {
            var sql = Query() + " and TreePath like @treePath";
            return DbConn.Query<SysApartment>(sql, new { treePath=$"%{treePath}%" });
        }

        ///// <summary>
        ///// 返回部门所有的记录及部门下所有人员，用于树形结构
        ///// </summary>
        ///// <param name="userApartmentQuery">部门用户信息</param>
        ///// <param name="userQuery">用户信息</param>
        ///// <returns></returns>
        //public async <List<SysApartZTreeModel>> GetListByZTree(IQueryable<User2Apartment> userApartmentQuery,
        //    IQueryable<Users> userQuery)
        //{
        //    var list =  QueryUnDelete().Select(d => new SysApartZTreeModel()
        //    {
        //        id = d.Id,
        //        pid = d.Pid,
        //        name = d.Name,
        //        treePath = d.TreePath,
        //        userTip = "",
        //        open = d.Pid == 0,
        //        sort = d.Sort
        //    })
        //        .OrderByDescending(d => d.treePath)
        //        .ToList();

        //    var query = QueryUnDelete();
        //    var user =
        //        userApartmentQuery.Select(
        //            d => new { User2Apartment = d, User = userQuery.FirstOrDefault(m => m.Id == d.UserId) })
        //            .Where(d => d.User != null)
        //            .Select(
        //                d =>
        //                    new
        //                    {
        //                        User = d.User,
        //                        Apartment = query.FirstOrDefault(m => m.Id == d.User2Apartment.ApartmentId)
        //                    })
        //            .Where(d => d.Apartment != null)
        //            .Select(d => new SysApartZTreeModel
        //            {
        //                id = d.User.Id,
        //                pid = d.Apartment.Id,
        //                name = d.User.RealName,
        //                treePath = d.Apartment.TreePath,
        //                userTip = "u_",
        //                open = false,
        //                sort = d.Apartment.Sort
        //            })
        //            .OrderByDescending(d => d.treePath)
        //            .ToList();
        //    var result = new List<SysApartZTreeModel>();
        //    foreach (var item in list)
        //    {
        //        item.children = result.Where(d => d.pid == item.id)
        //            .OrderBy(d => d.treePath)
        //            .ToList();

        //        if (item.children.Any())
        //        {
        //            result.RemoveAll(d => item.children.Any(m => m.id == d.id));
        //        }
        //        var userList = user.Where(d => d.pid == item.id)
        //            .OrderBy(d => d.treePath)
        //            .ToList();
        //        if (userList.Any())
        //        {
        //            item.children.AddRange(userList);
        //        }
        //        result.Add(item);
        //    }
        //    return result.OrderBy(d => d.treePath).ToList();
        //}

        public int GetIndexOfParent(SysApartment apart)
        {
            var sql = Query("Id") + " and Pid=@pid order by Sort,Id";
            var list =  DbConn.Query<long>(sql, new { pid = apart.Pid });

            var index = list.ToList().IndexOf(apart.Id);
            return index;
        }
    }
}

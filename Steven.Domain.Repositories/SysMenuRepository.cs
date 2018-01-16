using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Steven.Domain.Infrastructure;
using Steven.Domain.Models;
using Steven.Domain.ViewModels;
using Dapper;
using Steven.Domain.Enums;
using Steven.Core.Extensions;
using System.Data;

namespace Steven.Domain.Repositories
{
    public class SysMenuRepository : Repository<SysMenu>, ISysMenuRepository
    {
        public string TreePathFormat = "00000000";
        public string Delete(long id)
        {
            var treePath = id.ToString(TreePathFormat);
            var list = GetListByTreePath(treePath);
            var ids = "";
            if (list.Any())
            {
                BatchDele(TableSource.SysMenu, list.Select(m => m.Id));
            }
            return ids;
        }

        private IEnumerable<SysMenu> GetListByTreePath(string treePath)
        {
            return DbConn.Query<SysMenu>(Query() + " and TreePath like @treePath", new { treePath = $"%{treePath}%" });
        }

        public List<JsTreeJsonModel> GetJsonList()
        {
            var sql = new StringBuilder(Query(" Id as id,Name as text,Sort as sort,Pid as pid, Icon as icon"));
            sql.Append(" order by sort,id");
            var list = DbConn.Query<JsTreeJsonModel>(sql.ToString());
            var result = list.Where(m => m.pid == "0").ToList();
            foreach (var item in result)
            {
                SetChildren(item, list);
            }
            return result;
        }

        private void SetChildren(JsTreeJsonModel target, IEnumerable<JsTreeJsonModel> all)
        {
            if (all.Any(m => m.pid == target.id))
            {
                target.children = all.Where(m => m.pid == target.id).ToList();
                foreach (var child in target.children)
                {
                    SetChildren(child, all);
                }
            }
        }

        private List<SysButton> GetLstBtn(SysButton buttons)
        {
            var lstBtn = new List<SysButton>();
            foreach (var btn in buttons.GetDescriptDict())
            {
                var button = (SysButton)btn.Key;
                if (buttons.HasFlag(button))
                {
                    lstBtn.Add(button);
                }
            }
            return lstBtn;
        }

        public override long Save(SysMenu entity, IDbTransaction trans = null)
        {
            base.Save(entity,trans);

            entity.TreePath = "";
            if (entity.Pid != 0)
            {
                var parent = Get(entity.Pid);
                entity.TreePath = parent.TreePath + ".";
            }
            entity.TreePath += entity.Id.ToString(TreePathFormat);
            Update(entity);
            return entity.Id;
        }
        public int GetIndexOfParent(SysMenu menu)
        {
            var list = DbConn.Query<SysMenu>(Query() + " and Pid=@pid order by Sort,Id", new { pid = menu.Pid });

            var index = list.ToList().IndexOf(menu);
            return index;
        }
    }
}

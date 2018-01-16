using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Steven.Core.Utilities;
using Steven.Domain.Infrastructure;
using Steven.Domain.Models;
using Steven.Domain.ViewModels;
using Dapper;

namespace Steven.Domain.Repositories
{
    public class ArticleClassifyRepository : Repository<ArticleClassify>, IArticleClassifyRepository
    {
        /// <summary>
        /// 树显示方式
        /// </summary>
        public string TreePathFormat = "00000000";
        /// <summary>
        /// 返回下拉列表
        /// </summary>
        /// <returns></returns>
        public List<JsTreeJsonModel> GetListByZTree()
        {
            var sql = AdminQuery("Id as id,PId as pid,Name as text,TreePath as treePath,PartialViewCode as partialViewCode,Remark as remark");
            sql += " order by treePath desc";
            var list = DbConn.Query<JsTreeJsonModel>(sql);

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
            }
            else
            {
                target.icon = "fa fa-file";
            }
        }

        /// <summary>
        /// 添加或修改数据
        /// </summary>  
        /// <param name="model">当前分类</param>
        /// <param name="parent">当前主分类</param> 
        public void Save(ArticleClassify model, ArticleClassify parent)
        {
            if (string.IsNullOrEmpty(model.TreePath))
            {
                model.TreePath = TreePathFormat;
            }
            Save(model);

            var parentTreePath = "";
            if (parent != null)
            {
                parentTreePath = parent.TreePath + ".";
                int count = GetChildrenCount(parent.Id);
                parent.ChildrenCount = count;
                Save(parent);
            }
            model.TreePath =parentTreePath+ model.Id.ToString(TreePathFormat);
            model.Depth = model.TreePath.Split('.').Count(d => !string.IsNullOrEmpty(d)) - 1;

            Save(model);
        }

        private int GetChildrenCount(long pid)
        {
            var sql = Count() + $" and Pid = {pid}";
            return DbConn.QueryFirst<int>(sql);
        }

        /// <summary>
        /// 查看是否有子分类
        /// </summary>
        /// <param name="id">当前记录Id</param>
        /// <returns></returns>
        public bool IsLastChild(long id)
        {
            var sql = Count() + $" and Pid = {id} and {id} > 0";
            var count = DbConn.QueryFirst<int>(sql);
            return count > 0;
        }

        /// <summary>
        /// 返回当前分类及其所有的主分类
        /// </summary>
        /// <param name="id">当前记录Id</param>
        /// <returns></returns>
        public List<ArticleClassify> GetListChildById(long id)
        {
            var classify = Get(id);
            if (classify == null)
            {
                return null;
            }
            var sql = Query() + $" and {classify.TreePath} like %TreePath% order by TreePath";
            var list = DbConn.Query<ArticleClassify>(sql);
            return list.ToList();
        }

        public IEnumerable<long> GetLstChildId(long id)
        {
            var treePath = id.ToString(TreePathFormat);
            var sql = Query("Id") + $" and TreePath like @treePath";
            var list = DbConn.Query<long>(sql,new { treePath=$"%{treePath}%" });
            return list;
        }

        /// <summary>
        /// 通过Id集合返回其所包含的记录列表
        /// </summary>
        /// <param name="ids">Id集合</param>
        /// <returns></returns>
        public IEnumerable<ArticleClassify> GetListByIds(string ids)
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
            var list = DbConn.Query<ArticleClassify>(Query() + " and id in @ids", new { ids = idList });
            return list;
        }

        public JsonModel Delete(long id)
        {
            var result = new JsonModel();
            var treePath = id.ToString(TreePathFormat);
            var list = GetListByTreePath(treePath);
            if (!list.Any())
            {
                result.msg = $"找不到Id为{id}的记录";
                return result;
            }
            var idArr = list.Select(m => m.Id);
            var articleCount = DbConn.QueryFirst<int>("select Count(Id) from Article where ClassifyId in @idArr",new { idArr });
            if (articleCount > 0)
            {
                result.msg = "请先移除分类下的文章！";
                return result;
            }
            
            BatchDele(Enums.TableSource.ArticleClassify, idArr);
            result.code = JsonModelCode.Succ;
            result.data = string.Join(",", idArr);
            result.msg = "删除成功！";
            return result;
        }

        private IEnumerable<ArticleClassify> GetListByTreePath(string treePath)
        {
            return DbConn.Query<ArticleClassify>(Query() + " and TreePath like @treePath", new { treePath = $"%{treePath}%" });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Steven.Core.Utilities;
using Steven.Domain.Enums;
using Steven.Domain.Infrastructure;
using Steven.Domain.Models;
using Steven.Domain.ViewModels;
using Dapper;


namespace Steven.Domain.Repositories
{
    public class ArticleRepository : Repository<Article>, IArticleRepository
    {
        

        /// <summary>
        /// 返回当前分类下所有的文章
        /// </summary>
        /// <param name="articleClassifyQuery">分类信息</param>
        /// <param name="keyWord">搜索关键词</param>
        /// <param name="classifyId">当前分类的Id</param>
        /// <param name="orderField">排序的字段</param>
        /// <param name="orderDirection">排序的顺序</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示的数量</param>
        /// <returns></returns>
        public Pager<Article> GetPager(string keyWord, long? classifyId, PageSearchModel search)
        {
            var sql = new StringBuilder(AdminQuery());
            var param = new DynamicParameters();
            
            if (!string.IsNullOrEmpty(keyWord))
            {
                sql.Append(" and (Title like @keyword or Author like @keyword)");
                param.Add("keyword", $"%{keyWord}%");
            }
            if (classifyId.HasValue)
            {
                sql.Append(" and ClassifyId = @classifyId");
                param.Add("classifyId", classifyId.Value);
            }
            return Pager(sql.ToString(), param, search);
        }


        /// <summary>
        /// 判断该文章分类是否有文章记录
        /// </summary>
        /// <param name="classifyId">当前文章分类的Id</param>
        /// <returns></returns>
        public  bool IsAnyRecord(long classifyId)
        {
            var count = DbConn.QueryFirst<int>(Count() + " and ClassifyId = @classifyId",new { classifyId= classifyId});
            return count > 0;
        }


        public Article GetByIndex(string index)
        {
            if (string.IsNullOrEmpty(index))
            {
                return null;
            }
            var obj = DbConn.QuerySingleOrDefault<Article>(Query() + " and ArticleIndex = @index", new { index = index });
            return obj;
        }

        public IEnumerable<ArticleSimpleModel> GetList(IEnumerable<long> lstClz, int count)
        {
            var sql = Query($"top {count} Id,Title,ArticleDateTime") + " and ClassifyId in @clzIds and CommonStatus=@status order by ArticleDateTime desc";
            var lst = DbConn.Query<ArticleSimpleModel>(sql, new { clzIds = lstClz,status=CommonStatus.Enabled });
            return lst;
        }

        public Pager<ArticleListItemModel> GetPager(IEnumerable<long> lstClz, PageSearchModel search)
        {
            var sql = @"select a.Id,a.Title,a.ArticleDateTime,a.Author,a.ClassifyId,ac.Name as ClassifyName,ac.PicAttaId as ClassifyPicAttaId from Article a 
                left join ArticleClassify ac on a.ClassifyId = ac.Id
                where ClassifyId in @clzIds and CommonStatus=@status";
            var param = new DynamicParameters();
            param.Add("clzIds", lstClz);
            param.Add("status", CommonStatus.Enabled);
            search.Order = "desc";
            search.Sort = "ArticleDateTime";
            return Pager<ArticleListItemModel>(sql, param, search);
        }

        public Article GetEnable(long id)
        {
            var obj = DbConn.QuerySingleOrDefault<Article>(Query() + " and Id = @id and CommonStatus=@status", new { id,status=CommonStatus.Enabled });
            return obj;
        }

    }
}

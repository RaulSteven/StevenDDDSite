using System.Threading.Tasks;
using Steven.Domain.Infrastructure;
using Steven.Domain.Models;
using Steven.Domain.ViewModels;
using System.Collections;
using System.Collections.Generic;

namespace Steven.Domain.Repositories
{
    public interface IArticleRepository : IRepository<Article>
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
        Pager<Article> GetPager(string keyWord, long? classifyId, PageSearchModel search);

        /// <summary>
        /// 判断该文章分类是否有文章记录
        /// </summary>
        /// <param name="classifyId">当前文章分类的Id</param>
        /// <returns></returns>
        bool IsAnyRecord(long classifyId);

        Article GetByIndex(string index);

        IEnumerable<ArticleSimpleModel> GetList(IEnumerable<long> lstClz, int count);
        Pager<ArticleListItemModel> GetPager(IEnumerable<long> lstClz, PageSearchModel search);

        Article GetEnable(long id);

    }
}

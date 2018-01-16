using System.Collections.Generic;
using System.Threading.Tasks;
using Steven.Domain.Models;
using Steven.Domain.ViewModels;

namespace Steven.Domain.Repositories
{
    public interface IArticleClassifyRepository : IRepository<ArticleClassify>
    {
        /// <summary>
        /// 返回下拉列表
        /// </summary>
        /// <returns></returns>
        List<JsTreeJsonModel> GetListByZTree();

        /// <summary>
        /// 添加或修改数据
        /// </summary>  
        /// <param name="model">当前分类</param>
        /// <param name="p">当前主分类</param> 
        void Save(ArticleClassify model, ArticleClassify p); 

        /// <summary>
        /// 查看是否有子分类
        /// </summary>
        /// <param name="id">当前记录Id</param>
        /// <returns></returns>
        bool IsLastChild(long id);

        /// <summary>
        /// 返回当前分类及其所有的主分类
        /// </summary>
        /// <param name="id">当前记录Id</param>
        /// <returns></returns>
        List<ArticleClassify> GetListChildById(long id);

        IEnumerable<long> GetLstChildId(long id);

        /// <summary>
        /// 通过Id集合返回其所包含的记录列表
        /// </summary>
        /// <param name="ids">Id集合</param>
        /// <returns></returns>
        IEnumerable<ArticleClassify> GetListByIds(string ids);

        JsonModel Delete(long id);

    }
}

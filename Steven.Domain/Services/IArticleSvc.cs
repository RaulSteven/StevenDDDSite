using Steven.Domain.Infrastructure;
using Steven.Domain.ViewModels;
using System.Collections.Generic;

namespace Steven.Domain.Services
{
    public interface IArticleSvc
    {
        IEnumerable<ArticleSimpleModel> GetList(long clzId, int count);

        Pager<ArticleListItemModel> GetPager(long clzId, PageSearchModel search);
    }
}

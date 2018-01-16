using Steven.Domain.Infrastructure;
using Steven.Domain.Models;
using Steven.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steven.Domain.Services
{
    public interface IArticleSvc
    {
        IEnumerable<ArticleSimpleModel> GetList(long clzId, int count);

        Pager<ArticleListItemModel> GetPager(long clzId, PageSearchModel search);
    }
}

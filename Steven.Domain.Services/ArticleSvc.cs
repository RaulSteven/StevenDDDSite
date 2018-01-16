using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.ViewModels;
using Steven.Domain.Repositories;
using Steven.Domain.Infrastructure;

namespace Steven.Domain.Services
{
    public class ArticleSvc : BaseSvc, IArticleSvc
    {
        public IArticleClassifyRepository ArticleClassifyRepository { get; set; }
        public IArticleRepository ArticleRepository { get; set; }

        public IEnumerable<ArticleSimpleModel> GetList(long clzId, int count)
        {
            var lstClzId = ArticleClassifyRepository.GetLstChildId(clzId);
            return ArticleRepository.GetList(lstClzId, count);
        }

        public Pager<ArticleListItemModel> GetPager(long clzId, PageSearchModel search)
        {
            var lstClzId = ArticleClassifyRepository.GetLstChildId(clzId);
            return ArticleRepository.GetPager(lstClzId, search);
        }
    }
}

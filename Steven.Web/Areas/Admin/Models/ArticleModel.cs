using Steven.Domain.Enums;
using Steven.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Steven.Web.Areas.Admin.Models
{
    public class ArticleModel
    {
        public long Id { get; set; }

        public long ClassifyId { get; set; }
        public string ClassifyName { get; set; }

        public string ShoulderTitle { get; set; }

        public string ShortTitle { get; set; }

        public string OriginalTitle { get; set; }

        public string Title { get; set; }

        public string TitleSub { get; set; }

        public string Author { get; set; }

        public string Brief { get; set; }

        public string ArticleContent { get; set; }

        public DateTime ArticleDateTime { get; set; }

        public string Source { get; set; }

        public string SourceLink { get; set; }

        public int Sort { get; set; }

        public long PicAttaId { get; set; }

        public int ViewCount { get; set; }

        public string ExecutiveEditor { get; set; }

        public string Keyword { get; set; }


        public CommonStatus CommonStatus { get; set; }

        public ArticleDetailType PartialViewCode { get; set; }

        public ArticleFlags Flags { get; set; }

        public string VideoUrl { get; set; }

        public string ArticleIndex { get; set; }

        public string FocusMap { get; set; }

        public List<ArticleFocusMapModel> FocusMapList { get; set; }
        public ArticleTarget ArticleTarget { get; set; }
    }
}
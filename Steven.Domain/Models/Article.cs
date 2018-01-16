using System;
using System.ComponentModel.DataAnnotations;
using Steven.Domain.Enums;
using Steven.Domain.Infrastructure;
using Dapper.Contrib.Extensions;

namespace Steven.Domain.Models
{
    [Table("Article")]
    public partial class Article : AggregateRoot
    {

        [Display(Name = "分类")] 
        public long ClassifyId { get; set; }

        [Display(Name = "肩标题")]
        [StringLength(255)]
        public string ShoulderTitle { get; set; }

        [Display(Name = "短标题")]
        [StringLength(255)]
        public string ShortTitle { get; set; }

        [Display(Name = "原标题")]
        [StringLength(255)]
        public string OriginalTitle { get; set; }

        [Display(Name = "标题")]
        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [Display(Name = "副标题")]
        [StringLength(255)]
        public string TitleSub { get; set; }

        [Display(Name = "作者")]
        [StringLength(50)]
        public string Author { get; set; }

        [Display(Name = "自动摘要")]
        [StringLength(2000)]
        public string Brief { get; set; }

        [Display(Name = "文章内容")]
        [Required]
        public string ArticleContent { get; set; }

        [Display(Name = "显示时间")]
        [Required]
        public DateTime ArticleDateTime { get; set; } 

        [Display(Name = "文章来源")]
        [StringLength(200)]
        public string Source { get; set; }

        [Display(Name = "来源网址")]
        [StringLength(255)] 
        public string SourceLink { get; set; }
         
        public int Sort { get; set; }  

        [Display(Name = "封面图")]
        public long PicAttaId { get; set; } 

        [Display(Name = "浏览次数")] 
        public int ViewCount { get; set; }

        [Display(Name = "责任编辑")]
        [StringLength(50)]
        public string ExecutiveEditor { get; set; }

        [Display(Name = "关键字")]
        [StringLength(250)]
        public string Keyword { get; set; }

        public ArticleDetailType PartialViewCode { get; set; }

        [Display(Name = "标记")]
        public ArticleFlags Flags { get; set; }

        [Display(Name = "视频")]
        [StringLength(500)]
        public string VideoUrl { get; set; }

        [Display(Name = "文章索引")]
        [StringLength(100)]
        public string ArticleIndex { get; set; }

        [Display(Name = "高清轮播图")]
        [StringLength(2000)]
        public string FocusMap { get; set; }
        public int ImageCount { get; set; }
        public ArticleTarget ArticleTarget { get; set; }
        public CommonStatus CommonStatus { get; set; }

    }
}

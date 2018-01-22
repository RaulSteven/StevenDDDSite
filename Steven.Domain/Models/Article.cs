using System;
using Steven.Domain.Enums;
using Steven.Domain.Infrastructure;
using Dapper.Contrib.Extensions;

namespace Steven.Domain.Models
{
    /// <summary>
    /// 文章类
    /// </summary>
    [Table("Article")]
    public partial class Article : AggregateRoot
    {
        /// <summary>
        /// 文章分类主键
        /// </summary>
        public long ClassifyId { get; set; }

        /// <summary>
        /// 肩标题
        /// </summary>
        public string ShoulderTitle { get; set; }

        /// <summary>
        /// 短标题
        /// </summary>
        public string ShortTitle { get; set; }

        /// <summary>
        /// 原标题
        /// </summary>
        public string OriginalTitle { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 副标题
        /// </summary>
        public string TitleSub { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 自动摘要
        /// </summary>
        public string Brief { get; set; }

        /// <summary>
        /// 文章内容
        /// </summary>
        public string ArticleContent { get; set; }

        /// <summary>
        /// 显示时间
        /// </summary>
        public DateTime ArticleDateTime { get; set; }

        /// <summary>
        /// 文章来源
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 来源网址
        /// </summary>
        public string SourceLink { get; set; }
         
        /// <summary>
        /// 排序值，倒序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 封面图
        /// </summary>
        public long PicAttaId { get; set; }

        /// <summary>
        /// 浏览次数
        /// </summary>
        public int ViewCount { get; set; }

        /// <summary>
        /// 责任编辑
        /// </summary>
        public string ExecutiveEditor { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 展示模板：图片、视频、高清
        /// </summary>
        public ArticleDetailType PartialViewCode { get; set; }

        /// <summary>
        /// 标记：无、推荐、置顶
        /// </summary>
        public ArticleFlags Flags { get; set; }

        /// <summary>
        /// 视频地址
        /// </summary>
        public string VideoUrl { get; set; }

        /// <summary>
        /// 文章索引，用于前台链接
        /// </summary>
        public string ArticleIndex { get; set; }

        /// <summary>
        /// 高清轮播图，Json格式，包含图片id、图片说明、图片排序
        /// </summary>
        public string FocusMap { get; set; }
        
        /// <summary>
        /// 图片数量
        /// </summary>
        public int ImageCount { get; set; }

        /// <summary>
        /// 文章目标：原文、本页。
        /// </summary>
        public ArticleTarget ArticleTarget { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public CommonStatus CommonStatus { get; set; }

    }
}

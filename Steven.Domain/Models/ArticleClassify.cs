using Steven.Domain.Enums;
using Steven.Domain.Infrastructure;
using Dapper.Contrib.Extensions;

namespace Steven.Domain.Models
{
    /// <summary>
    /// 文章分类
    /// </summary>
    [Table("ArticleClassify")]
    public partial class ArticleClassify : AggregateRoot
    { 
        /// <summary>
        /// 父分类主键
        /// </summary>
        public long PId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 排序值，倒序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 儿子数量
        /// </summary>
        public int ChildrenCount { get; set; }

        /// <summary>
        /// 二叉树深度
        /// </summary>
        public int Depth { get; set; }

        /// <summary>
        /// 二叉树Path，便于查询。如，主键是1：00000001。主键是2，Pid是1：00000001.00000002
        /// </summary>
        public string TreePath { get; set; }

        /// <summary>
        /// 显示模板：标题、视频、图片
        /// </summary>

        public ArticleListType PartialViewCode { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public long PicAttaId { get; set; }


    }
}

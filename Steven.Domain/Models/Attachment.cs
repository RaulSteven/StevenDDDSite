using System.ComponentModel.DataAnnotations;
using Steven.Domain.Enums;
using Steven.Domain.Infrastructure;
using Dapper.Contrib.Extensions;

namespace Steven.Domain.Models
{
    /// <summary>
    /// 附件表
    /// </summary>
    [Table("Attachment")]
    public partial class Attachment : AggregateRoot
    {
        /// <summary>
        /// 附件名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 附件大小
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// 扩展名
        /// </summary>
        public string FileExt { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        [Required]
        [StringLength(255)]
        public string FilePath { get; set; }

        /// <summary>
        /// 排序值，倒序
        /// </summary>
        public int SortIndex { get; set; }

        /// <summary>
        /// 描述，备注
        /// </summary>
        public string Descript { get; set; }

        /// <summary>
        /// 数据源
        /// </summary>
        public TableSource Source { get; set; }

        /// <summary>
        /// 数据源主键
        /// </summary>
        public long SourceId { get; set; }

        /// <summary>
        /// 浏览次数
        /// </summary>
        public int ViewCount { get; set; }
    }
}

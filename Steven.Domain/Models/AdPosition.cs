using Steven.Domain.Enums;
using Steven.Domain.Infrastructure;
using Dapper.Contrib.Extensions;

namespace Steven.Domain.Models
{
    /// <summary>
    /// 广告位
    /// </summary>
    [Table("AdPosition")]
    public partial class AdPosition : AggregateRoot
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        public AdPosKey Code { get; set; }

        /// <summary>
        /// 显示模版
        /// </summary>
        public string ViewName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Descript { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public long ImageId { get; set; }

        /// <summary>
        /// 宽高，如500x500
        /// </summary>
        public string Size { get; set; }

        /// <summary>
        /// 排序，排序值越大越靠前
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 链接目标
        /// </summary>
        public string LinkUrl { get; set; }

    }
}

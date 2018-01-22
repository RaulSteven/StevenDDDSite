using Steven.Domain.Infrastructure;
using Dapper.Contrib.Extensions;

namespace Steven.Domain.Models
{
    /// <summary>
    /// 合作伙伴
    /// </summary>
    [Table("SysPartner")]
    public class SysPartner : AggregateRoot
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 图片，logo
        /// </summary>
        public long PicAttaId { get; set; }

        /// <summary>
        /// 伙伴地址
        /// </summary>
        public string PartnerUrl { get; set; }

        /// <summary>
        /// 排序值，倒序
        /// </summary>
        public int Sort { get; set; }
    }
}

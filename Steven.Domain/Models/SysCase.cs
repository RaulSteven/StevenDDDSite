using Steven.Domain.Infrastructure;
using Dapper.Contrib.Extensions;
namespace Steven.Domain.Models
{
    /// <summary>
    /// 案例
    /// </summary>
    [Table("SysCase")]
    public class SysCase:AggregateRoot
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 封面图
        /// </summary>
        public long PicAttaId { get; set; }

        /// <summary>
        /// 二维码图片
        /// </summary>
        public long QrAttaId { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public string Brief { get; set; }

        /// <summary>
        /// 案例地址
        /// </summary>
        public string CaseUrl { get; set; }

        /// <summary>
        /// 排序值，倒序
        /// </summary>
        public int Sort { get; set; }
    }
}

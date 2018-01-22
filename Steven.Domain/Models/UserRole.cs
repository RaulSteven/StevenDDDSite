using Steven.Domain.Infrastructure;
using Dapper.Contrib.Extensions;

namespace Steven.Domain.Models
{
    /// <summary>
    /// 角色
    /// </summary>
    [Table("UserRole")]
    public class UserRole : AggregateRoot
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 排序值
        /// </summary>
        public int Sort { get; set; }

    }

}
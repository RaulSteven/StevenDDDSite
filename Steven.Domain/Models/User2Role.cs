using Steven.Domain.Infrastructure;
using Dapper.Contrib.Extensions;

namespace Steven.Domain.Models
{
    /// <summary>
    /// 用户和角色关联表
    /// </summary>
    [Table("User2Role")]
    public class User2Role : AggregateRoot
    {
        /// <summary>
        /// 用户主键
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 角色主键
        /// </summary>
        public long RoleId { get; set; }

    }

}
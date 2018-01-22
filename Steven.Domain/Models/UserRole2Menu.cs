using Steven.Domain.Enums;
using Steven.Domain.Infrastructure;
using Dapper.Contrib.Extensions;

namespace Steven.Domain.Models
{
    /// <summary>
    /// 角色和菜单关联表
    /// </summary>
    [Table("UserRole2Menu")]
    public class UserRole2Menu : AggregateRoot
    {
        /// <summary>
        /// 角色主键
        /// </summary>
        public long RoleId { get; set; }

        /// <summary>
        /// 菜单主键
        /// </summary>
        public long MenuId { get; set; }

        /// <summary>
        /// 不允许查看字段，扩展用
        /// </summary>
        public string DisallowField { get; set; }

        /// <summary>
        /// 筛选规则
        /// </summary>
        public string FilterGroups { get; set; }

        /// <summary>
        /// 按钮
        /// </summary>
        public SysButton Buttons { get; set; }

    }



}
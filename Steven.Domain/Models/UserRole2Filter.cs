using Steven.Domain.Infrastructure;
using Dapper.Contrib.Extensions;
using System;

namespace Steven.Domain.Models
{
    /// <summary>
    /// 角色数据权限筛选
    /// </summary>
    [Table("UserRole2Filter")]
    [Serializable]
    public partial class UserRole2Filter : AggregateRoot
    {
        /// <summary>
        /// 角色主键
        /// </summary>
        public long RoleId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 数据源，表名
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 筛选规则，Json格式
        /// </summary>
        public string FilterGroups { get; set; }
    }
}

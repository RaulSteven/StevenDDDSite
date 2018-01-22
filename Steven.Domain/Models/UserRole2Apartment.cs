using Steven.Domain.Infrastructure;
using Dapper.Contrib.Extensions;

namespace Steven.Domain.Models
{
    /// <summary>
    /// 角色和部门关联表
    /// </summary>
    [Table("UserRole2Apartment")]
    public class UserRole2Apartment : AggregateRoot
    {
        /// <summary>
        /// 角色主键
        /// </summary>
        public long RoleId { get; set; }

        /// <summary>
        /// 部门主键
        /// </summary>
        public long ApartmentId { get; set; }

    }

}
using Steven.Domain.Infrastructure;
using Dapper.Contrib.Extensions;

namespace Steven.Domain.Models
{
    /// <summary>
    /// 用户和部门关联表
    /// </summary>
    [Table("User2Apartment")]
    public class User2Apartment : AggregateRoot
    {
        /// <summary>
        /// 用户主键
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 部门主键
        /// </summary>
        public long ApartmentId { get; set; }

    }

}
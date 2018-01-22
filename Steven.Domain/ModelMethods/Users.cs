using Dapper.Contrib.Extensions;

namespace Steven.Domain.Models
{
    public partial class Users
    {
        /// <summary>
        /// 用户缓存Key前缀
        /// </summary>
        public const string GIdPrefix = "UsersGId-";

        /// <summary>
        /// 用户缓存Key
        /// </summary>
        [Write(false)]
        public string GId
        {
            get
            {
                return GIdPrefix + Id;
            }
        }

        /// <summary>
        /// 角色名称
        /// </summary>
        [Write(false)]
        public string RoleName { get; set; }
    }
}

using Steven.Domain.Infrastructure;
using Dapper.Contrib.Extensions;

namespace Steven.Domain.Models
{
    /// <summary>
    /// 用户第三方平台账号信息
    /// </summary>
    [Table("UsersMedias")]
    public class UsersMedia : AggregateRoot
    {
        /// <summary>
        /// 用户主键
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 用户在第三方平台的昵称
        /// </summary>
        public string UserName { get; set; }
        
        /// <summary>
        /// 同一用户在不同微信公众号下，他的OpenId都是不同的
        /// </summary>
        public string UserOpenId { get; set; }

        /// <summary>
        /// 同一用户，对同一个微信开放平台下的不同应用（移动应用、网站应用和公众帐号），unionid是相同的
        /// </summary>
        public string UserUnionId { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string AvatarPic { get; set; }

    }
}

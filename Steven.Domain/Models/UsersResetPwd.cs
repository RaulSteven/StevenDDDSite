using Steven.Domain.Infrastructure;
using Dapper.Contrib.Extensions;

namespace Steven.Domain.Models
{
    /// <summary>
    /// 重置密码记录
    /// </summary>
    [Table("UsersResetPwd")]
    public class UsersResetPwd : AggregateRoot
    {
        /// <summary>
        /// 用户主键
        /// </summary>
        public long ResetUserId { get; set; }

        /// <summary>
        /// 编号，随机
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 邮件内容
        /// </summary>
        public string EmailContent { get; set; }

        /// <summary>
        /// 是否使用
        /// </summary>
        public bool Used { get; set; }
    }
}

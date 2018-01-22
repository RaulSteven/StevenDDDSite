using Steven.Domain.Enums;
using Steven.Domain.Infrastructure;
using Dapper.Contrib.Extensions;

namespace Steven.Domain.Models
{
    /// <summary>
    /// 用户、管理员
    /// </summary>
    [Table("Users")]
    public partial class Users : AggregateRoot
    {
        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 密码盐
        /// </summary>
        public string PasswordSalt { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 登录次数
        /// </summary>
        public int LoginCount { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public long HeadImageId { get; set; }

        /// <summary>
        /// 性别，0:保密;1:男;2:女
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 加入方式
        /// </summary>
        public UserJoinWay JoinWay { get; set; }

        /// <summary>
        /// 设备号，用于APP
        /// </summary>
        public string DeviceCode { get; set; }

        /// <summary>
        /// 所属平台
        /// </summary>
        public UserPaltForm PaltForm { get; set; }

        /// <summary>
        /// 极光注册号
        /// </summary>
        public string JPushRegId { get; set; }

        /// <summary>
        /// 用户分组：管理员、普通用户
        /// </summary>
        public UserGroup UserGroup { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public CommonStatus CommonStatus { get; set; }
    }

}
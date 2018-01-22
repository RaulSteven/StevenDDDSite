using Steven.Domain.Infrastructure;
using Steven.Domain.Enums;
using Dapper.Contrib.Extensions;

namespace Steven.Domain.Models
{
    /// <summary>
    /// 配置表
    /// </summary>
    [Table("SysConfig")]
    public class SysConfig : AggregateRoot
    {
        /// <summary>
        /// 配置Key
        /// </summary>
        public string ConKey { get; set; }

        /// <summary>
        /// 配置值，Json格式
        /// </summary>
        public string ConValue { get; set; }

        /// <summary>
        /// 配置名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 值类型，int、long等
        /// </summary>
        public SysConfigType ConfigType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 配置分类：网站配置、上传配置等
        /// </summary>
        public SysConfigClassify ConfigClassify { get; set; }
    }
}

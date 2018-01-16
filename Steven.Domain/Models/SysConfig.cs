using System.ComponentModel.DataAnnotations;
using Steven.Domain.Infrastructure;
using Steven.Domain.Enums;
using Dapper.Contrib.Extensions;

namespace Steven.Domain.Models
{
    [Table("SysConfig")]
    public class SysConfig : AggregateRoot
    {

        [Required]
        [StringLength(50)]
        public string ConKey { get; set; }

        [Required]
        public string ConValue { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public SysConfigType ConfigType { get; set; }

        [StringLength(250)]
        public string Remark { get; set; }

        /// <summary>
        /// 配置分类
        /// </summary>
        public SysConfigClassify ConfigClassify { get; set; }
    }
}

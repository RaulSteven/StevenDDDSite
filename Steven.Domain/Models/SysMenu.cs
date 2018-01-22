using Steven.Domain.Infrastructure;
using Steven.Domain.Enums;
using Dapper.Contrib.Extensions;

namespace Steven.Domain.Models
{
    /// <summary>
    /// 后台菜单
    /// </summary>
    [Table("SysMenu")]
    public partial class SysMenu : AggregateRoot
    {
        /// <summary>
        /// 父菜单主键
        /// </summary>
        public long Pid { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Icon，图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 排序值，倒序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 数据源
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 二叉树Path，便于查询。如，主键是1：00000001。主键是2，Pid是1：00000001.00000002
        /// </summary>

        public string TreePath { get; set; }

        /// <summary>
        /// 按钮
        /// </summary>
        public SysButton Buttons { get; set; }
    }

}
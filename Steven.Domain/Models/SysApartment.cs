using Steven.Domain.Infrastructure;
using Dapper.Contrib.Extensions;

namespace Steven.Domain.Models
{
    /// <summary>
    /// 部门
    /// </summary>
    [Table("SysApartment")]
    public partial class SysApartment : AggregateRoot
    {
        /// <summary>
        /// 父部门主键
        /// </summary>
        public long Pid { get; set; }

        /// <summary>
        /// 排序值，倒序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }

        /// <summary>
        /// 二叉树Path，便于查询。如，主键是1：00000001。主键是2，Pid是1：00000001.00000002
        /// </summary>
        public string TreePath { get; set; }

    }
}


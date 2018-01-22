using Steven.Domain.Enums;
using Steven.Domain.Infrastructure;
using Dapper.Contrib.Extensions;

namespace Steven.Domain.Models
{
    /// <summary>
    /// 操作日志
    /// </summary>
    [Table("SysOperationLog")]
    public class SysOperationLog:AggregateRoot
    {
        /// <summary>
        /// 数据源
        /// </summary>
        public TableSource LogCat { get; set; }

        /// <summary>
        /// 操作类型：插入、更新、删除等
        /// </summary>
        public OperationType LogType { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string LogTitle { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string LogDesc { get; set; }

        /// <summary>
        /// 数据源说明
        /// </summary>
        public string DataSource { get; set; }

        /// <summary>
        /// 数据源主键值
        /// </summary>
        public string DataSouceId { get; set; }

        /// <summary>
        /// 操作IP地址
        /// </summary>
        public string CreateIP { get; set; }
    }
}

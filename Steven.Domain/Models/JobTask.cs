using System;
using Steven.Domain.Enums;
using Steven.Domain.Infrastructure;
using Dapper.Contrib.Extensions;

namespace Steven.Domain.Models
{
    /// <summary>
    /// 后台任务
    /// </summary>
    [Table("JobTask")]
    public class JobTask:AggregateRoot
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        public Guid TaskId { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        public string TaskName { get; set; }

        /// <summary>
        /// 任务执行参数
        /// </summary>
        public string TaskParam { get; set; }

        /// <summary>
        /// 运行频率设置
        /// </summary>
        public string CronExpressionString { get; set; }

        /// <summary>
        /// 任务运频率中文说明
        /// </summary>
        public string CronRemark { get; set; }

        /// <summary>
        /// 任务所在DLL对应的程序集名称
        /// </summary>
        public string Assembly { get; set; }

        /// <summary>
        /// 任务所在类，包含命名空间
        /// </summary>
        public string Class { get; set; }
        
        /// <summary>
        /// 任务最近运行时间
        /// </summary>
        public DateTime? RecentRunTime { get; set; }

        /// <summary>
        /// 任务下次运行时间
        /// </summary>
        public DateTime? LastRunTime { get; set; }

        /// <summary>
        /// 任务备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 是否要删掉旧的任务
        /// </summary>
        public bool IsDeleteOldTask { get; set; }

        /// <summary>
        /// 任务状态：运行、停止
        /// </summary>
        public JobTaskStatus Status{get;set;}
    }
}

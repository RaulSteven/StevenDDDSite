using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Steven.Core.Utilities;
using Steven.Domain.Enums;
using Steven.Domain.Infrastructure;
using Dapper.Contrib.Extensions;

namespace Steven.Domain.Models
{

    [Table("JobTask")]
    public class JobTask:AggregateRoot
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        [Required]
        public Guid TaskId { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        [DisplayName("任务名称")]
        [MaxLength(255, ErrorMessage = ErrorMsgUtils.MaxStringLength)]
        public string TaskName { get; set; }

        /// <summary>
        /// 任务执行参数
        /// </summary>
        [DisplayName("任务参数")]
        public string TaskParam { get; set; }

        /// <summary>
        /// 运行频率设置
        /// </summary>
        [DisplayName("Cron表达式")]
        [MaxLength(200, ErrorMessage = ErrorMsgUtils.MaxStringLength)]
        public string CronExpressionString { get; set; }

        /// <summary>
        /// 任务运频率中文说明
        /// </summary>
        [DisplayName("表达式说明")]
        [MaxLength(300, ErrorMessage = ErrorMsgUtils.MaxStringLength)]
        public string CronRemark { get; set; }

        /// <summary>
        /// 任务所在DLL对应的程序集名称
        /// </summary>
        [DisplayName("程序集名称")]
        [MaxLength(150, ErrorMessage = ErrorMsgUtils.MaxStringLength)]
        public string Assembly { get; set; }

        /// <summary>
        /// 任务所在类
        /// </summary>
        [DisplayName("类名(包含命名空间)")]
        [MaxLength(150, ErrorMessage = ErrorMsgUtils.MaxStringLength)]
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
        [DisplayName("备注")]
        public string Remark { get; set; }

        /// <summary>
        /// 是否要删掉旧的任务
        /// </summary>
        public bool IsDeleteOldTask { get; set; }

        [DisplayName("状态")]
        public JobTaskStatus Status{get;set;}
    }
}

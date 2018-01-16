using System;
using Autofac;
using Steven.Domain.Repositories;
using Steven.Service.Tasks.Infrastructure;
using Quartz;

namespace Steven.Service.Tasks.Common
{
    /// <summary>
    /// 自定义触发器监听
    /// </summary>
    public class CustomTriggerListener : ITriggerListener
    {
        public string Name
        {
            get
            {
                return "All_TriggerListener";
            }
        }

        /// <summary>
        /// Job执行时调用
        /// </summary>
        /// <param name="trigger">触发器</param>
        /// <param name="context">上下文</param>
        public void TriggerFired(ITrigger trigger, IJobExecutionContext context)
        {

        }


        /// <summary>
        ///  //Trigger触发后，job执行时调用本方法。true即否决，job后面不执行。
        /// </summary>
        /// <param name="trigger"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool VetoJobExecution(ITrigger trigger, IJobExecutionContext context)
        {
            DependencyConfig.Register();
            using (var timeScope = DependencyConfig.Container.BeginLifetimeScope())
            {
                var jobTaskRepository = timeScope.Resolve<IJobTaskRepository>();
                var jobTask = jobTaskRepository.GetByTaskId(trigger.JobKey.Name);
                if (jobTask != null)
                {
                    jobTask.RecentRunTime = DateTime.Now;
                    jobTask.LastRunTime = TimeZoneInfo.ConvertTimeFromUtc(context.NextFireTimeUtc.Value.DateTime,
                        TimeZoneInfo.Local);
                   jobTaskRepository.Save(jobTask);
                }

                return false;
            }
        }

        /// <summary>
        /// Job完成时调用
        /// </summary>
        /// <param name="trigger">触发器</param>
        /// <param name="context">上下文</param>
        /// <param name="triggerInstructionCode"></param>
        public void TriggerComplete(ITrigger trigger, IJobExecutionContext context, SchedulerInstruction triggerInstructionCode)
        {
            DependencyConfig.Register();
            using (var timeScope = DependencyConfig.Container.BeginLifetimeScope())
            {
                var jobTaskRepository = timeScope.Resolve<IJobTaskRepository>();
                var jobTask = jobTaskRepository.GetByTaskId(trigger.JobKey.Name);
                if (jobTask != null)
                {
                    jobTask.LastRunTime = TimeZoneInfo.ConvertTimeFromUtc(context.NextFireTimeUtc.Value.DateTime,
                        TimeZoneInfo.Local);
                    jobTaskRepository.Save(jobTask);
                }
            }
        }

        /// <summary>
        /// 错过触发时调用
        /// </summary>
        /// <param name="trigger">触发器</param>
        public void TriggerMisfired(ITrigger trigger)
        {
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using Autofac;
using Steven.Domain.Enums;
using Steven.Domain.Models;
using Steven.Domain.Repositories;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using Quartz.Impl.Triggers;
using Quartz.Spi;
using Steven.Service.Tasks.Infrastructure;

namespace Steven.Service.Tasks.Common
{
    /// <summary>
    /// 任务处理帮助类
    /// </summary>
    public class QuartzHelper
    {
        private QuartzHelper() { }

        private static object obj = new object();

        /// <summary>
        /// 缓存任务所在程序集信息
        /// </summary>
        private static Dictionary<string, Assembly> AssemblyDict = new Dictionary<string, Assembly>();

        private static IJobTaskRepository _jobTaskRepository;
        /// <summary>
        /// 启用任务调度
        /// 启动调度时会把任务表中状态为“执行中”的任务加入到任务调度队列中
        /// </summary>
        public static void StartScheduler()
        {
            try
            {
                DependencyConfig.Register();
                using (var timeScope = DependencyConfig.Container.BeginLifetimeScope())
                {
                    _jobTaskRepository = timeScope.Resolve<IJobTaskRepository>();
                    if (TaskManager.scheduler != null)
                    {
                        //获取所有执行中的任务
                        List<JobTask> listTask = _jobTaskRepository.GetList();
                        if (listTask != null && listTask.Count > 0)
                        {
                            foreach (JobTask taskUtil in listTask)
                            {
                                try
                                {
                                    ScheduleJob(taskUtil);
                                }
                                catch (Exception e)
                                {
                                   LogHelper.WriteLog(string.Format("任务“{0}”启动失败！", taskUtil.TaskName)+e);
                                }
                            }
                        }
                        LogHelper.WriteLog("任务调度启动成功！");
                    }

                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("任务调度启动失败！"+ex);
            }
        }

        /// <summary>
        /// 删除现有任务
        /// </summary>
        /// <param name="jobKey"></param>
        public static void DeleteJob(string jobKey)
        {
            JobKey jk = new JobKey(jobKey);
            if (TaskManager.scheduler.CheckExists(jk))
            {
                //任务已经存在则删除
                TaskManager.scheduler.DeleteJob(jk);
                LogHelper.WriteLog(string.Format("任务“{0}”已经删除", jobKey));
            }
        }

        /// <summary>
        /// 启用任务
        /// <param name="taskUtil">任务信息</param>
        /// <returns>返回任务trigger</returns>
        /// </summary>
        public static void ScheduleJob(JobTask taskUtil)
        {
            if (taskUtil.IsDeleteOldTask)
            {
                //先删除现有已存在任务
                DeleteJob(taskUtil.TaskId.ToString());
            }
            JobKey jk = new JobKey(taskUtil.TaskId.ToString());
            if (TaskManager.scheduler.CheckExists(jk))
            {
                TaskStatus(taskUtil, jk);
                return;
            }
            if (taskUtil.Status != JobTaskStatus.Deleted)
            {
                //验证是否正确的Cron表达式
                if (ValidExpression(taskUtil.CronExpressionString))
                {
                    IJobDetail job = new JobDetailImpl(taskUtil.TaskId.ToString(), GetClassInfo(taskUtil.Assembly, taskUtil.Class));
                    CronTriggerImpl trigger = new CronTriggerImpl();
                    trigger.CronExpressionString = taskUtil.CronExpressionString;
                    trigger.Name = taskUtil.TaskId.ToString();
                    trigger.Description = taskUtil.TaskName;
                    //添加任务执行参数
                    job.JobDataMap.Add("TaskParam", taskUtil.TaskParam);
                    TaskManager.scheduler.ScheduleJob(job, trigger);
                    TaskStatus(taskUtil, jk);
                    if (taskUtil.IsDeleteOldTask)
                    {
                        taskUtil.IsDeleteOldTask = false;
                        _jobTaskRepository.Save(taskUtil);
                    }
                }
                else
                {
                    LogHelper.WriteLog(taskUtil.CronExpressionString + "不是正确的Cron表达式,无法启动该任务!");
                }
            }
        }

        private static void TaskStatus(JobTask taskUtil, JobKey jk)
        {
            if (taskUtil.Status == JobTaskStatus.Disabled)
            {
                PauseJob(jk);
            }
            else if (taskUtil.Status == JobTaskStatus.Deleted)
            {
                //删除已存在任务
                DeleteJob(taskUtil.TaskId.ToString());
            }
            else
            {
                ResumeJob(jk);
            }
        }

        /// <summary>
        /// 暂停任务
        /// </summary>
        /// <param name="jk"></param>
        public static void PauseJob(JobKey jk)
        {
            if (TaskManager.scheduler.CheckExists(jk))
            {
                //任务已经存在则暂停任务
                TaskManager.scheduler.PauseJob(jk);
                LogHelper.WriteLog(string.Format("任务“{0}”已经暂停", jk));
            }
        }

        /// <summary>
        /// 恢复运行暂停的任务
        /// </summary>
        /// <param name="jk">任务key</param>
        public static void ResumeJob(JobKey jk)
        {
            if (TaskManager.scheduler.CheckExists(jk))
            {
                //任务已经存在则暂停任务
                TaskManager.scheduler.ResumeJob(jk);
                LogHelper.WriteLog(string.Format("任务“{0}”恢复运行", jk));
            }
        }

        /// 获取类的属性、方法  
        /// <summary>  
        /// <param name="assemblyName">程序集</param>  
        /// <param name="className">类名</param>  
        /// </summary>
        private static Type GetClassInfo(string assemblyName, string className)
        {
            try
            {
                assemblyName = FileHelper.GetAbsolutePath(assemblyName + ".dll");
                Assembly assembly = null;
                if (!AssemblyDict.TryGetValue(assemblyName, out assembly))
                {
                    assembly = Assembly.LoadFrom(assemblyName);
                    AssemblyDict[assemblyName] = assembly;
                }
                Type type = assembly.GetType(className, true, true);
                return type;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 停止任务调度
        /// </summary>
        public static void StopSchedule()
        {
            try
            {
                //判断调度是否已经关闭
                if (!TaskManager.scheduler.IsShutdown)
                {
                    //等待任务运行完成
                    TaskManager.scheduler.Shutdown(true);
                    LogHelper.WriteLog("任务调度停止！");
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("任务调度停止失败！"+ex);
            }
        }

        /// <summary>
        /// 校验字符串是否为正确的Cron表达式
        /// </summary>
        /// <param name="cronExpression">带校验表达式</param>
        /// <returns></returns>
        public static bool ValidExpression(string cronExpression)
        {
            return CronExpression.IsValidExpression(cronExpression);
        }
    }
}
using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Xml;
using log4net;
using Steven.Service.Tasks.Common;
using Steven.Service.Tasks.Infrastructure;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using Quartz.Impl.Triggers;

namespace Steven.Service.Tasks
{
    public class TaskManager
    {
        public static IScheduler scheduler = null;
        public static void Start()
        {
            var log = LogManager.GetLogger("TaskManager");
            log.Info("任务开始");
            try
            {
                DependencyConfig.Register();
                log.Info("完成ioc注入");
                var schedulerFactory = new StdSchedulerFactory();
                scheduler = schedulerFactory.GetScheduler();
                log.Info("创建scheduler");
                var xmlDocument = new XmlDocument();
                var taskPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TasksSchedule.xml");
                xmlDocument.Load(taskPath);
                log.Info("加载tasksSchedule.xml");
                var nodes = xmlDocument.GetElementsByTagName("Task");
                log.Info("任务数量：" + nodes.Count);
                var taskIndex = 1;
                log.Info("开始启动任务！");
                foreach (XmlNode node in nodes)
                {
                    var taskname = node.Attributes["name"].Value;
                    var cronExpression = node.Attributes["cronExpression"].Value;
                    log.Info(string.Format("正在启动任务{0},任务时间Cron表达式：{1}。", taskname, cronExpression));
                    IJobDetail jobDetail = new JobDetailImpl("task" + taskIndex, "group" + taskIndex, Assembly.GetExecutingAssembly().GetType(taskname));
                    var trigger = new CronTriggerImpl("trigger" + taskIndex, "group" + taskIndex, cronExpression);
                    scheduler.ScheduleJob(jobDetail, trigger);
                    taskIndex++;
                }

                log.Info("任务启动完成！");
                if (!scheduler.IsStarted)
                {
                    //添加全局监听
                    scheduler.ListenerManager.AddTriggerListener(new CustomTriggerListener(),
                        GroupMatcher<TriggerKey>.AnyGroup());
                }
                scheduler.Start();
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
           

        }

        public static void Stop()
        {
            var log = LogManager.GetLogger("TaskManager");
            log.Info("开始停止任务！");
            //停止任务
            if (scheduler != null)
            {
                scheduler.PauseAll();
            }
            log.Info("任务停止完成！");
        }
    }
}

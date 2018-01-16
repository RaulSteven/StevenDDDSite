using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using log4net;
using Steven.Domain.Repositories;
using Steven.Service.Tasks.Common;
using Steven.Service.Tasks.Infrastructure;
using Quartz;

namespace Steven.Service.Tasks.Jobs
{
    public abstract class BaseJob : IJob
    {
        private static readonly object LockObj = new object();

        public ILog Log { get; private set; }

        protected BaseJob()
        {
            Log = LogManager.GetLogger(this.GetType().FullName);
        }

        public void Execute(IJobExecutionContext context)
        {
            lock (LockObj)
            {
                try
                {
                    Log.Info("开始任务!");
                    //DependencyConfig.Register();
                    ExcuteTask();
                    Log.Info("结束任务!");
                }
                catch (Exception ex)
                {
                    Log.Error(ex.ToString());
                    //SendEmail(ex);
                }
            }
        }

        public virtual void ExcuteTask() { }

        //TODO：任务改为异步实现，需要修改
        Task IJob.Execute(IJobExecutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
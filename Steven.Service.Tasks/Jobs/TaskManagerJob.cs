using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Service.Tasks.Jobs;
using Steven.Service.Tasks.Common;
using Quartz;

namespace Steven.Service.Tasks.Jobs
{
    public class TaskManagerJob : BaseJob
    {
        public override void ExcuteTask()
        {
            base.ExcuteTask();
            try
            {
                QuartzHelper.StartScheduler();
            }
            catch (Exception ex)
            {
                JobExecutionException e2 = new JobExecutionException(ex);
                Log.Error("测试任务异常"+ex);
                //1.立即重新执行任务 
                e2.RefireImmediately = true;
                //2 立即停止所有相关这个任务的触发器
                //e2.UnscheduleAllTriggers=true; 
            }
        }

    }
}

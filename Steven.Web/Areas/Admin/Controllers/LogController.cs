using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Steven.Web.Framework.Controllers;
using Steven.Domain.Repositories;
using Steven.Domain.Models;
using System.Threading.Tasks;
using System.IO;
using Steven.Web.Areas.Admin.Models;
using System.Text;
using Steven.Core.Utilities;
using Steven.Domain.Enums;
using Steven.Domain.ViewModels;

namespace Steven.Web.Areas.Admin.Controllers
{
    public class LogController : AdminController
    {
        public ISysConfigRepository SysConfigRepository { get; set; }
        public IJobTaskRepository JobTaskRepository { get; set; }
        public ISysOperationLogRepository SysOperationLogRepository { get; set; }
        // GET: Admin/Log
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetLog(string uname, string q, string source, string sourceId)
        {
            var search = GetSearchModel();
            var lst = LogRepository.GetPager(search, source, sourceId, null, null, q, uname, null, null);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        #region 系统日志
        public ActionResult LogList()
        {
            DirectoryInfo d = new DirectoryInfo(SysConfigRepository.LogFilePath);
            var query = d.GetFiles("*.*").Where(file => file.Name.ToLower().EndsWith(".txt"));
            var list = query
                .Select(m => new LogModel
                {
                    Name = m.Name,
                    Length = m.Length,
                    CreatDateTime = m.CreationTime,
                    LastWriteTime = m.LastWriteTime,
                    FullName = m.FullName
                })
                .OrderByDescending(m => m.LastWriteTime)
                .ToList();
            return View(list);
        }

        public ActionResult DownFile(string filePath, string name)//相对路径及完整文件名（有后缀）
        {
            FileStream fs = new FileStream(filePath, FileMode.Open);
            byte[] bytes = new byte[(int)fs.Length];
            fs.Read(bytes, 0, bytes.Length);
            fs.Close();
            Response.Charset = "UTF-8";
            Response.ContentEncoding = Encoding.GetEncoding("UTF-8");
            Response.ContentType = "application/octet-stream";

            Response.AddHeader("Content-Disposition", "attachment; filename=" + name);
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
            return new EmptyResult();
        }

        public FileStreamResult ReadFile(string filepath)
        {
            FileStream stream = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(stream);
            reader.BaseStream.Seek(0L, SeekOrigin.Begin);
            return File(stream, "text/plain");
        }

        #endregion

        #region MyRegion
        public ActionResult JobTask()
        {
            return View();
        }
        public ActionResult GetJobTask(string name, CommonStatus? status)
        {
            var search = GetSearchModel();
            var list = JobTaskRepository.GetPager(name, status, search);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult JobTaskEdit(long id = 0, string reUrl = null)
        {
            ViewBag.ReUrl = reUrl ?? Url.Action("JobTask");
            JobTask model;
            if (id > 0)
            {
                model = JobTaskRepository.Get(id);
                if (model == null)
                {
                    ShowErrorMsg();
                    return Redirect(ViewBag.ReUrl);
                }
            }
            else
            {
                model = new JobTask
                {
                    Status = JobTaskStatus.Enabled,
                    TaskId = Guid.NewGuid()
                };
            }
            return View(model);
        }


        [HttpPost]
        public ActionResult JobTaskChangeStatus(long id)
        {
            var result = new JsonModel();
            if (id <= 0)
            {
                return Json(result);
            }
            var model = JobTaskRepository.Get(id);
            if (model == null)
            {
                return Json(result);
            }
            if (model.Status == JobTaskStatus.Enabled)
            {
                model.Status = JobTaskStatus.Disabled;
            }
            else if (model.Status == JobTaskStatus.Disabled)
            {
                model.Status = JobTaskStatus.Enabled;
            }
            SysOperationLogRepository.Insert(TableSource.JobTask, OperationType.Update, id.ToString());
            result.code = JsonModelCode.Succ;
            result.msg = "修改状态成功！";
            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult JobTaskEdit(JobTask model)
        {
            var result = new JsonModel();

            var type = OperationType.Insert;
            //var lastRunTime = GetTaskeLastRunTime(model.CronExpressionString);
            if (model.Id > 0)
            {
                type = OperationType.Update;
                JobTask oModel = JobTaskRepository.Get(model.Id);
                if (oModel == null)
                {
                    result.msg = "记录不存在！";
                    return Json(result);
                }
                //表达式改变了重新计算下次运行时间
                if (!model.CronExpressionString.Equals(oModel.CronExpressionString, StringComparison.OrdinalIgnoreCase))
                {
                    //model.LastRunTime = lastRunTime;
                    model.IsDeleteOldTask = true;
                }
                else
                {
                    model.LastRunTime = oModel.LastRunTime;
                }
            }
            else
            {
                //model.LastRunTime = lastRunTime;
            }
            JobTaskRepository.Save(model);
            //插入日志
            SysOperationLogRepository.Insert(TableSource.JobTask, type, model.Id);
            result.code = JsonModelCode.Succ;
            result.msg = "保存成功！";
            return Json(result);
        }
        #endregion
    }
}
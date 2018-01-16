using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Steven.Web.Areas.Admin.Models
{
    public class LogModel
    {
        public string Name { get; set; }

        public long Length { get; set; }

        public DateTime CreatDateTime { get; set; }

        public DateTime LastWriteTime { get; set; }

        public FileInfo FileInfo { get; set; }

        public string FullName { get; set; }
    }
}
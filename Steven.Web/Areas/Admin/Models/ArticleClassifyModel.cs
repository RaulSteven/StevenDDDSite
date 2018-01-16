using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Steven.Domain.Enums;

namespace Steven.Web.Areas.Admin.Models
{
    public class ArticleClassifyModel
    {
        public long Id { get; set; }

        public int PId { get; set; }
        
        public string Name { get; set; }
        
        public string Remark { get; set; }

        public string TreePath { get; set; }
        
        public ArticleListType PartialViewCode { get; set; }
        public long PicAttaId { get; set; }

    }
}
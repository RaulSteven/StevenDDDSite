using System.ComponentModel.DataAnnotations;
using Steven.Domain.Enums;
using Steven.Domain.Infrastructure;
using Dapper.Contrib.Extensions;

namespace Steven.Domain.Models
{
    [Table("ArticleClassify")]
    public partial class ArticleClassify : AggregateRoot
    { 
        public int PId { get; set; }

        [Display(Name = "名称")]
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Display(Name = "备注")]
        [StringLength(200)]
        public string Remark { get; set; }

        [Display(Name = "排序")]
        public int Sort { get; set; }

        [Display(Name = "子数量")]
        public int ChildrenCount { get; set; }

        [Display(Name = "深度")]
        public int Depth { get; set; }

        [Required]
        [StringLength(255)]
        public string TreePath { get; set; }

        public ArticleListType PartialViewCode { get; set; }

        public long PicAttaId { get; set; }


    }
}

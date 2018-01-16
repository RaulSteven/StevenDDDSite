using System.ComponentModel.DataAnnotations;
using Steven.Domain.Enums;
using Steven.Domain.Infrastructure;
using Dapper.Contrib.Extensions;

namespace Steven.Domain.Models
{
    [Table("Attachment")]
    public partial class Attachment : AggregateRoot
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public long FileSize { get; set; }

        [Required]
        [StringLength(20)]
        public string FileExt { get; set; }

        [Required]
        [StringLength(255)]
        public string FilePath { get; set; }

        public int SortIndex { get; set; }

        [StringLength(255)]
        public string Descript { get; set; }

        public TableSource Source { get; set; }

        [Display(Name = "数据源Id")]
        public long SourceId { get; set; }


        public int ViewCount { get; set; }
    }
}

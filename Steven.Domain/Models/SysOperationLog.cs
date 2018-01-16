using Steven.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using Steven.Domain.Infrastructure;
using Dapper.Contrib.Extensions;

namespace Steven.Domain.Models
{
    [Table("SysOperationLog")]
    public class SysOperationLog:AggregateRoot
    {
        [Required]
        public TableSource LogCat { get; set; }

        [Required]
        public OperationType LogType { get; set; }

        [Required]
        [StringLength(200)]
        public string LogTitle { get; set; }

        public string LogDesc { get; set; }

        [StringLength(20)]
        public string DataSource { get; set; }

        [StringLength(50)]
        public string DataSouceId { get; set; }
        public string CreateIP { get; set; }
    }
}

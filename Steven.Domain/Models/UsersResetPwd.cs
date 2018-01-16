using System.ComponentModel.DataAnnotations;
using Steven.Domain.Infrastructure;
using Dapper.Contrib.Extensions;

namespace Steven.Domain.Models
{
    [Table("UsersResetPwd")]
    public class UsersResetPwd : AggregateRoot
    {
        public long ResetUserId { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; }
        
        [Required]
        public string EmailContent { get; set; }

        public bool Used { get; set; }
    }
}

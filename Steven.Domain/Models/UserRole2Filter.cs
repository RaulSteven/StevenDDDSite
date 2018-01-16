using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Steven.Core.Utilities;
using Steven.Domain.Infrastructure;
using Dapper.Contrib.Extensions;
using System;

namespace Steven.Domain.Models
{
    [Table("UserRole2Filter")]
    [Serializable]
    public partial class UserRole2Filter : AggregateRoot
    {
        public virtual long RoleId
        {
            get;
            set;
        }

        [DisplayName("名称")]
        [MaxLength(20, ErrorMessage = ErrorMsgUtils.MaxStringLength), Required(ErrorMessage = ErrorMsgUtils.Required)]
        public virtual string Name
        {
            get;
            set;
        }

        [DisplayName("数据源")]
        [MaxLength(20, ErrorMessage = ErrorMsgUtils.MaxStringLength)]
        public virtual string Source
        {
            get;
            set;
        }

        public virtual string FilterGroups
        {
            get;
            set;
        }
    }
}

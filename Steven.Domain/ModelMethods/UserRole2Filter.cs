using Dapper.Contrib.Extensions;
using System.Collections.Generic;

namespace Steven.Domain.Models
{
    public partial class UserRole2Filter
    {
        [Write(false)]
        public List<PropertyModel> SourceProperties { get; set; }
    }
}

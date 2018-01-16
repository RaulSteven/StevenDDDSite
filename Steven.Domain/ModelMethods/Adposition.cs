
using Dapper.Contrib.Extensions;

namespace Steven.Domain.Models
{
    public partial class AdPosition
    {
        [Write(false)]
        public string ImageUrl { get; set; }
    }
}

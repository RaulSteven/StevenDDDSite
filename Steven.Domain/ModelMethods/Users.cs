using Dapper.Contrib.Extensions;

namespace Steven.Domain.Models
{
    public partial class Users
    {
        public const string GIdPrefix = "UsersGId-";
        [Write(false)]
        public string GId
        {
            get
            {
                return GIdPrefix + Id;
            }
        }
        [Write(false)]
        public string RoleName { get; set; }
    }
}

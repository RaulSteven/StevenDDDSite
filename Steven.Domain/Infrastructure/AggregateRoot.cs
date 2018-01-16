using System;
using Dapper.Contrib.Extensions;
using Steven.Core.Extensions;
namespace Steven.Domain.Infrastructure
{
    [Serializable]
    public class AggregateRoot : IAggregateRoot
    {
        [Key]
        public long Id
        {
            get;
            set;
        }
        public long CreateUserId
        {
            get;
            set;
        }


        
        public DateTime UpdateTime
        {
            get;
            set;
        }

        public string CreateUserName
        {
            get;
            set;
        }
        
    }
}

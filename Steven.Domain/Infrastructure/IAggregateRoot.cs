using System;

namespace Steven.Domain.Infrastructure
{
    public interface IAggregateRoot
    {
        long Id { get; set; }

        long CreateUserId { get; set; }
        string CreateUserName { get; set; }

        DateTime UpdateTime { get; set; }
    }
}

using Steven.Domain.Enums;
using Steven.Domain.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;

namespace Steven.Domain.Repositories
{
    public interface IRepository<T>:IDisposable where T:class,IAggregateRoot
    {
        long Save(T obj, IDbTransaction trans = null);
        T Get(long id);

        long Insert(T obj, IDbTransaction trans = null);

        bool Delete(T obj, IDbTransaction trans = null);

        bool Update(T obj, IDbTransaction trans = null);

        string GetIP();

        int BatchDele(TableSource src, string ids, IDbTransaction trans = null);
        int BatchDele(TableSource src, IEnumerable<long> idArra, IDbTransaction trans = null);
    }
}

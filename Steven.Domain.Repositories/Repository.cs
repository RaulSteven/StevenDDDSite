using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Steven.Domain.Infrastructure;
using System.Web;
using log4net;
using Dapper;
using Dapper.Contrib;
using System.Data;
using Steven.Domain.Repositories;
using Steven.Domain.Repositories.Infrastructure;
using Dapper.Contrib.Extensions;
using System.Collections.Concurrent;
using static Dapper.Contrib.Extensions.SqlMapperExtensions;
using Steven.Domain.Enums;
using Steven.Core.Utilities;
using Steven.Domain.ViewModels;
using Steven.Domain.Infrastructure.SysUser;

namespace Steven.Domain.Repositories
{
    public class Repository<T> : IRepository<T> where T : class, IAggregateRoot
    {
        #region fields
        public ILog Log
        {
            get;
            private set;
        }

        private ISysUser _User = null;
        public ISysUser User
        {
            get
            {
                if (_User == null)
                {
                    if (HttpContext.Current == null || HttpContext.Current.User == null)
                    {
                        _User = new MemberUser();
                    }
                    else if (HttpContext.Current.User is ISysUser)
                    {
                        _User = (ISysUser)HttpContext.Current.User;
                    }
                    else
                    {
                        _User = new MemberUser();
                    }
                }
                return _User;
            }
        }

        private IDbConnection _dbConn;

        protected IDbConnection DbConn
        {
            get
            {
                if (_dbConn == null)
                {
                    _dbConn = ConnectionFactory.CreateConnection();
                }
                return _dbConn;
            }
        }
        public string CacheKey = "Steven.";
        #endregion

        #region TableName

        private static readonly ConcurrentDictionary<RuntimeTypeHandle, string> TypeTableName = new ConcurrentDictionary<RuntimeTypeHandle, string>();
        public static TableNameMapperDelegate TableNameMapper;
        protected static string GetTableName(Type type)
        {
            string name;
            if (TypeTableName.TryGetValue(type.TypeHandle, out name)) return name;

            if (TableNameMapper != null)
            {
                name = TableNameMapper(type);
            }
            else
            {
                //NOTE: This as dynamic trick should be able to handle both our own Table-attribute as well as the one in EntityFramework 
                var tableAttr = type
#if COREFX
                    .GetTypeInfo()
#endif
                    .GetCustomAttributes(false).SingleOrDefault(attr => attr.GetType().Name == "TableAttribute") as dynamic;
                if (tableAttr != null)
                    name = tableAttr.Name;
                else
                {
                    name = type.Name + "s";
                    if (type.IsInterface && name.StartsWith("I"))
                        name = name.Substring(1);
                }
            }

            TypeTableName[type.TypeHandle] = name;
            return name;
        }

        protected static string GetTableName()
        {
            var type = typeof(T);
            return GetTableName(type);
        }

        protected string Query(string columns = null)
        {
            return $"select {columns ?? "*"} from {GetTableName()} where 1=1 ";
        }
        protected string AdminQuery(string columns = null)
        {
            return $"select {columns ?? "*"} from {GetTableName()} where 1=1 ";
        }

        protected string Count()
        {
            return $"select count(*) from {GetTableName()} where 1=1 ";
        }

        protected string Delete()
        {
            return $"delete from {GetTableName()} where 1=1 ";
        }
        #endregion

        public Repository()
        {
            #region Log
            Log = LogManager.GetLogger(this.GetType().FullName);
            #endregion
            CacheKey += GetTableName()+".";
        }

        #region CRUD

        public bool Delete(T obj, IDbTransaction trans = null)
        {
            RemoveCache(obj);
            return DbConn.Delete(obj,trans);
        }

        public T Get(long id)
        {
            return DbConn.Get<T>(id);
        }

        public virtual long Save(T obj, IDbTransaction trans = null)
        {
            if (obj.Id > 0)
            {
                Update(obj,trans);
            }else
            {
                Insert(obj,trans);
            }
            RemoveCache(obj);
            return obj.Id;
        }

        public long Insert(T obj,IDbTransaction trans = null)
        {
            obj.CreateUserId = User.UserModel.UserId;
            obj.CreateUserName = User.UserModel.UserName;
            obj.UpdateTime = DateTime.Now;
            return DbConn.Insert(obj,trans);
        }

        public bool Update(T obj, IDbTransaction trans = null)
        {
            obj.UpdateTime = DateTime.Now;
            return DbConn.Update(obj,trans);
        }

        public int BatchDele(TableSource src, string ids, IDbTransaction trans = null)
        {
            var idArra = StringUtility.ConvertToBigIntArray(ids, ',');
           
            return BatchDele(src, idArra,trans);
        }

        public int BatchDele(TableSource src,IEnumerable<long> idArra, IDbTransaction trans = null)
        {
            if (idArra == null || idArra.Count() == 0)
            {
                return 0;
            }
            var sql = $"delete from {src} where Id in @ids";
            return DbConn.Execute(sql, new { ids = idArra },trans);
        }


        #endregion

        #region Pager

        public Pager<T> Pager(string sql, DynamicParameters param,PageSearchModel search)
        {
            return Pager<T>(sql, param, search);
        }
        public Pager<TP> Pager<TP>(string sql, DynamicParameters param, PageSearchModel search)
        {
            var pager = new Pager<TP>();
            var countSql = $"Select Count(*) from ({sql}) c";
            pager.total = DbConn.QueryFirst<int>(countSql, param);

            sql += $" order by {search.Sort} {search.Order} OFFSET {search.Offset} ROWS FETCH NEXT {search.Limit} ROWS ONLY";

            pager.rows = DbConn.Query<TP>(sql, param);

            return pager;
        }
        public Pager<TP> Pager<TP>(string sql, DynamicParameters param, PageSearchSortModel search)
        {
            var pager = new Pager<TP>();
            var countSql = $"Select Count(*) from ({sql}) c";
            pager.total = DbConn.QueryFirst<int>(countSql, param);

            sql += $" order by {search.Sort} OFFSET {search.Offset} ROWS FETCH NEXT {search.Limit} ROWS ONLY";

            pager.rows = DbConn.Query<TP>(sql, param);

            return pager;
        }

        #endregion

        #region Cache

        public virtual void RemoveCache(T entity)
        {
        }
        #endregion

        #region IP
        private IPUtility _ipUtility = null;
        public string GetIP()
        {
            if (_ipUtility == null)
            {
                _ipUtility = new IPUtility();
            }
            return _ipUtility.GetIPAndCity();
        }
        #endregion

        #region 事物

        public TP Trans<TP>(Func<IDbTransaction, TP> func,IsolationLevel level = IsolationLevel.ReadCommitted)
        {
            var trans = DbConn.BeginTransaction(level);
            try
            {
                var result = func(trans);
                trans.Commit();
                return result;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                Log.Error(ex);
                throw;
            }
            finally
            {
                trans.Dispose();
                trans = null;
            }
        }

        #endregion


        public void Dispose()
        {
            if (_dbConn != null)
            {
                _dbConn.Close();
                _dbConn = null;
            }
        }
    }
}

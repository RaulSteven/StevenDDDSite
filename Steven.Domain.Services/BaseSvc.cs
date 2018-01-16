using Steven.Domain.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using log4net;
using System.Data;
using Steven.Domain.Repositories.Infrastructure;
using Steven.Domain.Infrastructure.SysUser;
using Dapper;

namespace Steven.Domain.Services
{
    public class BaseSvc:IDisposable
    {
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

        public ILog Log
        {
            get;
            private set;
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

        public BaseSvc()
        {
            Log = LogManager.GetLogger(this.GetType().FullName);
        }
        #region Pager
        public Pager<TP> Pager<TP>(string sql, DynamicParameters param, PageSearchModel search)
        {
            var pager = new Pager<TP>();
            var countSql = $"Select Count(*) from ({sql}) c";
            pager.total = DbConn.QueryFirst<int>(countSql, param);

            sql += $" order by {search.Sort} {search.Order} OFFSET {search.Offset} ROWS FETCH NEXT {search.Limit} ROWS ONLY";

            pager.rows = DbConn.Query<TP>(sql, param);


            return pager;
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

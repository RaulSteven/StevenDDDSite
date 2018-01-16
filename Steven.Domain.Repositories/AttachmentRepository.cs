using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Steven.Domain.Infrastructure;
using Steven.Domain.Models;
using Steven.Domain.Enums;
using Dapper;
using Steven.Domain.ViewModels;

namespace Steven.Domain.Repositories
{
    public class AttachmentRepository : Repository<Attachment>, IAttachmentRepository
    {
        public Pager<Attachment> GetList(string name, TableSource? src, PageSearchModel search)
        {
            var sql = new StringBuilder(AdminQuery());
            var param = new DynamicParameters();
            if (!string.IsNullOrEmpty(name))
            {
                sql.Append(" and Name like @name");
                param.Add("name", $"%{name}%");
            }
            if (src.HasValue)
            {
                sql.Append(" and Source = @src");
                param.Add("src", src.Value);
            }
            return Pager(sql.ToString(), param, search);
        }

        /// <summary>
        /// 通过文件路径返回所有的记录
        /// </summary>
        /// <param name="filePaths">文件路径</param>
        /// <returns></returns>
        public IEnumerable<Attachment> GetList(string[] filePaths)
        {
            if (filePaths == null || filePaths.Length == 0)
            {
                return null;
            }
            return DbConn.Query<Attachment>(Query() + " and FilePath in @files", new { files = filePaths });
        }

        public IEnumerable<Attachment> GetList(TableSource src)
        {
            long userId = User.UserModel.UserId;
            var sql = Query() + " and CreateUserId = @userId and Source = @src";
            return DbConn.Query<Attachment>(sql, new { userId, src });
        }
    }
}

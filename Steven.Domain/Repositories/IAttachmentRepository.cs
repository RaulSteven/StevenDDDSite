using System.Collections.Generic;
using System.Threading.Tasks;
using Steven.Domain.Infrastructure;
using Steven.Domain.Models;
using Steven.Domain.Enums;
using Steven.Domain.ViewModels;

namespace Steven.Domain.Repositories
{
    public interface IAttachmentRepository : IRepository<Attachment>
    {
        Pager<Attachment> GetList(string name, TableSource? src, PageSearchModel search);

        /// <summary>
        /// 通过文件路径返回所有的记录
        /// </summary>
        /// <param name="filePaths">文件路径</param>
        /// <returns></returns>
        IEnumerable<Attachment> GetList(string[] filePaths);

        IEnumerable<Attachment> GetList(TableSource src);
    }
}

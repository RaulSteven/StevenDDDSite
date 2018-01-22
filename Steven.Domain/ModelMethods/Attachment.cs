using Steven.Core.Extensions;
using Dapper.Contrib.Extensions;

namespace Steven.Domain.Models
{
    public partial class Attachment
    {
        /// <summary>
        /// 文件大小
        /// </summary>
        [Write(false)]
        public string FileSizeStr
        {
            get
            {
                return FileSize.ToFileSize();
            }
        }
    }
}

using Steven.Core.Extensions;
using Dapper.Contrib.Extensions;

namespace Steven.Domain.Models
{
    public partial class Attachment
    {
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

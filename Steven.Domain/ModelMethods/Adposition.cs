
using Dapper.Contrib.Extensions;

namespace Steven.Domain.Models
{
    public partial class AdPosition
    {
        /// <summary>
        /// 广告位图片链接
        /// </summary>
        [Write(false)]
        public string ImageUrl { get; set; }
    }
}

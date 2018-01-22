using Dapper.Contrib.Extensions;

namespace Steven.Domain.Models
{
    public partial class ArticleClassify
    {
        /// <summary>
        /// 文章分类显示的partialview
        /// </summary>
        [Write(false)]
        public string ViewCode { get { return $"{PartialViewCode}"; } }

        /// <summary>
        /// 图片链接url
        /// </summary>
        [Write(false)]
        public string PicUrl { get; set; }
    }
}

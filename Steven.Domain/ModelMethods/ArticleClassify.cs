using Dapper.Contrib.Extensions;

namespace Steven.Domain.Models
{
    public partial class ArticleClassify
    {
        [Write(false)]
        public string ViewCode { get { return PartialViewCode + ""; } }

        [Write(false)]
        public string PicUrl { get; set; }
    }
}

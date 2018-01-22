using Dapper.Contrib.Extensions;

namespace Steven.Domain.Models
{
    public partial class Article
    {
        /// <summary>
        /// 分类名称
        /// </summary>
        [Write(false)]
        public string ClassifyName { get; set; }
    }
}

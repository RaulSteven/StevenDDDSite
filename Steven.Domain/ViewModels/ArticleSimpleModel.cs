using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steven.Domain.ViewModels
{
    public class ArticleSimpleModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public DateTime ArticleDateTime { get; set; }
    }

    public class ArticleListItemModel:ArticleSimpleModel
    {
        public string Author { get; set; }
        public long ClassifyId { get; set; }
        public string ClassifyName { get; set; }
        public long ClassifyPicAttaId { get; set; }


    }
}

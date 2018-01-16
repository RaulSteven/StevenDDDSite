using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steven.Domain.ViewModels
{
    public class ProductSelectModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string PicAttIds { get; set; }
        public long ProductClassifyId { get; set; }
        public string PicUrl { get; set; }
        public string MainPushPicUrl { get; set; }

        public bool Selected { get; set; }
        public decimal Price { get; set; }
        public decimal OldPrice { get; set; }
    }
}

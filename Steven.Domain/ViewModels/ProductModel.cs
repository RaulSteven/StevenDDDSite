using Steven.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.ViewModels;

namespace Steven.Domain.ViewModels
{
    public class ProductModel
    {
        public long Id { get; set; }
        public long ShopId { get; set; }
        public string ShopName { get; set; }
        public string PicAttIds { get; set; }
        public string Title { get; set; }
        public string Unit { get; set; }
        public ProductStatus Status { get; set; }
        public decimal Price { get; set; }
        public decimal OldPrice { get; set; }
        public int Stock { get; set; }
        public long ProductClassifyId { get; set; }
        public string ProductClassifyName { get; set; }
        public string Descript { get; set; }
        public int Sort { get; set; }
        public ProductTag Tag { get; set; }

        public IEnumerable<ProductSpecsModel> LstSpecs { get; set; }

        public string LstSysSpecs { get; set; }

        public string SpecsJson { get; set; }

        public IEnumerable<TypeaheadModel> LstClassify { get; set; }
        public string LstUnit { get; set; }
    }
}

using Steven.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Steven.Core.Extensions;
using Steven.Domain.ViewModels;
using MLS.Domain.APIModels;

namespace Steven.Domain.APIModels
{
    [DataContract]
    public class HomeProductModel
    {
        public HomeProductModel()
        {
            List = new List<HomeProductListModel>();
        }

        [DataMember]
        public string Columntitle { get; set; }

        [DataMember]
        public string Subtitle { get; set; }

        [DataMember]
        public List<HomeProductListModel> List { get; set; }
    }

    [DataContract]
    public class HomeProductListModel
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        public decimal PriceDecimal { get; set; }
        [DataMember]
        public string Price { get; set; }

        public decimal OldPrice { get; set; }
        [DataMember]
        public string OriginalPrice { get; set; }

        [DataMember]
        public string Unit { get; set; }

        public string PicAttIds { get; set; }
        [DataMember]
        public string Img { get; set; }

        public int Sort { get; set; }

        public DateTime? UpdateTime { get; set; }
    }

    [DataContract]
    public class HomeClassifyModel
    {
        public HomeClassifyModel()
        {
            List = new List<HomeClassifyListModel>();
        }

        [DataMember]
        public string Columntitle { get; set; }

        [DataMember]
        public List<HomeClassifyListModel> List { get; set; }
    }

    [DataContract]
    public class HomeClassifyListModel
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        public long IconAttaId { get; set; }
        [DataMember]
        public string Icon { get; set; }

        public int Sort { get; set; }
    }

    [DataContract]
    public class ProductDetailModel
    {

        public ProductDetailModel()
        {
            Imgs = new List<string>();
            BigImgs = new List<string>();
            Detail = new List<ProductDetailSpecsModel>();
            BuyType = new List<BuyTypeBizModel>();
        }
        [DataMember]
        public List<string> Imgs { get; set; }

        [DataMember]
        public List<string> BigImgs { get; set; }

        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Price { get; set; }

        [DataMember]
        public string OriginalPrice { get; set; }

        [DataMember]
        public string Unit { get; set; }

        [DataMember]
        public int Stock { get; set; }

        [DataMember]
        public int CartNum { get; set; }

        [DataMember]
        public List<ProductDetailSpecsModel> Detail { get; set; }

        [DataMember]
        public List<BuyTypeBizModel> BuyType { get; set; }

        [DataMember]
        public string Descript { get; set; }
    }
    [DataContract]
    public class ProductDetailSpecsModel
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Value { get; set; }
    }
    
    [DataContract]
    public class HomeDataModel
    {
        public HomeDataModel()
        {
            TodayHotShow = false;
            TodayHot = new HomeProductModel();
            ConvenienceShow = false;
            Convenience = new HomeClassifyModel();
            Today24HotShow = false;
            Today24Hot = new HomeProductModel();
            TodayNewsShow = false;
            TodayNews = new HomeProductModel();
            InfoShow = false;
            HomeInfo = new HomeInfoModel();
        }
        [DataMember]
        public bool TodayHotShow { get; set; }
        [DataMember]
        public HomeProductModel TodayHot { get; set; }

        [DataMember]
        public bool ConvenienceShow { get; set; }
        [DataMember]
        public HomeClassifyModel Convenience { get; set; }

        [DataMember]
        public bool Today24HotShow { get; set; }
        [DataMember]
        public HomeProductModel Today24Hot { get; set; }

        [DataMember]
        public bool TodayNewsShow { get; set; }
        [DataMember]
        public HomeProductModel TodayNews { get; set; }

        [DataMember]
        public bool InfoShow { get; set; }
        [DataMember]
        public HomeInfoModel HomeInfo { get; set; }
    }

    [DataContract]
    public class CartProDataModel
    {
        public CartProDataModel()
        {
            TodayNewsShow = false;
            TodayNews = new HomeProductModel();
        }
        [DataMember]
        public bool TodayNewsShow { get; set; }
        [DataMember]
        public HomeProductModel TodayNews { get; set; }
    }
    [DataContract]
    public class HomeInfoModel
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Img { get; set; }
        [DataMember]
        public string Logo { get; set; }

        [DataMember]
        public bool ShowContact { get; set; }
        [DataMember]
        public string Contact { get; set; }

        [DataMember]
        public bool ShowAddress { get; set; }
        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public bool ShowOthers { get; set; }
        [DataMember]
        public string Others { get; set; }

        [DataMember]
        public double Lat { get; set; }

        [DataMember]
        public double Lng { get; set; }
    }
}

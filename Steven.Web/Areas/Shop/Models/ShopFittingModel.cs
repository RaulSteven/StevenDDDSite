using Steven.Domain.APIModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Steven.Web.Areas.Shop.Models
{
    public class ShopFittingModel
    {
        public Steven.Domain.Models.Shop Shop { get; set; }
        #region 主推
        public bool MainPushHasSelected { get; set; }
        public string MainPushTitle { get; set; }
        public string MainPushSubTitle { get; set; }
        public string MainPushProductIds { get; set; }
        #endregion

        #region 分组
        public bool ClassifyHasSelected { get; set; }
        public string ClassifyTitle { get; set; }

        public List<HomeClassifyListModel> LstClassify { get; set; }
        #endregion

        #region 热卖

        public bool HotSaleHasSelected { get; set; }
        public string HotSaleTitle { get; set; }
        public string HotSaleSubTitle { get; set; }

        public string HotSaleProductIds { get; set; }
        #endregion

        #region 今日新品
        public bool NewHasSelected { get; set; }
        public string NewTitle { get; set; }
        public string NewSubTitle { get; set; }
        public string NewProductIds { get; set; }
        #endregion

        #region 店铺展示
        public bool ShowHasSelected { get; set; }
        public bool ShowContact { get; set; }
        public bool ShowAddress { get; set; }
        public bool ShowOthers { get; set; }
        public string Others { get; set; }
        #endregion

        public string JsonProducts { get; set; }
    }

    public class ShowJsonData
    {
        public bool ShowContact { get; set; }
        public bool ShowAddress { get; set; }
        public bool ShowOthers { get; set; }
    }
}
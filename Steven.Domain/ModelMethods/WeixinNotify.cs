using Steven.Core.Extensions;
using Steven.Domain.Enums;
using Dapper.Contrib.Extensions;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;

namespace Steven.Domain.Models
{
    public partial class WeixinNotify
    {
        #region notmapped properties
        [Write(false)]
        public string UserName { get; set; }

        [Write(false)]
        public string TemplateName { get; set; }

        [Write(false)]
        public NotifyTemplateData NotifyTemplateData { get; set; }

        #endregion

        #region methods

        public void InitialShopOrderTemplateData(ShopOrder order, string proName, ShopTemplate template)
        {
            if (template == null)
            {
                return;
            }
            if (order == null)
            {
                return;
            }
            NotifyTemplateData = new NotifyTemplateData()
            {
                keyword1 = new TemplateDataItem(order.Code),
                keyword2 = new TemplateDataItem(proName),
                keyword3 = new TemplateDataItem(order.TotalPrice.ToString("F2")),             
            };
            switch (template.TemplateType)
            {
                case TemplateType.UserOrderDown:
                    NotifyTemplateData.first = new TemplateDataItem("有新的订单啦！");
                    NotifyTemplateData.keyword4 = new TemplateDataItem(order.CreateTime.ToDisplayDateTime());
                    break;
                case TemplateType.UserPay:
                    NotifyTemplateData.first = new TemplateDataItem("有用户支付啦！");
                    NotifyTemplateData.keyword1 = new TemplateDataItem(proName);
                    NotifyTemplateData.keyword2 = new TemplateDataItem(order.Code);
                    NotifyTemplateData.keyword4 = new TemplateDataItem(order.PayTime.ToDisplayDateTime());
                    break;
                case TemplateType.UserTake:
                    NotifyTemplateData.first = new TemplateDataItem("有用户确认收货啦！");
                    NotifyTemplateData.keyword2 = new TemplateDataItem(order.Receiver);
                    break;
            }
        }

        #endregion


    }

    public class NotifyTemplateData
    {
        public TemplateDataItem first { get; set; }
        public TemplateDataItem keyword1 { get; set; }

        public TemplateDataItem keyword2 { get; set; }

        public TemplateDataItem keyword3 { get; set; }

        public TemplateDataItem keyword4 { get; set; }
        public TemplateDataItem remark { get; set; }

    }
}
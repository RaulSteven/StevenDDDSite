using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using Steven.Core.Extensions;
using Steven.Domain.APIModels;
using Steven.Domain.Enums;
using Steven.Domain.Infrastructure;
using Steven.Domain.Models;
using Steven.Domain.Repositories;
using Dapper;
using Newtonsoft.Json;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using Senparc.Weixin.MP.Containers;
using Senparc.Weixin.WxOpen.AdvancedAPIs.Template;

namespace Steven.Domain.Services
{
    public class ShopOrderSvc : BaseSvc, IShopOrderSvc
    {
        public IShopOrderRepository ShopOrderRepository { get; set; }
        public IAttachmentSvc AttachmentSvc { get; set; }
        public IShopAppInfoRepository ShopAppInfoRepository { get; set; }
        public IUserAppInfoRepository UserAppInfoRepository { get; set; }
        public IShopTemplateRepository ShopTemplateRepository { get; set; }
        public IShopOrderProductRepository ShopOrderProductRepository { get; set; }
        public IUsersMediaRepository UsersMediaRepository { get; set; }
        public IWeixinNotifyRepository WeixinNotifyRepository { get; set; }
        public ISysConfigRepository SysConfigRepository { get; set; }
        public IShopRepository ShopRepository { get; set; }
        public Pager<OrderModel> GetMemberOrderList(BuyType type,MemberOrderStatus? status, long userId, long shopId, PageSearchModel search)
        {
            var param = new DynamicParameters();
            var sql = new StringBuilder();
            sql.Append("select a.Id as OrderId, a.CreateTime as DTime, a.OrderStatus,a.BuyType, a.TotalPrice,(select isnull(sum(b.Number), 0) from ShopOrderProduct as b where b.OrderId = a.Id) as TotalProNum,(select top 1 b.ProductName from ShopOrderProduct as b where b.OrderId = a.Id) as Name,(select top 1 b.ProductPicAttId from ShopOrderProduct as b where b.OrderId = a.Id) as ImgId from ShopOrder a where a.UserId = @userId and a.ShopId=@shopId and a.BuyType=@type");
            param.Add("userId", userId);
            param.Add("shopId", shopId);
            param.Add("type", type);
            if (status.HasValue && status.Value > 0)
            {
                sql.Append(" and a.OrderStatus=@status");
                if (status.Value == MemberOrderStatus.WaitPay)
                {
                    param.Add("status", OrderStatus.UnPay);
                }
                if (status.Value == MemberOrderStatus.WaitUse)
                {
                    param.Add("status", OrderStatus.Paid);
                }
                if (status.Value == MemberOrderStatus.WaitPending)
                {
                    sql.Append(" or a.OrderStatus=@status2");
                    param.Add("status", OrderStatus.Waiting);
                    param.Add("status2", OrderStatus.ShipmentPending);
                }
                if (status.Value == MemberOrderStatus.Pended)
                {
                    param.Add("status", OrderStatus.Consign);
                }
                if (status.Value == MemberOrderStatus.Completed)
                {
                    param.Add("status", OrderStatus.Completed);
                }
            }
            var list = Pager<OrderModel>(sql.ToString(), param, search);
            foreach (var item in list.rows)
            {
                item.Img = AttachmentSvc.GetPicUrl(item.ImgId);
                item.OrderTime = item.DTime.ToDisplayDateTime();
                item.StatusId = ShopOrder.GetMemberOrderStatus(item.OrderStatus,item.BuyType);
                item.Status = item.StatusId.GetDescriotion();
            }
            return list;
        }
        
        public async Task SendTemplate(long orderId, long shopId, long userId, TemplateType type, string formId)
        {
            if (string.IsNullOrEmpty(formId) || formId == "the formId is a mock one")
            {
                return;
            }
            var shopAppInfo = ShopAppInfoRepository.GetByShopId(shopId);
            if (shopAppInfo == null)
            {
                return;
            }
            var userAppInfo = UserAppInfoRepository.GetByUserId(userId, shopId);
            if (userAppInfo == null)
            {
                return;
            }
            var order = ShopOrderRepository.GetByOrderId(orderId, shopId, userId);
            if (order == null)
            {
                return;
            }
            var template = ShopTemplateRepository.GetByShopTemplateType(shopId, type);
            if (template == null)
            {
                return;
            }
            object data = new object();
            switch (type)
            {
                case TemplateType.WaitPay:
                    data = new
                    {
                        keyword1 = new TemplateDataItem(order.Code),
                        keyword2 = new TemplateDataItem(order.TotalPrice.ToString("F2")),
                        keyword3 = new TemplateDataItem(ShopOrderProductRepository.GetShopProName(orderId)),
                        keyword4 = new TemplateDataItem(order.CreateTime.ToDisplayDateTime())
                    };
                    break;
                case TemplateType.Payed:
                    data = new
                    {
                        keyword1 = new TemplateDataItem(order.Code),
                        keyword2 = new TemplateDataItem(ShopOrderProductRepository.GetShopProName(orderId)),
                        keyword3 = new TemplateDataItem(order.TotalPrice.ToString("F2")),
                        keyword4 = new TemplateDataItem(order.CreateTime.ToDisplayDateTime())
                    };
                    break;
            }
            try
            {
                var page = "pages/orderDetail/orderDetail?OrderId=" + order.Id;
                AccessTokenContainer.Register(shopAppInfo.WxAppId, shopAppInfo.WxAppSecrect);
                var wxResult =
                    await
                        TemplateApi.SendTemplateMessageAsync(shopAppInfo.WxAppId, userAppInfo.OpenId, template.TemplateId,
                            data, formId, page);
                Log.Error("发送小程序模板消息error：" + wxResult.errcode);
            }
            catch (Exception ex)
            {
                Log.Error("发送小程序模板消息error：" + ex);
            }

        }

        public void ShopNotifyOrderSave(long shopId, ShopOrder order, TemplateType type)
        {
            var shop = ShopRepository.Get(shopId);
            if (shop == null)
            {
                return;
            }
            var userMedia = UsersMediaRepository.GetByUserId(shop.UserId);
            if (userMedia == null)
            {
                return;
            }
            var notify = new WeixinNotify
            {
                UserOpenId = userMedia.UserOpenId,
                Source = TableSource.ShopOrder,
                SourceId = order.Id
            };
            var template = ShopTemplateRepository.GetByShopTemplateType(shop.Id, type);
            if (template == null || !template.IsUsed)
            {
                return;
            }
            notify.WeixinNotifyTemplateId = template.Id;
            notify.InitialShopOrderTemplateData(
                                order,
                                ShopOrderProductRepository.GetShopProName(order.Id), template);
            notify.NotifyData = JsonConvert.SerializeObject(notify.NotifyTemplateData);
            notify.NotifyUrl = SysConfigRepository.WebSiteUrl + "/Shop/Order";
            WeixinNotifyRepository.Save(notify);
        }
    }
}

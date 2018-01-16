using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Steven.Core.Extensions;
using Steven.Domain.Enums;
using Steven.Domain.Models;
using Steven.Domain.Repositories;
using Steven.Service.Tasks.Infrastructure;
using Newtonsoft.Json;
using Senparc.Weixin;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.Containers;

namespace Steven.Service.Tasks.Jobs
{
    public class WeixinNotifyJob : BaseJob
    {
        public override void ExcuteTask()
        {
            try
            {
                using (var timeScope = DependencyConfig.Container.BeginLifetimeScope())
                {
                    var templateRepository = timeScope.Resolve<IShopTemplateRepository>();
                    var notifyRepository = timeScope.Resolve<IWeixinNotifyRepository>();
                    var configRepository = timeScope.Resolve<ISysConfigRepository>();
                    //查找UserAction，生成对应的Notify，并且发送通知
                    var lst = notifyRepository.GetNeedSendList();
                    if (!lst.Any())
                    {
                        return;
                    }
                    // 获取access_token前，需要全局注册一次
                    AccessTokenContainer.Register(configRepository.WxAppId, configRepository.WxAppSecret);
                    foreach (var notify in lst)
                    {
                        var template = templateRepository.Get(notify.WeixinNotifyTemplateId);
                        if (template == null || !template.IsUsed)
                        {
                            continue;
                        }
                        string templateId = string.Empty;
                        switch (template.TemplateType)
                        {
                            case TemplateType.UserOrderDown:
                                templateId = configRepository.ShopUserDownTemplateId;
                                break;
                            case TemplateType.UserPay:
                                templateId = configRepository.ShopUserPayTemplateId;
                                break;
                            case TemplateType.UserTake:
                                templateId = configRepository.ShopUserTakeTemplateId;
                                break;
                        }
                        if (string.IsNullOrEmpty(templateId))
                        {
                            continue;
                        }
                        var notifyTemplateData = JsonConvert.DeserializeObject<NotifyTemplateData>(notify.NotifyData);
                        // 使用AccessToken发送微信通知，如果遇到AccessToken错误的情况，重新获取AccessToken一次，并重试
                        var result = TemplateApi.SendTemplateMessage(
                            configRepository.WxAppId,
                            notify.UserOpenId,
                            templateId,
                            notify.NotifyUrl,
                            notifyTemplateData);
                        notify.Result = result.errcode;
                        if (notify.Result == ReturnCode.请求成功)
                        {
                            notify.Status=CommonStatus.Enabled;
                        }
                        notifyRepository.Save(notify);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);

            }

        }
    }
}
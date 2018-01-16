using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Core.Extensions;
using Steven.Domain.Enums;
using Steven.Domain.Models;
using Steven.Domain.ViewModels;
using Dapper;
using Newtonsoft.Json;

namespace Steven.Domain.Repositories
{
    public class ShopTemplateRepository : Repository<ShopTemplate>, IShopTemplateRepository
    {
        public List<ShopTemplate> GetList(long shopId)
        {
            var sql = Query() + " and ShopId=@shopId";
            return DbConn.Query<ShopTemplate>(sql, new { shopId }).ToList();
        }

        public ShopTemplate GetByShopTemplateType(long shopId, TemplateType type)
        {
            var sql = Query() + " and ShopId=@shopId and TemplateType=@type";
            return DbConn.QueryFirstOrDefault<ShopTemplate>(sql, new { shopId, type });
        }
        public JsonModel Save(string templateIds, long shopId)
        {
            var result = new JsonModel();
            //var deleSql = "delete from ShopTemplate where ShopId=@shopId";
            //DbConn.Execute(deleSql, new { shopId });
            if (!string.IsNullOrEmpty(templateIds))
            {
                var lst = JsonConvert.DeserializeObject<TemplateJson[]>(templateIds);
                if (lst != null && lst.Length > 0)
                {
                    foreach (var item in lst)
                    {
                        ShopTemplate model = GetByShopTemplateType(shopId, item.TemplateType);
                        if (model == null)
                        {
                            model = new ShopTemplate
                            {
                                Name = item.TemplateType.GetDescriotion(),
                                ShopId = shopId,
                                TemplateType = item.TemplateType,
                                TemplateId = item.TemplateId
                            };
                        }
                        else
                        {
                            model.TemplateId = item.TemplateId;
                        }
                        Save(model);
                    }
                }
            }
            result.code = JsonModelCode.Succ;
            return result;
        }
        public class TemplateJson
        {
            public TemplateType TemplateType { get; set; }
            public string TemplateId { get; set; }
        }

        public void ShopNotifyAdd(long shopId, ISysConfigRepository sysConfigRepository)
        {
            foreach (var item in TemplateType.Closed.GetDescriptDict(TemplateType.UserOrderDown, TemplateType.UserPay, TemplateType.UserTake))
            {
                var model = GetByShopTemplateType(shopId, (TemplateType)item.Key);
                if (model == null)
                {
                    var t = (TemplateType)item.Key;
                    model = new ShopTemplate
                    {
                        Name = t.GetDescriotion(),
                        ShopId = shopId,
                        TemplateType = t,
                        IsUsed = true
                    };
                    model.TemplateId = GetTemplateId(t, sysConfigRepository);
                    Save(model);
                }
            }
        }

        public void ShopNotifyDelete(long shopId)
        {
            foreach (var item in TemplateType.Closed.GetDescriptDict(TemplateType.UserOrderDown, TemplateType.UserPay, TemplateType.UserTake))
            {
                var model = GetByShopTemplateType(shopId, (TemplateType)item.Key);
                if (model != null)
                {
                    Delete(model);
                }
            }
        }

        public bool IsHaveNotify(long shopId, TemplateType type)
        {
            var isHave = false;
            var sql = Query() + " and ShopId=@shopId and TemplateType=@type";
            var model= DbConn.QueryFirstOrDefault<ShopTemplate>(sql, new { shopId, type });
            if (model != null && model.IsUsed)
            {
                isHave = true;
            }
            return isHave;
        }

        public string GetTemplateId(TemplateType t,ISysConfigRepository sysConfigRepository)
        {
            string templateId = string.Empty;
            switch (t)
            {
                case TemplateType.UserOrderDown:
                    templateId = sysConfigRepository.ShopUserDownTemplateId;
                    break;
                case TemplateType.UserPay:
                    templateId = sysConfigRepository.ShopUserPayTemplateId;
                    break;
                case TemplateType.UserTake:
                    templateId = sysConfigRepository.ShopUserTakeTemplateId;
                    break;
            }
            return templateId;
        }
    }
}

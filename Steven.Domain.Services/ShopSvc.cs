using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.ViewModels;
using Steven.Domain.Repositories;
using AutoMapper;
using Steven.Domain.Enums;
using Steven.Domain.Models;
using Steven.Core.Utilities;

namespace Steven.Domain.Services
{
    public class ShopSvc : BaseSvc, IShopSvc
    {
        public ISysOperationLogRepository LogRepository { get; set; }
        public IShopRepository ShopRepository { get; set; }
        public IUsersRepository UsersRepository { get; set; }
        public IShopAppInfoRepository ShopAppInfoRepository { get; set; }
        public IShopFittingRepository ShopFittingRepository { get; set; }
        public IProductRepository ProductRepository { get; set; }
        public IProductClassifyRepository ProductClassifyRepository { get; set; }
        public IShopBuyWayRepository ShopBuyWayRepository { get; set; }
        public JsonModel Save(ShopModel model)
        {
            var result = new JsonModel();
            var opType = OperationType.Insert;
            Shop shop = null;
            Users user = null;
            if (model.Id > 0)
            {
                shop = ShopRepository.Get(model.Id);
                if (shop == null)
                {
                    result.msg = $"找不到id为{model.Id}的商户";
                    return result;
                }
                opType = OperationType.Update;
                user = UsersRepository.Get(shop.UserId);
                if (user == null)
                {
                    ShopRepository.Delete(shop);
                    result.msg = $"找不到id为{model.Id}的商户";
                    return result;
                }
            }
            else
            {
                shop = new Shop();
                user = new Users();
            }
            //事物

            Mapper.Map(model, user);

            if (!string.IsNullOrEmpty(model.Password))
            {
                user.PasswordSalt = HashUtils.GenerateSalt();
                user.Password = HashUtils.HashPasswordWithSalt(model.Password, user.PasswordSalt);
            }
            user.UserGroup = UserGroup.Shop;
            UsersRepository.Save(user);
            Mapper.Map(model, shop);
            shop.UserId = user.Id;
            ShopRepository.Save(shop);

            SaveFitting(opType, shop);

            SaveAppInfo(opType, shop);
            //新增商家默认添加购买方式为配送
            if (model.Id <= 0 && !ShopBuyWayRepository.IsHaveByShop(shop.Id))
            {
                var shopBuyWay = new ShopBuyWay
                {
                    ShopId = shop.Id,
                    Type = BuyType.Delivery
                };
                ShopBuyWayRepository.Save(shopBuyWay);
            }
            LogRepository.Insert(TableSource.Users, opType, user.Id);
            LogRepository.Insert(TableSource.Shop, opType, shop.Id);
            result.code = JsonModelCode.Succ;


            return result;
        }

        private void SaveFitting(OperationType opType, Shop shop)
        {
            if (opType == OperationType.Insert)
            {
                ShopFittingRepository.Save(shop.Id, "今日主推", "每日好物、限时福利、精选指南", true, ShopFittingType.MainPush);
                ShopFittingRepository.Save(shop.Id, "懂吃、会选、有格调", "", true, ShopFittingType.ProductClassify);
                ShopFittingRepository.Save(shop.Id, "24小时热卖", "每日8点更新", true, ShopFittingType.HotSale);
                ShopFittingRepository.Save(shop.Id, "今日新品", "每日0点更新", true, ShopFittingType.TodayNews);
            }
        }

        private void SaveAppInfo(OperationType opType, Shop shop)
        {
            var appInfo = ShopAppInfoRepository.GetByShopId(shop.Id);
            if (appInfo == null)
            {
                var appSecrect = Guid.NewGuid().ToString("N");
                var appId = Math.Abs(appSecrect.GetHashCode()).ToString();
                appInfo = new ShopAppInfo()
                {
                    ShopId = shop.Id,
                    BeiLinAppId = appId,
                    BeiLinAppSecrect = appSecrect
                };
                ShopAppInfoRepository.Save(appInfo);
                LogRepository.Insert(TableSource.ShopAppInfo, opType, appInfo.Id);
            }
        }

        /// <summary>
        /// 是否超过商品限制数
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="limitNum"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public bool IsLimitShopPro(long shopId,ref int limitNum,ref int count)
        {
            var shop = ShopRepository.Get(shopId);
            if (shop == null)
            {
                return false;
            }
            var limit = shop.ProLimitNum;
            var num = ProductRepository.ShopProCount(shopId);
            limitNum = limit;
            count = num;
            if (num >= limit)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 是否超过分组限制数
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="limitNum"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public bool IsLimitShopCls(long shopId, ref int limitNum, ref int count)
        {
            var shop = ShopRepository.Get(shopId);
            if (shop == null)
            {
                return false;
            }
            var limit = shop.ClsLimitNum;
            var num = ProductClassifyRepository.ShopClsCount(shopId);
            limitNum = limit;
            count = num;
            if (num >= limit)
            {
                return false;
            }
            return true;
        }
    }
}

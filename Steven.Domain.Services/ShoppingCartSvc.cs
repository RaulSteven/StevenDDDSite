using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Core.Extensions;
using Steven.Domain.APIModels;
using Steven.Domain.Models;
using Steven.Domain.Repositories;
using Dapper;

namespace Steven.Domain.Services
{
    public class ShoppingCartSvc : BaseSvc, IShoppingCartSvc
    {
        public IShoppingCartRepository ShoppingCartRepository { get; set; }
        public IProductRepository ProductRepository { get; set; }
        public IAttachmentSvc AttachmentSvc { get; set; }
        public IUserShipingAddressRepository ShipingAddressRepository { get; set; }

        public UserCartModel GetUserCart(long userId, long shopId)
        {
            var result = new UserCartModel();
            var cartList = ShoppingCartRepository.GetUserCart(userId, shopId);
            decimal totalPrice = 0;
            var list = new List<UserCartProModel>();
            bool isChooseAll = true;
            foreach (var item in cartList)
            {
                var cart = new UserCartProModel
                {
                    ProductId = item.ProductId,
                    CartId = item.Id,
                    IsChoose = item.IsChoose,
                    Number = item.Number
                };
                var product = ProductRepository.GetApiProduct(shopId, item.ProductId);
                if (product == null)
                {
                    ShoppingCartRepository.Delete(item);//移除出购物车
                    continue;
                }
                cart.Img = AttachmentSvc.GetPicUrl(product.CoverImgId);
                cart.Name = product.Title;
                cart.Unit = product.Unit;
                cart.Price = product.Price;
                if (item.IsChoose)
                {
                    totalPrice = totalPrice + item.Number * product.Price;
                    result.TotalProNum = result.TotalProNum + item.Number;
                }
                else
                {
                    isChooseAll = false;
                }
                list.Add(cart);
            }
            result.List = list;
            result.TotalPrice = totalPrice;
            result.IsChooseAll = isChooseAll;
            return result;
        }

        public void UpdateAllChecked(bool isChecked, long userId, long shopId)
        {
            var sql = "update ShoppingCart set IsChoose=@isChecked where UserId=@userId and ShopId=@shopId";
            DbConn.Execute(sql, new { isChecked, userId, shopId });
        }

        public void DeleteUserCart(long userId, long shopId)
        {
            var sql = "delete from ShoppingCart where IsChoose=@IsChoose and UserId=@userId and ShopId=@shopId";
            DbConn.Execute(sql, new { userId, shopId, IsChoose = true });
        }

        public ConfirmOrderModel GetConfirmOrder(long userId, long shopId)
        {
            var result = new ConfirmOrderModel();
            var cartList = ShoppingCartRepository.GetUserChooseCart(userId, shopId);
            decimal totalPrice = 0;
            var list = new List<ConfirmOrderProModel>();
            foreach (var item in cartList)
            {
                var cart = new ConfirmOrderProModel
                {
                    ProductId = item.ProductId,
                    Number = item.Number
                };
                var product = ProductRepository.GetApiProduct(shopId, item.ProductId);
                if (product == null)
                {
                    ShoppingCartRepository.Delete(item);//移除出购物车
                    continue;
                }
                cart.ImgId = product.CoverImgId;
                cart.Img = AttachmentSvc.GetPicUrl(product.CoverImgId);
                cart.Name = product.Title;
                cart.Unit = product.Unit;
                cart.Price = product.Price;
                totalPrice = totalPrice + item.Number * product.Price;
                list.Add(cart);
            }
            result.List = list;
            result.TotalPrice = totalPrice;
            return result;
        }
        public ConfirmOrderModel GetNowConfirmOrder(long shopId, Product product)
        {
            var result = new ConfirmOrderModel();
            var list = new List<ConfirmOrderProModel>();
            var cart = new ConfirmOrderProModel
            {
                ProductId = product.Id,
                Number = 1,
                ImgId = product.CoverImgId,
                Img = AttachmentSvc.GetPicUrl(product.CoverImgId),
                Name = product.Title,
                Unit = product.Unit,
                Price = product.Price
            };
            var totalPrice = product.Price;
            list.Add(cart);
            result.List = list;
            result.TotalPrice = totalPrice;
            return result;
        }
    }
}

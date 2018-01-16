using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steven.Domain.ViewModels;
using Steven.Domain.Models;
using Steven.Domain.Repositories;
using AutoMapper;
using Steven.Domain.APIModels;
using Steven.Domain.Enums;
using Newtonsoft.Json;
using Dapper;

namespace Steven.Domain.Services
{
    public class ProductSvc : BaseSvc, IProductSvc
    {
        public IProductRepository ProductRepository { get; set; }
        public IProductSpecsRepository ProductSpecsRepository { get; set; }
        public ISysOperationLogRepository LogRepository { get; set; }
        public IProductClassifyRepository ProductClassifyRepository { get; set; }
        public IShopSvc ShopSvc { get; set; }
        public JsonModel Save(ProductModel model)
        {
            var result = new JsonModel();

            var opType = OperationType.Insert;
            Product product = null;
            int limitNum = 0, num = 0;
            if (model.Id == 0)
            {
                if (!ShopSvc.IsLimitShopPro(model.ShopId, ref limitNum, ref num))
                {
                    result.msg = $"您已上传了{num}个商品，不能超过限制数{limitNum}";
                    return result;
                }
                product = new Product();
                num = num + 1;
            }
            else
            {
                opType = OperationType.Update;
                product = ProductRepository.Get(model.Id);
                if (product == null)
                {
                    result.msg = $"找不到Id为{model.Id}的商品！";
                    return result;
                }
            }
            Mapper.Map(model, product);

            ProductRepository.Save(product);
            LogRepository.Insert(TableSource.Product, opType, product.Id);
            //先删除所有规格
            SaveSpecs(model, product);

            result.code = JsonModelCode.Succ;
            result.data = limitNum - num;
            return result;
        }

        private void SaveSpecs(ProductModel model, Product product)
        {
            var deleSql = "delete from ProductSpecs where ProductId=@productId";
            DbConn.Execute(deleSql, new { productId = product.Id });
            if (!string.IsNullOrEmpty(model.SpecsJson))
            {
                var lstSpecs = JsonConvert.DeserializeObject<ProductSpecsModel[]>(model.SpecsJson);
                if (lstSpecs != null && lstSpecs.Length > 0)
                {
                    foreach (var specs in lstSpecs)
                    {
                        var productSpecs = new ProductSpecs()
                        {
                            Name = specs.Name,
                            ProductId = product.Id,
                            ShopId = product.ShopId,
                            SpecsValue = specs.SpecsValue
                        };
                        ProductSpecsRepository.Insert(productSpecs);
                    }
                }
            }
        }
        public List<HomeProductListModel> GetCartProduct(long shopId)
        {
            int count = 4;
            var list = new List<HomeProductListModel>();
            if (User.Identity.IsAuthenticated)
            {
                var param = new DynamicParameters();
                var sql = "select top "+ count + " b.Id,b.Title as Name,b.Price as PriceDecimal,b.OldPrice,b.Unit,b.PicAttIds from UserHistory as a  join Product as b  on a.SourceId=b.Id  where a.ShopId=@shopId and b.Status=@status and a.UserId=@userId and a.Source=@source";
                param.Add("shopId", shopId);
                param.Add("status", ProductStatus.OnSale);
                param.Add("userId", User.UserModel.UserId);
                param.Add("source", TableSource.Product);
                sql = sql + " order by a.UpdateTime desc";
                list.AddRange(DbConn.Query<HomeProductListModel>(sql, param).ToList());
            }

            if (list.Count < count)
            {
                count = count - list.Count;
                var ids = list.Select(m => m.Id);
                var param = new DynamicParameters();
                var query = "select top " + count +
                          " Id,Title as Name,Price as PriceDecimal,OldPrice,Unit as ProductUnit,PicAttIds from Product where 1=1";
                if (ids.Any())
                {
                    query = query + " and (Id not in @ids)";
                    param.Add("ids", ids);
                }
                var baseSql = ProductRepository.ApiBaseSql(query, shopId, ref param);
                baseSql = baseSql + "  order by newid()";
                list.AddRange(DbConn.Query<HomeProductListModel>(baseSql, param).ToList());
            }
            foreach (var item in list)
            {
                item.Price = item.PriceDecimal.ToString("F2");
                item.OriginalPrice = item.OldPrice.ToString("F2");
            }
            return list;
        }
    }
}

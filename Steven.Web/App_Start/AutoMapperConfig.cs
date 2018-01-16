using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Steven.Web.Areas.Admin.Models;
using Steven.Domain.Models;
using Steven.Domain.ViewModels;
using Steven.Web.Areas.Shop.Models;

namespace Steven.Web
{
    public class AutoMapperConfig
    {
        public static void Register()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Users, AdminUserModel>()
                .ForMember(dest => dest.Password, opt => opt.Ignore());
                cfg.CreateMap<AdminUserModel, Users>()
                .ForMember(dest => dest.Password, opt => opt.Ignore());

                cfg.CreateMap<SysConfig, SysConfigModel>();
                cfg.CreateMap<SysConfigModel, SysConfig>();

                cfg.CreateMap<SysApartment, SysApartModel>();
                cfg.CreateMap<SysApartModel, SysApartment>();

                cfg.CreateMap<SysMenu, SysMenuModel>();
                cfg.CreateMap<SysMenuModel, SysMenu>();

                cfg.CreateMap<UserRole, UserRoleModel>();
                cfg.CreateMap<UserRoleModel, UserRole>();

                cfg.CreateMap<ArticleClassify, ArticleClassifyModel>();
                cfg.CreateMap<ArticleClassifyModel, ArticleClassify>();

                cfg.CreateMap<Agent, AgentModel>()
                   .ForMember(dest => dest.Password, opt => opt.Ignore())
                   .ForMember(dest => dest.LoginName, opt=>opt.MapFrom(a=>a.User.LoginName))
                   .ForMember(dest => dest.RealName, opt => opt.MapFrom(a => a.User.RealName))
                   .ForMember(dest => dest.Password, opt => opt.MapFrom(a => a.User.Password))
                   .ForMember(dest => dest.HeadImageId, opt => opt.MapFrom(a => a.User.HeadImageId))
                   .ForMember(dest => dest.CommonStatus, opt => opt.MapFrom(a => a.User.CommonStatus));

                cfg.CreateMap<AgentModel, Agent>();
                cfg.CreateMap<AgentModel, Users>()
                    .ForMember(dest=>dest.Id,opt=>opt.Ignore())
                    .ForMember(dest => dest.Password, opt => opt.Ignore());

                cfg.CreateMap<Shop, ShopModel>()
                   .ForMember(dest => dest.Password, opt => opt.Ignore())
                   .ForMember(dest => dest.LoginName, opt => opt.MapFrom(a => a.User.LoginName))
                   .ForMember(dest => dest.RealName, opt => opt.MapFrom(a => a.User.RealName))
                   .ForMember(dest => dest.Password, opt => opt.MapFrom(a => a.User.Password))
                   .ForMember(dest => dest.HeadImageId, opt => opt.MapFrom(a => a.User.HeadImageId))
                   .ForMember(dest => dest.CommonStatus, opt => opt.MapFrom(a => a.User.CommonStatus));

                cfg.CreateMap<ShopModel, Shop>();
                cfg.CreateMap<ShopModel, Users>()
                    .ForMember(dest => dest.Id, opt => opt.Ignore())
                    .ForMember(dest => dest.Password, opt => opt.Ignore());

                cfg.CreateMap<ShopAppInfo, ShopAppInfoModel>();
                cfg.CreateMap<ShopAppInfoModel, ShopAppInfo>()
                    .ForMember(dest => dest.BeiLinAppId, opt => opt.Ignore())
                    .ForMember(dest => dest.BeiLinAppSecrect, opt => opt.Ignore());

                cfg.CreateMap<ProductClassify, ProductClassifyModel>();
                cfg.CreateMap<ProductClassifyModel, ProductClassify>();

                cfg.CreateMap<Product, ProductModel>();
                cfg.CreateMap<ProductModel, Product>();

                cfg.CreateMap<SysSpecs, SysSpecsModel>();
                cfg.CreateMap<SysSpecsModel, SysSpecs>();

                cfg.CreateMap<Article, ArticleModel>();
                cfg.CreateMap<ArticleModel, Article>();

                cfg.CreateMap<Shop, SettingModel>();
                cfg.CreateMap<SettingModel, Shop>();

                cfg.CreateMap<SysCase, SysCaseModel>();
                cfg.CreateMap<SysCaseModel, SysCase>();

                cfg.CreateMap<SysPartner, SysPartnerModel>();
                cfg.CreateMap<SysPartnerModel, SysPartner>();

                cfg.CreateMap<SysUnit, SysUnitModel>();
                cfg.CreateMap<SysUnitModel, SysUnit>();

                cfg.CreateMap<SysExpress, SysExpressModel>();
                cfg.CreateMap<SysExpressModel, SysExpress>();
            });
        }
    }
}
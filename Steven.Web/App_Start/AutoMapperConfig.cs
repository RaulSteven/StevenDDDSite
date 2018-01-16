using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Steven.Web.Areas.Admin.Models;
using Steven.Domain.Models;
using Steven.Domain.ViewModels;

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

                cfg.CreateMap<Article, ArticleModel>();
                cfg.CreateMap<ArticleModel, Article>();

                cfg.CreateMap<SysCase, SysCaseModel>();
                cfg.CreateMap<SysCaseModel, SysCase>();

                cfg.CreateMap<SysPartner, SysPartnerModel>();
                cfg.CreateMap<SysPartnerModel, SysPartner>();
            });
        }
    }
}
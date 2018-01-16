using Autofac;
using Autofac.Integration.Mvc;
using Steven.Core.Cache;
using Steven.Domain.Infrastructure;
using Steven.Domain.Repositories;
using Steven.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Steven.Web
{
    public class DependencyConfig
    {
        public static void Register()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(Assembly.GetCallingAssembly())
                .PropertiesAutowired();
            builder.RegisterGeneric(typeof(Repository<>))
                .As(typeof(IRepository<>))
                .InstancePerRequest()
                .PropertiesAutowired();

            builder.RegisterAssemblyTypes(typeof(UsersRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerRequest()
                .PropertiesAutowired();

            builder.RegisterAssemblyTypes(typeof(FormsAuthenticationSvc).Assembly)
                .Where(t => t.Name.EndsWith("Svc"))
                .AsImplementedInterfaces()
                .InstancePerRequest()
                .PropertiesAutowired();

            builder.RegisterType<RedisCacheManager>()
                .As<ICacheManager>()
                .SingleInstance()
                .PropertiesAutowired();
            
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
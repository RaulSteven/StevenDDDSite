using Autofac;
using Steven.Core.Cache;
using Steven.Domain.Infrastructure;
using Steven.Domain.Repositories;
using Steven.Domain.Services;

namespace Steven.Service.Tasks.Infrastructure
{
    public class DependencyConfig
    {
        public static void Register()
        {
            var builder = new ContainerBuilder();
            //builder.RegisterType<DbFactory>().As<IDbFactory>().InstancePerLifetimeScope()
            //    .PropertiesAutowired();
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope()
                .PropertiesAutowired();

            builder.RegisterAssemblyTypes(typeof(AttachmentRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();
            builder.RegisterAssemblyTypes(typeof(UserRoleSvc).Assembly)
                .Where(t => t.Name.EndsWith("Svc"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();

            builder.RegisterType<RedisCacheManager>()
                .As<ICacheManager>()
                .SingleInstance()
                .PropertiesAutowired();

            var container = builder.Build();
            Container = container;
            //ApplicationContainer.Container = container;
        }

        public static IContainer Container { get; private set; }
    }
}
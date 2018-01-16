using Autofac;
using Steven.Core.Cache;
using Steven.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steven.UnitTest.Dependency
{
    public class DependencyConfig
    {
        public static void Register()
        {
            var builder = new ContainerBuilder();

            builder.RegisterGeneric(typeof(Repository<>))
                .As(typeof(IRepository<>))
                .InstancePerLifetimeScope()
                .PropertiesAutowired();

            builder.RegisterAssemblyTypes(typeof(SysOperationLogRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();

            builder.RegisterType<MemoryCacheManager>()
                .As<ICacheManager>()
                .SingleInstance()
                .PropertiesAutowired();

            var container = builder.Build();
            Container = container;
        }
        public static IContainer Container { get; private set; }
}
}

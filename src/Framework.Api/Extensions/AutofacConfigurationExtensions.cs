using System.Reflection;
using Autofac;
using Common.LifeTimes;

namespace Framework.Api.Extensions;

public static class AutofacConfigurationExtensions
{
    public static void AddServices(this ContainerBuilder containerBuilder, Type repositoryType,
        Type iRepositoryType, params Assembly[] assemblies)
    {
        //RegisterType > As > Lifetime
        containerBuilder.RegisterGeneric(repositoryType).As(iRepositoryType).InstancePerLifetimeScope();

        // Add scoped services to dependency
        containerBuilder.RegisterAssemblyTypes(assemblies)
            .AssignableTo<IScopedDependency>()
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        // طول عمرش فقط مربوط به همان کلاسی که ازش استفاده می‌کند می‌شود
        // Add transient services to dependency
        containerBuilder.RegisterAssemblyTypes(assemblies)
            .AssignableTo<ITransientDependency>()
            .AsImplementedInterfaces()
            .InstancePerDependency();

        // یکبار ایجاد می‌شود و تا آخر عمر برنامه باقی می‌ماند
        // Add singleton services to dependency
        containerBuilder.RegisterAssemblyTypes(assemblies)
            .AssignableTo<ISingletonDependency>()
            .AsImplementedInterfaces()
            .SingleInstance();
    }
}
using Autofac;
using Core.Domain;

namespace Core.AutofacModules
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(Repository<>))
                   .As(typeof(IRepository<>))
                   .InstancePerDependency();

            builder.RegisterType<UnitOfWork>()
                   .AsImplementedInterfaces()
                   .OnRelease(u =>
                   {
                       u.Commit();
                       u.Dispose();
                   })
                   .InstancePerLifetimeScope();

            builder.RegisterType<QueryableSnapshot>()
                   .AsImplementedInterfaces()
                   .As<IHandle<IFact>>()
                   .SingleInstance();

            builder.RegisterType<AggregateBuilder>()
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            builder.RegisterType<DomainEventBroker>()
                   .AsImplementedInterfaces()
                   .SingleInstance();
        }
    }

    public class HandlerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(Repository<>).Assembly)
                   .Where(t => t.IsClosedTypeOf(typeof(IHandleDuringUnitOfWork<>)))
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(Repository<>).Assembly)
                   .Where(t => t.IsClosedTypeOf(typeof(IHandle<>)))
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();
        }
    }

    public class UtilityModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SystemClock>()
                   .AsImplementedInterfaces()
                   .SingleInstance();
        }
    }
}
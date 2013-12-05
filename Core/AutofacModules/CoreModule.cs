using System.Security.Principal;
using System.Threading;
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
                .SingleInstance()
                .AutoActivate();

            builder.RegisterType<AggregateBuilder>()
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            builder.RegisterType<DomainEventBroker>()
                   .AsImplementedInterfaces()
                   .SingleInstance();

            builder.Register(c => Thread.CurrentPrincipal.Identity)
                   .As<IIdentity>()
                   .InstancePerDependency();
        }
    }
}
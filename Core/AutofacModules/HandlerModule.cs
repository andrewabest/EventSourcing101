using Autofac;

namespace Core.AutofacModules
{
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
}
using Autofac;

namespace Core.AutofacModules
{
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
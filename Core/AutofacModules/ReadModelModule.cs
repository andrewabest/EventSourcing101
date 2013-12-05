using Autofac;
using Core.Domain.ReadModels;

namespace Core.AutofacModules
{
    public class ReadModelModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CustomersNamedAndrewReadModel>()
                .AsImplementedInterfaces()
                .AsSelf()
                .SingleInstance()
                .AutoActivate();
        }
    }
}
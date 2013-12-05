using Autofac;

namespace Core.AutofacModules
{
    public class PersistenceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //    Sample wireup of NEventStore using RavenDb.
            //    builder.Register(c =>
            //    {
            //        var bus = c.Resolve<IDomainEventBroker>();
            //        var factStore =
            //               Wireup.Init()
            //                     .LogToOutputWindow()
            //                     .UsingRavenPersistence(c.Resolve<IDocumentStore>)
            //                     .InitializeStorageEngine()
            //                     .UsingJsonSerialization()
            //                     .UsingSynchronousDispatchScheduler()
            //                     .DispatchTo(bus)
            //                     .Build();
            //        bus.StartDispatching();
            //        return factStore;
            //    })
            //    .As<IStoreEvents>()
            //    .SingleInstance();
            //
        }
    }
}
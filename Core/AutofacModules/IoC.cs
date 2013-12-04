using System.Diagnostics.CodeAnalysis;
using Autofac;
using Autofac.Builder;

namespace Core.AutofacModules
{
    public class IoC
    {
        public static IContainer Container { get; private set; }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Io")]
        public static void LetThereBeIoC(ContainerBuildOptions containerBuildOptions = ContainerBuildOptions.None)
        {
            Container = ConstructContainer(containerBuildOptions);
        }

        public static IContainer ConstructContainer(ContainerBuildOptions containerBuildOptions)
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyModules(typeof (Repository<>).Assembly);
            return builder.Build(containerBuildOptions);
        }
    }
}
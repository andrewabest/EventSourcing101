using Core.AutofacModules;
using NUnit.Framework;

namespace Tests
{
    public abstract class IntegrationTest
    {
        [SetUp]
        public void Setup()
        {
            IoC.LetThereBeIoC();
        }

        [TearDown]
        public void Teardown()
        {
            IoC.Container.Dispose();
        }
    }
}
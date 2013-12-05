using Autofac;
using Core;
using Core.AutofacModules;
using Core.Domain.Aggregates.CustomerAggregate;
using Core.Domain.ReadModels;
using FluentAssertions;
using NUnit.Framework;

namespace Tests
{
    public class WhenTheNameAndrewIsUsedForACustomer : IntegrationTest
    {
        [Test]
        public void ThenTheReadModelContainsTheCustomerIds()
        {
            var originallyTom = Customer.Create("Tom");
            var actuallyAndrew = Customer.Create("Andrew");

            using (var lifetimescope = IoC.Container.Resolve<ILifetimeScope>().BeginLifetimeScope())
            {
                var repository = lifetimescope.Resolve<IRepository<Customer>>();

                repository.Add(originallyTom);
                repository.Add(actuallyAndrew);
            }

            using (var lifetimescope = IoC.Container.Resolve<ILifetimeScope>().BeginLifetimeScope())
            {
                var repository = lifetimescope.Resolve<IRepository<Customer>>();

                var storedCustomer = repository.GetById(originallyTom.Id);

                storedCustomer.ChangeName("Sally");
            }

            using (var lifetimescope = IoC.Container.Resolve<ILifetimeScope>().BeginLifetimeScope())
            {
                var readModel = lifetimescope.Resolve<CustomersNamedAndrewReadModel>();

                readModel.CustomersNamedAndrew.Should().Contain(new[] {originallyTom.Id, actuallyAndrew.Id});
            }
        }
    }
}
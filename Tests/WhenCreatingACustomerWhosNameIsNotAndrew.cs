using Autofac;
using Core;
using Core.AutofacModules;
using Core.Domain.Aggregates.CustomerAggregate;
using FluentAssertions;
using NUnit.Framework;

namespace Tests
{
    public class WhenChangingACustomersNameToNotAndrew : IntegrationTest
    {
        [Test]
        public void ThenTheCustomersNameShouldBeAndrew()
        {
            var customer = Customer.Create("Tom");

            using (var lifetimescope = IoC.Container.Resolve<ILifetimeScope>().BeginLifetimeScope())
            {
                var repository = lifetimescope.Resolve<IRepository<Customer>>();

                repository.Add(customer);
            }

            using (var lifetimescope = IoC.Container.Resolve<ILifetimeScope>().BeginLifetimeScope())
            {
                var repository = lifetimescope.Resolve<IRepository<Customer>>();

                var storedCustomer = repository.GetById(customer.Id);

                storedCustomer.ChangeName("Sally");
            }

            using (var lifetimescope = IoC.Container.Resolve<ILifetimeScope>().BeginLifetimeScope())
            {
                var repository = lifetimescope.Resolve<IRepository<Customer>>();

                var storedCustomer = repository.GetById(customer.Id);

                storedCustomer.Name.Should().Be("Andrew");
            }
        }
    }
}
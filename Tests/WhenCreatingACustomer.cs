using Autofac;
using Core;
using Core.AutofacModules;
using Core.Domain.Aggregates.CustomerAggregate;
using Core.Domain.Aggregates.CustomerAggregate.Facts;
using FluentAssertions;
using NUnit.Framework;

namespace Tests
{
    public class WhenCreatingACustomer : IntegrationTest
    {
        [Test]
        public void ThenTheCustomerShouldContainCreatedFact()
        {
            var customer = Customer.Create("Tom");
            customer.GetPendingFacts().Should().ContainItemsAssignableTo<CustomerCreatedFact>();
        }

        [Test]
        public void ThenTheCustomerShouldBeAvailableToQuery()
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

                storedCustomer.Should().NotBeNull();
            }
        }
    }
}
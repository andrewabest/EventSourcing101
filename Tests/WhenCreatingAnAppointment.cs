using System;
using Autofac;
using Core;
using Core.AutofacModules;
using Core.Domain.Aggregates.CustomerAggregate;
using FluentAssertions;
using NUnit.Framework;

namespace Tests
{
    public class WhenCreatingAnAppointment : IntegrationTest
    {
        [Test]
        public void ThenTheAppointmentShouldBeAvailableToQuery()
        {
            var customer = Customer.Create("Tom");

            var appointment = customer.ScheduleAppointment(DateTimeOffset.UtcNow);

            using (var lifetimescope = IoC.Container.Resolve<ILifetimeScope>().BeginLifetimeScope())
            {
                var repository = lifetimescope.Resolve<IRepository<Customer>>();

                repository.Add(customer);
            }

            using (var lifetimescope = IoC.Container.Resolve<ILifetimeScope>().BeginLifetimeScope())
            {
                var repository = lifetimescope.Resolve<IRepository<Customer>>();

                var storedCustomer = repository.GetById(customer.Id);

                storedCustomer.Appointments.Should().Contain(a => a.Id == appointment.Id);
            }
        } 
    }
}
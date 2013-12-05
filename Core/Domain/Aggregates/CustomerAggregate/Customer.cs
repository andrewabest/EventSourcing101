using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Core.Domain.Aggregates.CustomerAggregate.Facts;

namespace Core.Domain.Aggregates.CustomerAggregate
{
    [Serializable]
    public class Customer : AggregateRoot
    {
        private readonly ICollection<Appointment> _appointments;

        private Customer()
        {
            _appointments = new Collection<Appointment>();
        }

        public static Customer Create(string name)
        {
            var fact = new CustomerCreatedFact() {Name = name, AggregateRootId = Guid.NewGuid()};

            var customer = new Customer();
            customer.Append(fact);
            customer.Apply(fact);

            return customer;
        }

        public string Name { get; private set; }

        public IEnumerable<Appointment> Appointments { get { return _appointments; } }

        public void ChangeName(string name)
        {
            if (Name == name) return;

            var fact = new CustomerNameChangedFact() {Name = name};

            Append(fact);
            Apply(fact);
        }

        public Appointment ScheduleAppointment(DateTimeOffset dateOfAppointment)
        {
            var appointment = Appointment.Create(dateOfAppointment, this);
            _appointments.Add(appointment);
            return appointment;
        }

        public void Apply(CustomerNameChangedFact fact)
        {
            Name = fact.Name;
        }

        public void Apply(AppointmentCreatedFact fact)
        {
            var appointment = Appointment.HydrateFrom(fact, this);
            _appointments.Add(appointment);
        }

        public void Apply(FactAboutAppointment fact)
        {
            var appointment = Appointments.Single(n => n.Id == fact.Id);
            appointment.Apply((dynamic)fact);
        }

        public void Apply(CustomerCreatedFact fact)
        {
            Id = fact.AggregateRootId;
            Name = fact.Name;
        }
    }
}
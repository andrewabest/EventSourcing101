using System;
using System.Dynamic;
using Core.Domain.Aggregates.CustomerAggregate.Facts;

namespace Core.Domain.Aggregates.CustomerAggregate
{
    [Serializable]
    public class Appointment : Entity
    {
        private Appointment(IAppendFacts parent) : base(parent)
        {
        }

        internal static Appointment Create(DateTimeOffset dateOfAppointment, IAppendFacts parent)
        {
            var fact = new AppointmentCreatedFact() {DateOfAppointment = dateOfAppointment};

            var appointment = new Appointment(parent);
            appointment.Append(fact);
            appointment.Apply(fact);

            return appointment;
        }

        internal static Appointment HydrateFrom(AppointmentCreatedFact fact, Customer parent)
        {
            var appointment = new Appointment(parent);
            appointment.Apply(fact);
            return appointment;
        }

        public DateTimeOffset DateOfAppointment { get; private set; }

        public void RescheduleTo(DateTimeOffset rescheduledDate)
        {
            var fact = new AppointmentRescheduledFact() { RescheduledDate = rescheduledDate};
            
            Append(fact);
            Apply(fact);
        }

        public void Apply(AppointmentRescheduledFact fact)
        {
            DateOfAppointment = fact.RescheduledDate;
        }

        public void Apply(AppointmentCreatedFact fact)
        {
            DateOfAppointment = fact.DateOfAppointment;
        }
    }
}
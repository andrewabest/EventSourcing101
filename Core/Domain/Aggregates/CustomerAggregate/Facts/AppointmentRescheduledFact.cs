using System;

namespace Core.Domain.Aggregates.CustomerAggregate.Facts
{
    public class AppointmentRescheduledFact : FactAboutAppointment
    {
        public DateTimeOffset RescheduledDate { get; set; }
    }
}
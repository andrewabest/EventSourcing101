using System;

namespace Core.Domain.Aggregates.CustomerAggregate.Facts
{
    [Serializable]
    public class AppointmentRescheduledFact : FactAboutAppointment
    {
        public DateTimeOffset RescheduledDate { get; set; }
    }
}
using System;

namespace Core.Domain.Aggregates.CustomerAggregate.Facts
{
    public class AppointmentCreatedFact : FactAboutAppointment
    {
        public DateTimeOffset DateOfAppointment { get; set; }
    }
}
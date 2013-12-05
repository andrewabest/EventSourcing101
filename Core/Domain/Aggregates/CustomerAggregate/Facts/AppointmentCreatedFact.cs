using System;

namespace Core.Domain.Aggregates.CustomerAggregate.Facts
{
    [Serializable]
    public class AppointmentCreatedFact : FactAboutAppointment
    {
        public DateTimeOffset DateOfAppointment { get; set; }
    }
}
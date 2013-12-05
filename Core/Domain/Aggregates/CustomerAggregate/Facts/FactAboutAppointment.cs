using System;

namespace Core.Domain.Aggregates.CustomerAggregate.Facts
{
    [Serializable]
    public abstract class FactAboutAppointment : Fact<Customer>
    {
    }
}
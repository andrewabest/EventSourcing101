using System;

namespace Core.Domain.Aggregates.CustomerAggregate.Facts
{
    [Serializable]
    public class CustomerCreatedFact : Fact<Customer>
    {
        public string Name { get; set; }
    }
}
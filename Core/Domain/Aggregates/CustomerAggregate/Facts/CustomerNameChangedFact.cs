using System;

namespace Core.Domain.Aggregates.CustomerAggregate.Facts
{
    [Serializable]
    public class CustomerNameChangedFact : Fact<Customer>
    {
        public string Name { get; set; }
    }
}
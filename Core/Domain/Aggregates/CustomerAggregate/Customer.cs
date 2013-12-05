using System;
using System.Collections.Specialized;
using Core.Domain.Aggregates.CustomerAggregate.Facts;

namespace Core.Domain.Aggregates.CustomerAggregate
{
    [Serializable]
    public class Customer : AggregateRoot
    {
        private Customer()
        {
        }

        public static Customer Create(string name)
        {
            var fact = new CustomerCreatedFact() {Name = name};

            var customer = new Customer();
            customer.Append(fact);
            customer.Apply(fact);

            return customer;
        }

        public string Name { get; set; }

        public void Apply(CustomerCreatedFact fact)
        {
            Name = fact.Name;
        }
    }
}
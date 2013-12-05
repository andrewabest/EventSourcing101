using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Core.Domain.Aggregates.CustomerAggregate.Facts;
using Core.Extensions;

namespace Core.Domain.ReadModels
{
    public class CustomersNamedAndrewReadModel : IHandle<CustomerCreatedFact>, IHandle<CustomerNameChangedFact>
    {
        private readonly HashSet<Guid> _customersNamedAndrew = new HashSet<Guid>();

        public HashSet<Guid> CustomersNamedAndrew
        {
            get { return _customersNamedAndrew; }
        }

        public void Handle(CustomerCreatedFact message)
        {
            ProcessAndStore(message.AggregateRootId, message.Name);
        }

        public void Handle(CustomerNameChangedFact message)
        {
            ProcessAndStore(message.AggregateRootId, message.Name);
        }

        private void ProcessAndStore(Guid id, string name)
        {
            if (name.IsAndrew() && CustomersNamedAndrew.Contains(id) == false)
            {
                CustomersNamedAndrew.Add(id);
            }
        }
    }
}
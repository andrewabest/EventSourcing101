using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Core.Domain
{
    [Serializable]
    public abstract class AggregateRoot : IAggregateRoot, IAppendFacts
    {
        private readonly ConcurrentQueue<IFact> _pendingFacts = new ConcurrentQueue<IFact>();

        public Guid Id { get; protected set; }

        public IEnumerable<IFact> GetPendingFacts()
        {
            IFact fact;

            while (_pendingFacts.TryDequeue(out fact)) yield return fact;
        }

        public void Append(IFact fact)
        {
            if (fact.AggregateRootId == Guid.Empty) fact.AggregateRootId = Id;

            _pendingFacts.Enqueue(fact);
        }
    }
}
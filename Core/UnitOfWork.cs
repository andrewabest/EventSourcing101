using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using Core.Domain;
using Core.Extensions;

namespace Core
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private int _committed; 
        //private readonly IStoreEvents _factStore;
        private readonly IClock _clock;
        private readonly IIdentity _identity;
        private readonly IDomainEventBroker _domainEventBroker;
        private readonly List<IAggregateRoot> _enlistedAggregateRoots = new List<IAggregateRoot>();
        private readonly Guid _unitOfWorkId = Guid.NewGuid();

        public UnitOfWork(
            //IStoreEvents factStore, 
            IClock clock, 
            IIdentity identity, 
            IDomainEventBroker domainEventBroker)
        {
            //_factStore = factStore;

            _clock = clock;
            _identity = identity;
            _domainEventBroker = domainEventBroker;
        }

        public void EnlistAggregate(IAggregateRoot item)
        {
            _enlistedAggregateRoots.Add(item);
        }

        public T TryGetEnlistedAggregateRoot<T>(Guid id) where T : IAggregateRoot
        {
            return _enlistedAggregateRoots.OfType<T>().FirstOrDefault(ar => ar.Id == id);
        }

        public IQueryable<T> EnlistedAggregateRoots<T>() where T : IAggregateRoot
        {
            return _enlistedAggregateRoots.OfType<T>().AsQueryable();
        }

        public void Commit()
        {
            if (Interlocked.CompareExchange(ref _committed, 1, 0) != 0) return;

            var allFactsForThisUnitOfWork = GetAllFactsForThisUnitOfWork();
            SetAllFactDetails(allFactsForThisUnitOfWork);
            DispatchAllFacts(allFactsForThisUnitOfWork);

            //CommitAllFacts(allFactsForThisUnitOfWork);
        }

        private List<IFact> GetAllFactsForThisUnitOfWork()
        {
            var allFactsForThisUnitOfWork = new List<IFact>();

            while (true)
            {
                var factsFromThisPass = _enlistedAggregateRoots
                    .SelectMany(ar => ar.GetPendingFacts())
                    .ToArray();

                if (factsFromThisPass.None()) break;

                allFactsForThisUnitOfWork.AddRange(factsFromThisPass);

                foreach (var fact in factsFromThisPass)
                {
                    _domainEventBroker.RaiseWithinUnitOfWork(fact, this);
                }
            }

            return allFactsForThisUnitOfWork;
        }

        private void SetAllFactDetails(IEnumerable<IFact> allFactsForThisUnitOfWork)
        {
            var userId = _identity.Id();
            var timestamp = _clock.UtcNow;
            var sequenceNumber = 0;

            foreach (var fact in allFactsForThisUnitOfWork)
            {
                fact.SetUnitOfWorkDetails(_unitOfWorkId, sequenceNumber++, timestamp, userId);
            }
        }

        private void DispatchAllFacts(List<IFact> allFactsForThisUnitOfWork)
        {
            allFactsForThisUnitOfWork.ForEach(f => _domainEventBroker.Raise(f));
        }

        private void CommitAllFacts(IEnumerable<IFact> allFactsForThisUnitOfWork)
        {
            throw new NotImplementedException("Add NEventStore and uncomment this, and remove DispatchAllFacts as NEventStore handles dispatch after it commits.");

            //var groupedFacts = allFactsForThisUnitOfWork.GroupBy(f => f.AggregateRootId);

            //foreach (var factGroup in groupedFacts)
            //{

            //    var aggregateRootId = factGroup.First().AggregateRootId;

            //    using (var factStream = _factStore.OpenStream(aggregateRootId, 0, int.MaxValue))
            //    {
            //        foreach (var fact in factGroup)
            //        {
            //            factStream.Add(new EventMessage { Body = fact });
            //        }

            //        factStream.CommitChanges(Guid.NewGuid());
            //    }
            //}
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }
    }
}
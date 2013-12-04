using System;
using System.Collections.Concurrent;
using System.Linq;
using Core.Domain;

namespace Core
{
    public class QueryableSnapshot : IHandle<IFact>, IQueryableSnapshot
    {
        private readonly IAggregateBuilder _builder;

        private readonly ConcurrentDictionary<Type, ConcurrentDictionary<Guid, IAggregateRoot>> _items =
            new ConcurrentDictionary<Type, ConcurrentDictionary<Guid, IAggregateRoot>>();

        public QueryableSnapshot(IAggregateBuilder builder)
        {
            _builder = builder;
        }

        public void Handle(IFact fact)
        {
            var entityType = Type.GetType(fact.AggregateRootTypeName);

            var aggregateDictionary = _items.GetOrAdd(
                entityType,
                t => new ConcurrentDictionary<Guid, IAggregateRoot>());

            var aggregate = aggregateDictionary.GetOrAdd(
                fact.AggregateRootId,
                t => (IAggregateRoot)Activator.CreateInstance(entityType, true)
                );

            Apply(entityType, aggregate, fact);
        }

        private void Apply(Type aggregateType, IAggregateRoot aggregateRoot, IFact fact)
        {
            var openGenericMethod = typeof(IAggregateBuilder).GetMethod("Apply");
            var closedGenericMethod = openGenericMethod.MakeGenericMethod(aggregateType);
            closedGenericMethod.Invoke(_builder, new object[] { aggregateRoot, fact });
        }

        public T GetById<T>(Guid id) where T : IAggregateRoot
        {
            ConcurrentDictionary<Guid, IAggregateRoot> aggregateDictionary;
            if (_items.TryGetValue(typeof(T), out aggregateDictionary))
            {
                IAggregateRoot aggregateRoot;
                if (aggregateDictionary.TryGetValue(id, out aggregateRoot))
                {
                    return (T)aggregateRoot;
                }
            }

            return default(T);
        }

        public IQueryable<T> Items<T>() where T : IAggregateRoot
        {
            ConcurrentDictionary<Guid, IAggregateRoot> aggregateDictionary;
            if (_items.TryGetValue(typeof(T), out aggregateDictionary))
            {
                return aggregateDictionary.Values.Cast<T>().AsQueryable();
            }

            return Enumerable.Empty<T>().AsQueryable();
        }
    }
}
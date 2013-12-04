using System;
using System.Linq;
using Core.Domain;

namespace Core
{
    public class Repository<T> : IRepository<T> where T : class, IAggregateRoot
    {
        private readonly IQueryableSnapshot _queryableSnapshot;
        private readonly IUnitOfWork _unitOfWork;

        public Repository(IQueryableSnapshot queryableSnapshot, IUnitOfWork unitOfWork)
        {
            _queryableSnapshot = queryableSnapshot;
            _unitOfWork = unitOfWork;
        }

        public T GetById(Guid id)
        {
            var aggregateRoot = _queryableSnapshot.GetById<T>(id);

            var clone = aggregateRoot.BinaryClone();

            _unitOfWork.EnlistAggregate(clone);
            return clone;
        }

        public void Add(T item)
        {
            _unitOfWork.EnlistAggregate(item);
        }

        public T[] Query(IQuery<T> query)
        {
            return ExecuteQuery(query).ToArray();
        }

        public TOut Query<TOut>(IQuery<T, TOut> query)
        {
            return ExecuteQuery(query);
        }

        public int Count(IQuery<T> query)
        {
            return ExecuteQuery(query).Count();
        }

        private IQueryable<T> ExecuteQuery(IQuery<T> query)
        {
            var merged = _unitOfWork.EnlistedAggregateRoots<T>().Union(_queryableSnapshot.Items<T>(), new AggregateRootIdEqualityComparer<T>());

            return query.Execute(merged)
                .Select(storedEntity => FindEnlistedEntityOrCloneSnapshottedEntity(storedEntity));
        }

        private TOut ExecuteQuery<TOut>(IQuery<T, TOut> query)
        {
            //TODO: this _doesn't_ enlist in the unitofwork if it wasn't already there, which isn't consistent.
            var merged = _unitOfWork.EnlistedAggregateRoots<T>().Union(_queryableSnapshot.Items<T>(), new AggregateRootIdEqualityComparer<T>());
            return query.Execute(merged);
        }

        private T FindEnlistedEntityOrCloneSnapshottedEntity(T storedEntity)
        {
            // is this entity already participating in our current transaction? return that if so
            var entityInTransaction = _unitOfWork.TryGetEnlistedAggregateRoot<T>(storedEntity.Id);
            if (entityInTransaction != null) return entityInTransaction;

            // otherwise do a binary clone and enlist the clone
            var clone = storedEntity.BinaryClone();
            _unitOfWork.EnlistAggregate(clone);
            return clone;
        }
    }
}
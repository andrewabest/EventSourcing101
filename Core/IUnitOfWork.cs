using System;
using System.Linq;
using Core.Domain;

namespace Core
{
    public interface IUnitOfWork
    {
        void EnlistAggregate(IAggregateRoot aggregateRoot);
        void Commit();
        T TryGetEnlistedAggregateRoot<T>(Guid id) where T : IAggregateRoot;
        IQueryable<T> EnlistedAggregateRoots<T>() where T : IAggregateRoot;
    }
}
using System;
using Core.Domain;

namespace Core
{
    public interface IRepository<T> where T : class, IAggregateRoot
    {
        T GetById(Guid id);
        void Add(T item);
        T[] Query(IQuery<T> query);
        TOut Query<TOut>(IQuery<T, TOut> query);
        int Count(IQuery<T> query);
    }
}
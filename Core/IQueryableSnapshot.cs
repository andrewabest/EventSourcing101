using System;
using System.Linq;
using Core.Domain;

namespace Core
{
    public interface IQueryableSnapshot
    {
        T GetById<T>(Guid id) where T : IAggregateRoot;
        IQueryable<T> Items<T>() where T : IAggregateRoot;
    }
}
using System.Collections.Generic;

namespace Core.Domain
{
    public class AggregateRootIdEqualityComparer<T> : IEqualityComparer<T> where T : IAggregateRoot
    {
        public bool Equals(T x, T y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(T obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
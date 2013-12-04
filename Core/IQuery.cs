using System.Linq;

namespace Core
{
    public interface IQuery<TIn, TOut>
    {
        TOut Execute(IQueryable<TIn> source);
    }

    public interface IQuery<T> : IQuery<T, IQueryable<T>>
    {

    }
}
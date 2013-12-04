using Core.Domain;

namespace Core
{
    public interface IAggregateBuilder
    {
        void Apply<T>(T aggregateRoot, IFact fact) where T : IAggregateRoot;
    }
}
using Core.Domain;

namespace Core
{
    public class AggregateBuilder : IAggregateBuilder
    {
        public void Apply<T>(T aggregateRoot, IFact fact) where T : IAggregateRoot
        {
            ((dynamic)aggregateRoot).Apply((dynamic)fact);
        }
    }
}
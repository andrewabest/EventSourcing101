using Core.Domain;

namespace Core
{
    public interface IDomainEventBroker
    {
        void Raise(IFact fact, IUnitOfWork unitOfWork);
    }
}
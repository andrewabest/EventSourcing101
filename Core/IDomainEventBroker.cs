using Core.Domain;

namespace Core
{
    public interface IDomainEventBroker
    {
        void Raise(IFact fact);
        void RaiseWithinUnitOfWork(IFact fact, IUnitOfWork unitOfWork);
    }
}
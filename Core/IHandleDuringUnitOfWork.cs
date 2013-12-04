using Core.Domain;

namespace Core
{
    public interface IHandle { }

    public interface IHandle<in TFact> : IHandle where TFact : IFact
    {
        void Handle(TFact message);
    }

    public interface IHandleDuringUnitOfWork<TFact> where TFact : IFact
    {
        void Handle(TFact fact);
    }
}
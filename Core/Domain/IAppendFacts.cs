namespace Core.Domain
{
    public interface IAppendFacts : IIdentifiable
    {
        void Append(IFact fact);
    }
}
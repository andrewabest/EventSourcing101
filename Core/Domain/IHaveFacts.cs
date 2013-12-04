using System.Collections.Generic;

namespace Core.Domain
{
    public interface IHaveFacts
    {
        IEnumerable<IFact> GetPendingFacts();
    }
}